using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    [SerializeField] CharacterController controller;

    // 速さ
    public float speed = 6f;

    public float turnSmoothTime = 0.1f;

    float turnSmoothVelocity;

    public Transform cam;
    Animator anim;


    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    Vector3 direction;

    [SerializeField] WarpConntroller warpConntroller;

    private bool gravityOntrigger;
    public bool GetGravityOnTrigger()
    {
        return gravityOntrigger;
    }
    public void SetGravityOnTrigger(bool trigger)
    {
        this.gravityOntrigger = trigger;
    }
    private void Start()
    {
        gravityOntrigger = true;
        // アニメーターの取得
        anim = this.GetComponent<Animator>();
        warpConntroller = GetComponent<WarpConntroller>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
    }


    void PlayerMove()
    {
        if (warpConntroller.isWarp == false)
        {
            if (controller.isGrounded)
            {
                float horizontal = Input.GetAxisRaw("Horizontal");

                float vertical = Input.GetAxisRaw("Vertical");

                direction = new Vector3(horizontal, 0, vertical).normalized;

                float Speed;
                //Calculate the Input Magnitude
                Speed = new Vector2(horizontal, vertical).sqrMagnitude;

                if (Speed > 0.3f)
                {
                    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);

                    Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
                    controller.Move(moveDir.normalized * speed * Time.deltaTime);
                }

                if (Input.GetButton("Jump"))
                {
                    direction.y = jumpSpeed;
                }

                //Physically move player
                if (Speed > 0.3f)
                {
                    anim.SetFloat("Speed", Speed, 0.1f, Time.deltaTime);

                }
                else
                {
                    anim.SetFloat("Speed", 0.0f);
                }
            }
        }
        if (gravityOntrigger == true)
        {
            direction.y = direction.y - (gravity * Time.deltaTime);
            controller.Move(direction * Time.deltaTime);
        }
    }
}
