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

    float Speed;

    private void Start()
    {
        // アニメーターの取得
        anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        //Calculate the Input Magnitude
        Speed = new Vector2(horizontal, vertical).sqrMagnitude;
        Debug.Log("direction.magnitudeは" + Speed);
        if (Speed > 0.3f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg+cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        //Physically move player
        if (Speed > 0.3f)
        {
            anim.SetFloat("Speed", Speed,0.1f, Time.deltaTime);

        }
        else  
        {
            anim.SetFloat("Speed",0.0f);
        }



    }
}
