using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float speed;
	private PlayerState currentState;

	// private Vector2 moveInput;
	private Rigidbody2D rb;
	private Vector2 moveVelocity;

	public GameObject[] projectiles;

	private Animator animator;

	public float knockbackThrust = 5;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
	}

	void Update()
	{
		var moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		moveVelocity = moveInput.normalized * speed;
	}

	private IEnumerator AttackRoutine()
	{
		animator.SetBool("attacking", true);
		currentState = PlayerState.Attack;
		yield return null; // Wait 1 frame

		animator.SetBool("attacking", false);
		yield return new WaitForSeconds(.33f);

		currentState = PlayerState.Walk;
	}

	private void FireProjectile()
	{
		Instantiate(projectiles[0], transform.position, projectiles[0].transform.rotation);
	}

	// This is where you should put all your physics based adjustments
	private void FixedUpdate()
	{
		if (Input.GetButtonDown("Attack") && currentState != PlayerState.Attack)
		{
			StartCoroutine(AttackRoutine());
		}
		else if (moveVelocity != Vector2.zero)
		{
			rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
			animator.SetFloat("moveX", moveVelocity.x);
			animator.SetFloat("moveY", moveVelocity.y);
			animator.SetBool("moving", true);
		}
		else
		{
			animator.SetBool("moving", false);
		}
	}
}
