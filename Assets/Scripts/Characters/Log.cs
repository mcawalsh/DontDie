using UnityEngine;

public class Log : Enemy
{
	public Transform target;
	public float chaseRadius;
	public float attackRadius;
	public Transform homePosition;
	private Animator animator;

    void Start()
    {
		target = GameObject.FindWithTag("Player").transform;

		animator = GetComponent<Animator>();
    }

    void Update()
    {
		CheckDistance();
    }

	void CheckDistance()
	{
		float currentDistance = Vector3.Distance(target.position, transform.position);

		if (currentDistance <= chaseRadius && currentDistance >= attackRadius)
		{
			animator.SetBool("wakeUp", true);
			transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
		} else
		{
			animator.SetBool("wakeUp", false);
		}
	}
}
