using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class FPSWalkerEnhanced : MonoBehaviour
{
	public float walkingSpeed = 7.5f;
	public float runningSpeed = 11.5f;
	public float jumpSpeed = 8.0f;
	public float gravity = 20.0f;
	public Camera playerCamera;
	public float lookSpeed = 2.0f;
	public float lookXLimit = 45.0f;

	CharacterController characterController;
	Vector3 moveDirection = Vector3.zero;
	float rotationX = 0;

	//Mobile controller
	[HideInInspector]
	public Vector2 joystick = Vector2.zero;
	[HideInInspector]
	public Vector2 lookJoystick = Vector2.zero;

	[HideInInspector]
	public bool canMove = true;
	internal bool shootAxis;

	public FootStepSounds FSsounds;


	void Start()
	{
		characterController = GetComponent<CharacterController>();

		// Lock cursor
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void Update()
	{
		float horizontalMoveAmount;
		float verticalMoveAmount;

		horizontalMoveAmount = Input.GetAxis("Horizontal");
		verticalMoveAmount = Input.GetAxis("Vertical");

		// We are grounded, so recalculate move direction based on axes
		Vector3 forward = transform.TransformDirection(Vector3.forward);
		Vector3 right = transform.TransformDirection(Vector3.right);

		// Press Left Shift to run
		bool isRunning = Input.GetKey(KeyCode.LeftShift);
		float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * horizontalMoveAmount : 0;
		float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * verticalMoveAmount : 0;

		float movementDirectionY = moveDirection.y;
		moveDirection = (forward * curSpeedY) + (right * curSpeedX);

		if (moveDirection.magnitude > 0)
		{
			if (isRunning)
			{
				FSsounds.distance += runningSpeed * Time.deltaTime;
			}
			else
			{
				FSsounds.distance += walkingSpeed * Time.deltaTime;
			}
		}
		else
		{
			FSsounds.distance = 0;
		}


		if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
		{
			moveDirection.y = jumpSpeed;
		}
		else
		{
			moveDirection.y = movementDirectionY;
		}

		// Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
		// when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
		// as an acceleration (ms^-2)
		if (!characterController.isGrounded)
		{
			moveDirection.y -= gravity * Time.deltaTime;
		}

		// Move the controller
		characterController.Move(moveDirection * Time.deltaTime);

		// Player and Camera rotation
		if (canMove)
		{
			float horizontalLookAmount;
			float verticalLookAmount;

			horizontalLookAmount = Input.GetAxis("Mouse X") * Time.deltaTime;
			verticalLookAmount = Input.GetAxis("Mouse Y") * Time.deltaTime;

			rotationX += -verticalLookAmount * lookSpeed;
			rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
			playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
			transform.rotation *= Quaternion.Euler(0, horizontalLookAmount * lookSpeed, 0);
		}
	}
}