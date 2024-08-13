using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController: MonoBehaviour
{
    public static event Action<int> OnPlayerHeathChange;

    [SerializeField]
    private float speed = 5f;

    // references
    private new Rigidbody2D rigidbody;
    private Animator animator;
    private PlayerInput playerInput;

    // fields
    private Vector2 direction;
    private bool isAttacking;
    private int health;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        playerInput.enabled = true;

        direction = Vector2.zero;

        isAttacking = false;

        health = 50;
        OnPlayerHeathChange?.Invoke(health);
    }

    private void FixedUpdate()
    {
        if(!IsTakingDamage())
        {
            HandleMovement();
        }
    }

    private void HandleMovement()
    {
        if(isAttacking)
        {
            rigidbody.linearVelocity = Vector2.zero;
        }
        else
        {
            rigidbody.linearVelocity = direction * speed;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        switch(context.phase)
        {
            case InputActionPhase.Performed:
                direction = context.ReadValue<Vector2>();
                transform.localScale = new(Mathf.Sign(direction.x), 1, 1);
                break;

            case InputActionPhase.Canceled:
                direction = Vector2.zero;
                break;
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if(isAttacking || IsTakingDamage())
        {
            return;
        }

        if(context.performed)
        {
            isAttacking = true;

            animator.SetTrigger(AnimatorParameter.Attack);

            StartCoroutine(Cooldown());
        }
    }

    private bool IsTakingDamage() => animator.GetBool(AnimatorParameter.TakingDamage);

    public void TakeDamage(int damage, Vector2 damageSource)
    {
        if(!IsTakingDamage())
        {
            StopAllCoroutines();

            isAttacking = false;

            health -= damage;
            OnPlayerHeathChange?.Invoke(health);

            if (health <= 0)
            {
                playerInput.enabled = false;

                return;
            }

            animator.SetBool(AnimatorParameter.TakingDamage, true);

            KnockbackHandler.ApplyKnockback(rigidbody, 25f, damageSource, 0.5f, () => animator.SetBool(AnimatorParameter.TakingDamage, false));
        }
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(0.25f);

        isAttacking = false;
    }
}
