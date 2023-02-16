using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float velocity = 9;
	[Space]

	public float InputX;
	public float InputZ;
	public Vector3 desiredMoveDirection;
	public bool blockRotationPlayer;
	public float desiredRotationSpeed = 0.1f;
	public Animator anim;
	public float Speed;
	public float allowPlayerRotation = 0.1f;
	public Camera cam;
	public CharacterController controller;
	public bool isGrounded;

	[Header("Animation Smoothing")]
	[Range(0, 1f)]
	public float HorizontalAnimSmoothTime = 0.2f;
	[Range(0, 1f)]
	public float VerticalAnimTime = 0.2f;
	[Range(0, 1f)]
	public float StartAnimTime = 0.3f;
	[Range(0, 1f)]
	public float StopAnimTime = 0.15f;


	private float verticalVel;
	private Vector3 moveVector;
	public bool canMove;

	// Use this for initialization
	void Start()
	{
		// �A�j���[�^�[�̎擾
		anim = this.GetComponent<Animator>();
		// ���C���J�����̎擾
		cam = Camera.main;
		// �L�����R���̎擾
		controller = this.GetComponent<CharacterController>();
	}

	// Update is called once per frame
	void Update()
	{
		
		// �J�����������ĂȂ�������߂�
		if (!canMove)
		{
			return;
		}
		InputMagnitude();

		//If you don't need the character grounded then get rid of this part.
		//isGrounded = controller.isGrounded;
		//if (isGrounded) {
		//	verticalVel -= 0;
		//} else {
		//	verticalVel -= .05f * Time.deltaTime;
		//}
		//moveVector = new Vector3 (0, verticalVel, 0);
		//controller.Move (moveVector);

		//Updater
	}

	void PlayerMoveAndRotation()
	{
		InputX = Input.GetAxis("Horizontal");
		InputZ = Input.GetAxis("Vertical");

		var camera = Camera.main;
		var forward = cam.transform.forward;
		var right = cam.transform.right;

		forward.y = 0f;
		right.y = 0f;

		forward.Normalize();
		right.Normalize();

		desiredMoveDirection = forward * InputZ + right * InputX;

		if (blockRotationPlayer == false)
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), desiredRotationSpeed);
			controller.Move(desiredMoveDirection * Time.deltaTime * velocity);
		}
	}

	public void RotateToCamera(Transform t)
	{

		var camera = Camera.main;
		var forward = cam.transform.forward;
		var right = cam.transform.right;

		desiredMoveDirection = forward;

		t.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), desiredRotationSpeed);
	}

	public void RotateTowards(Transform t)
	{
		transform.rotation = Quaternion.LookRotation(t.position - transform.position);

	}

	void InputMagnitude()
	{
		// ����
		InputX = Input.GetAxis("Horizontal");
		InputZ = Input.GetAxis("Vertical");

		anim.SetFloat ("InputZ", InputZ, VerticalAnimTime, Time.deltaTime * 2f);
		anim.SetFloat ("InputX", InputX, HorizontalAnimSmoothTime, Time.deltaTime * 2f);

		//Calculate the Input Magnitude
		Speed = new Vector2(InputX, InputZ).sqrMagnitude;

		//Physically move player
		if (Speed > allowPlayerRotation)
		{
			anim.SetFloat ("Speed", Speed, StartAnimTime, Time.deltaTime);
			PlayerMoveAndRotation();
		}
		else if (Speed < allowPlayerRotation)
		{
			anim.SetFloat ("Speed", Speed, 0f, Time.deltaTime);
		}
	}
}
