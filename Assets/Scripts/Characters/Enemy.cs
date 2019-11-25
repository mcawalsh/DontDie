using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public int health;
	public string enemyName;
	public int baseAttack;
	public float moveSpeed;

	public Material matWhite;
	private Material matDefault;
	private SpriteRenderer sr;

	public virtual void Start()
	{
		sr = GetComponent<SpriteRenderer>();
		matDefault = sr.material;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Weapon"))
		{
			health--;

			sr.material = matWhite;

			if (health <= 0)
			{
				// Die();
			}
			else
			{
				StartCoroutine(ResetMaterial());
			}
		}
	}

	private IEnumerator ResetMaterial()
	{
		yield return new WaitForSeconds(0.2f);
		sr.material = matDefault;
	}

	private void Die()
	{
		Destroy(this.gameObject);
	}
}
