using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMove : MonoBehaviour
{
    // �L�����N�^�[�R���g���[���[
    [SerializeField] private CharacterController characterController;
    
    // �A�j���[�^�[
    private Animator animator;

    // �L�����N�^�[�̑��x
    private Vector3 velocity;

    // ��������
    [SerializeField] private float walkSpeed = 2f;

    // ���鑬��
    [SerializeField] private float runSpeed = 5f;

    /// <summary>
    /// �J�n����
    /// </summary>
    void Start()
    {
        // �L�����N�^�[�R���g���[���̎擾
        characterController = GetComponent<CharacterController>();
        
        // �A�j���[�^�[�̎擾
        animator = GetComponent<Animator>();
        
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    void Update()
    {
        Move();
        
    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        // ���������L�[
        float z = Input.GetAxisRaw("Vertical");
        // ����
        var direction = new Vector3(x, 0, z).normalized;
        // �ړ��p�L�[��������Ă����
        if (direction.magnitude >= 0.2f)
        {
            // ������ς���
            transform.rotation = Quaternion.LookRotation(direction);
            //animator.SetFloat("Speed", direction.magnitude);
            if (x > 0.5f || z > 0.5f || x < -0.5f || z < -0.5f)
            {
                animator.SetFloat("Speed", 1);
                characterController.Move(direction * runSpeed * Time.deltaTime);
            }
            else
            {
                animator.SetFloat("Speed", 0);
                characterController.Move(direction * walkSpeed * Time.deltaTime);
            }
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }

    }
}
