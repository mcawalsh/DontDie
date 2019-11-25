using System.Collections;
using UnityEngine;

public class Knockback : MonoBehaviour
{
	public float knockbackThrust = 2;
	public float knockTime = 0.4f;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Enemy"))
		{
			Rigidbody2D enemy = other.gameObject.GetComponent<Rigidbody2D>();

			if (enemy != null)
			{
				enemy.isKinematic = false;
				Vector2 difference = enemy.transform.position - transform.position;
				difference = difference.normalized * knockbackThrust;

				enemy.AddForce(difference, ForceMode2D.Impulse);
				StartCoroutine(KnockbackRoutine(enemy));
			}
		}
	}

	private IEnumerator KnockbackRoutine(Rigidbody2D enemy)
	{
		if (enemy != null)
		{
			yield return new WaitForSeconds(knockTime);
			enemy.velocity = Vector2.zero;
			enemy.isKinematic = true;
		}
	}
}
