using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    Animator anim;

    Quaternion targetRotation;

    WarpConntroller warp;
    // Start is called before the first frame update
    void Awake()
    {
        warp = GetComponent<WarpConntroller>();
        TryGetComponent(out anim);

        targetRotation = transform.rotation;
    }


    void Update()
    {
        if (anim == null || anim.applyRootMotion == false)
        {
            return;
        }
        // �ړ��x�N�g���̎擾
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var rotationSpeed = 600 * Time.deltaTime;
        var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
        var velocity = horizontalRotation * new Vector3(horizontal, 0f, vertical).normalized;
        var speed = Input.GetKey(KeyCode.LeftShift) ? 2 : 1;


        // �ړ�����������
        if (velocity.magnitude > 0.5f)
        {
            transform.rotation = Quaternion.LookRotation(velocity, Vector3.up);
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);

        // �ړ����x���A�j���[�^�[�ɔ��f
        anim.SetFloat("Speed", velocity.magnitude * speed, 0.1f, Time.deltaTime);

        
    }


}
