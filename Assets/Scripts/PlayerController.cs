using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public Camera camera;
	public float speed;

	// private Vector2 moveInput;
	private Rigidbody2D rb;
	private Vector2 moveVelocity;

	public GameObject[] projectiles;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		var moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		moveVelocity = moveInput.normalized * speed;
	}

	private void FireProjectile()
	{
		Instantiate(projectiles[0], transform.position, projectiles[0].transform.rotation);
	}

	// This is where you should put all your physics based adjustments
	private void FixedUpdate()
	{
		rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
	}
}
