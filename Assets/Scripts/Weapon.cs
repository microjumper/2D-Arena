using UnityEngine;

public class Weapon : MonoBehaviour
{
    private const int damage = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(damage, transform.position);
        }
    }
}
