using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    private TextMeshProUGUI textMesh;

    private void Awake() => textMesh = GetComponent<TextMeshProUGUI>();

    private void OnEnable() => Enemy.OnEnemyDeath += UpdateScore;

    private void OnDisable()
    {
        textMesh.text = "0";

        Enemy.OnEnemyDeath -= UpdateScore;
    }

    private void UpdateScore(Enemy enemy)
    {
        int score = int.Parse(textMesh.text);

        textMesh.text = (score + enemy.Points).ToString();
    }
}
