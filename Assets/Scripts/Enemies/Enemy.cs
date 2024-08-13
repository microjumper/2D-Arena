using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static event Action<Enemy> OnEnemyDeath;

    [SerializeField]
    private float speed = 3f;

    private new Rigidbody2D rigidbody;
    private Animator animator;
    private new BoxCollider2D collider;
    private SpriteRenderer spriteRenderer;

    private Transform target;
    private bool hasReachedTarget;
    private bool isTakingDamage;
    private int health;

    private const int damage = 5;

    public int Points { get; } = 5;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();    
        animator = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        target = FindFirstObjectByType<PlayerController>().gameObject.transform;

        hasReachedTarget = false;

        isTakingDamage = false;

        health = 30;
    }

    private void OnDisable()
    {
        transform.localScale = Vector3.one;
        collider.enabled = true;
        spriteRenderer.color = Color.white;
    }

    private void FixedUpdate()
    {
        if(GameManager.Instance.Running)
        {
            if (!hasReachedTarget && !isTakingDamage)
            {
                ChaseTarget();
            }
        }
    }

    private void ChaseTarget()
    {
        Vector2 direction = ((Vector2)target.position - rigidbody.position).normalized;
        rigidbody.MovePosition(rigidbody.position + speed * Time.fixedDeltaTime * direction);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out PlayerController playerController))
        {
            if(!hasReachedTarget)
            {
                hasReachedTarget = true;

                playerController.TakeDamage(damage, transform.position);

                StartCoroutine(Cooldown(0.75f));
            }
        }
    }

    private IEnumerator Cooldown(float duration)
    {
        yield return new WaitForSeconds(duration);

        hasReachedTarget = false;
    }

    public void TakeDamage(int damage, Vector2 damageSource)
    {
        if(isTakingDamage)
        {
            return;
        }

        isTakingDamage = true;
        
        health -= damage;

        if(health <= 0)
        {
            animator.SetTrigger(AnimatorParameter.Die);

            return;
        }

        animator.SetBool(AnimatorParameter.TakingDamage, isTakingDamage);

        KnockbackHandler.ApplyKnockback(rigidbody, 20f, damageSource, 0.5f, () =>
        {
            isTakingDamage = false;

            animator.SetBool(AnimatorParameter.TakingDamage, isTakingDamage);
        });
    }

    public void EmitDeathEvent() => OnEnemyDeath?.Invoke(this);
}
