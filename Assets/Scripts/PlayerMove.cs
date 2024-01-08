using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
	Rigidbody2D rigidbody2D;
	[HideInInspector]
	public Vector3 movementVector;
	[HideInInspector]
	public float lastHorizontalDeCoupledVector;
	[HideInInspector]
	public float lastVerticalDeCoupledVector;

	[HideInInspector]
	public float lastHorizontalCoupledVector;
	[HideInInspector]
	public float lastVerticalCoupledVector;

	[SerializeField] float speed = 3f;

	Animate animate;

	// Start is called before the first frame update
	private void Awake()
	{
		rigidbody2D = GetComponent<Rigidbody2D>();
		movementVector = new Vector3();
		animate = GetComponent<Animate>();
	}

	private void Start()
	{
		lastHorizontalDeCoupledVector = -1f;
		lastVerticalDeCoupledVector = 1f;

		lastVerticalCoupledVector = 1f;
		lastHorizontalCoupledVector = -1f;
	}

	// Update is called once per frame
	void Update()
	{
		movementVector.x = Input.GetAxisRaw("Horizontal");
		movementVector.y = Input.GetAxisRaw("Vertical");

		if (movementVector.x != 0 || movementVector.y != 0)
		{
			lastHorizontalCoupledVector = movementVector.x;
			lastVerticalCoupledVector = movementVector.y;
		}

		if (movementVector.x != 0)
		{
			lastHorizontalDeCoupledVector = movementVector.x;
		}
		if (movementVector.y != 0)
		{
			lastVerticalDeCoupledVector = movementVector.y;
		}

		movementVector *= speed;

		animate.horizontal = movementVector.x;

		rigidbody2D.velocity = movementVector;
	}
}
