using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering.PostProcessing;

public class WarpConntroller : MonoBehaviour
{
    Animator animator;
    public Transform target;

    public float warpDuration = .5f;

    // ���[�v�̔�������
    public bool isWarp;

    // ��
    [SerializeField] Transform sword;

    // ���̏����ʒu�i�[�p
    private Vector3 swordOrigRot;
    private Vector3 swordOrigPos;

    // ���̐e(������)
    public Transform swordHand;

    // �}�e���A��
    [Space]
    public Material glowMaterial;
    public Material endMaterial;
    ThirdPersonMovement thirdPerson;

    [SerializeField] GameObject gameObjectcam;

    [Header("Particles")]
    public ParticleSystem blueTrail;
    public ParticleSystem whiteTrail;
    public ParticleSystem swordParticle;

    private PostProcessVolume postVolume;
    private PostProcessProfile postProfile;

    /// <summary>
    /// �J�n����
    /// </summary>
    void Start()
    {
        thirdPerson = GetComponent<ThirdPersonMovement>();
        animator = GetComponent<Animator>();

        // ���̈ʒu���L��������
        swordOrigRot = sword.localEulerAngles;
        swordOrigPos = sword.localPosition;

        // ���[�v�̎d�l�����ݒ�
        isWarp = false;

        gameObjectcam.SetActive(true);

        postVolume = Camera.main.GetComponent<PostProcessVolume>();
        postProfile = postVolume.profile;
    }

    // Update is called once per frame
    void Update()
    {
        // ���[�v��̌��̃��[�e�V���������������Ȃ鏈���̉��}���u
        if (sword.localEulerAngles != swordOrigRot)
        {
            sword.localEulerAngles = swordOrigRot;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.transform.LookAt(target.position);
            animator.applyRootMotion = false;
           // gameObjectcam.SetActive(false);
            isWarp = true;
            swordParticle.Play();
            animator.SetTrigger("slash");
        }
    }

    /// <summary>
    /// ���[�v����(Domove)
    /// </summary>
    public void Warp()
    {
        
        // �c���p
        // �R���|�[�l���g���O�����N���[����\��
        GameObject clone = Instantiate(this.gameObject, transform.position, transform.rotation);
        Destroy(clone.GetComponent<WarpConntroller>().sword.gameObject);
        Destroy(clone.GetComponent<Animator>());
        Destroy(clone.GetComponent<WarpConntroller>());
        Destroy(clone.GetComponent<ThirdPersonMovement>());

        SkinnedMeshRenderer[] skinMeshList = clone.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer smr in skinMeshList)
        {
            smr.material = glowMaterial;
            smr.material.DOFloat(2, "_AlphaThreshold", 5f).OnComplete(() => Destroy(clone));
        }

        // �̂�����
        ShowBody(false);
        //thirdPerson.SetGravityOnTrigger(false);
        // �A�j���[�V�������~�߂�
        animator.speed = 0f;

        // ���[�v�����F�C�[���񂮏�����ŗ���
        transform.DOMove(target.position, warpDuration).SetEase(Ease.InExpo).OnComplete(() => FinshWarp());

        // �e��null�ɂ���
        sword.parent = null;
        sword.DOMove(target.position, warpDuration / 2);
        sword.DOLookAt(target.position, .2f, AxisConstraint.None);
        //sword.DORotate(new Vector3(0, 90, 0), 0.3f);

        //Particles
        blueTrail.Play();
        whiteTrail.Play();

        //Lens Distortion
        DOVirtual.Float(0, -80, .2f, DistortionAmount);
        DOVirtual.Float(1, 2f, .2f, ScaleAmount);
    }

    void DistortionAmount(float x)
    {
        postProfile.GetSetting<LensDistortion>().intensity.value = x;
    }
    void ScaleAmount(float x)
    {
        postProfile.GetSetting<LensDistortion>().scale.value = x;
    }


    /// <summary>
    /// ���[�v���Ɏp����������
    /// </summary>
    /// <param name="state"></param>
    private void ShowBody(bool state)
    {
        SkinnedMeshRenderer[] skinnedList = GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (SkinnedMeshRenderer smr in skinnedList)
        {
            smr.enabled = state;
        }
    }

    /// <summary>
    /// ���[�v�I�����̏���
    /// </summary>
    void FinshWarp()
    {
        // ���̐e�ƈʒu�̍Đݒ�
        sword.parent = swordHand;
        sword.localPosition = swordOrigPos;
        sword.localEulerAngles = swordOrigRot;

        SkinnedMeshRenderer[] skinMeshList = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer smr in skinMeshList)
        {
            GlowAmount(30);
            DOVirtual.Float(30, 0, .5f, GlowAmount);
        }
        animator.speed = 1f;
        ShowBody(true);
        isWarp = false;
        gameObjectcam.SetActive(true);
        StartCoroutine(StopParticles());


        animator.applyRootMotion = true;
    }

    void GlowAmount(float x)
    {
        SkinnedMeshRenderer[] skinMeshList = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer smr in skinMeshList)
        {
            smr.material = endMaterial;
            smr.material.SetVector("_FresnelAmount", new Vector4(x, x, x, x));
        }
    }

    /// <summary>
    /// �p�[�e�B�����~�߂�
    /// </summary>
    /// <returns></returns>
    IEnumerator StopParticles()
    {
        yield return new WaitForSeconds(.2f);
        blueTrail.Stop();
        whiteTrail.Stop();
    }

}
