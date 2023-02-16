using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMove : MonoBehaviour
{
    // キャラクターコントローラー
    [SerializeField] private CharacterController characterController;
    
    // アニメーター
    private Animator animator;

    // キャラクターの速度
    private Vector3 velocity;

    // 歩く速さ
    [SerializeField] private float walkSpeed = 2f;

    // 走る速さ
    [SerializeField] private float runSpeed = 5f;

    /// <summary>
    /// 開始処理
    /// </summary>
    void Start()
    {
        // キャラクターコントローラの取得
        characterController = GetComponent<CharacterController>();
        
        // アニメーターの取得
        animator = GetComponent<Animator>();
        
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        Move();
        
    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        // 垂直方向キー
        float z = Input.GetAxisRaw("Vertical");
        // 方向
        var direction = new Vector3(x, 0, z).normalized;
        // 移動用キーが押されていれば
        if (direction.magnitude >= 0.2f)
        {
            // 向きを変える
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
