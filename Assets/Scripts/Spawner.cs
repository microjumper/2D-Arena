using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private Transform[] spawnPoints;
    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private float time = 1;
    [SerializeField]
    private float repeatRate = 5;

    private const int capacity = 10;

    private Stack<GameObject> objectPool;
    private GameObject poolItemContainer;

    private void Awake()
    {
        objectPool = new (capacity);
    }

    private void OnEnable() => Enemy.OnEnemyDeath += SendBackToPool;

    private void OnDisable() => Enemy.OnEnemyDeath -= SendBackToPool;

    private void Start()
    {
        poolItemContainer = new($"{enemyPrefab.name}_Pool");

        InitPool();

        StartCoroutine(SpawnRoutine());
    }

    private void InitPool()
    {
        for (int i = 0; i < capacity; i++)
        {
            GameObject poolItem = Instantiate(enemyPrefab);
            poolItem.transform.parent = poolItemContainer.transform;
            poolItem.SetActive(false);

            objectPool.Push(poolItem);
        }
    }

    private IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(time);

        while (GameManager.Instance.Running)
        {
            Spawn();

            yield return new WaitForSeconds(repeatRate);
        }
    }

    private void Spawn()
    {
        if(objectPool.Count > 0)
        {
            int index = Random.Range(0, spawnPoints.Length);

            GameObject poolItem = objectPool.Pop();

            poolItem.transform.position = spawnPoints[index].position;
            poolItem.transform.rotation = Quaternion.identity;

            poolItem.SetActive(true);
        }
    }

    private void SendBackToPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);

        objectPool.Push(enemy.gameObject);
    }
}