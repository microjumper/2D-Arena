using UnityEngine;
using System.Collections;
using System;

public static class KnockbackHandler
{
    public static void ApplyKnockback(Rigidbody2D rigidbody, float knockbackForce, Vector2 knockbackOrigin, float duration, Action callback = null)
    {
        (float Angular, float Linear) dampings = (rigidbody.angularDamping, rigidbody.linearDamping);

        rigidbody.angularDamping = knockbackForce * 0.5f;
        rigidbody.linearDamping = knockbackForce * 0.5f;

        Vector2 direction = ((Vector2)rigidbody.transform.position - knockbackOrigin).normalized;

        rigidbody.AddForce(direction * knockbackForce, ForceMode2D.Impulse);

        rigidbody.GetComponent<MonoBehaviour>().StartCoroutine(ResetDampingAfterKnockback(rigidbody, duration, dampings, callback));
    }

    private static IEnumerator ResetDampingAfterKnockback(Rigidbody2D rigidbody, float duration, (float Angular, float Linear) dampings, Action callback)
    {
        yield return new WaitForSeconds(duration);

        rigidbody.angularDamping = dampings.Angular;
        rigidbody.linearDamping = dampings.Linear;

        callback?.Invoke();
    }
}
