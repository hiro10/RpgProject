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

    // ワープの発動判定
    public bool isWarp;

    // 剣
    [SerializeField] Transform sword;

    // 剣の初期位置格納用
    private Vector3 swordOrigRot;
    private Vector3 swordOrigPos;

    // 剣の親(持ち手)
    public Transform swordHand;

    // マテリアル
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
    /// 開始処理
    /// </summary>
    void Start()
    {
        thirdPerson = GetComponent<ThirdPersonMovement>();
        animator = GetComponent<Animator>();

        // 剣の位置を記憶させる
        swordOrigRot = sword.localEulerAngles;
        swordOrigPos = sword.localPosition;

        // ワープの仕様判定を設定
        isWarp = false;

        gameObjectcam.SetActive(true);

        postVolume = Camera.main.GetComponent<PostProcessVolume>();
        postProfile = postVolume.profile;
    }

    // Update is called once per frame
    void Update()
    {
        // ワープ後の剣のローテションがおかしくなる処理の応急処置
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
    /// ワープ処理(Domove)
    /// </summary>
    public void Warp()
    {
        
        // 残像用
        // コンポーネントを外したクローンを表示
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

        // 体を消す
        ShowBody(false);
        //thirdPerson.SetGravityOnTrigger(false);
        // アニメーションを止める
        animator.speed = 0f;

        // ワープ処理：イーじんぐ処理後で理解
        transform.DOMove(target.position, warpDuration).SetEase(Ease.InExpo).OnComplete(() => FinshWarp());

        // 親をnullにする
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
    /// ワープ中に姿を消す処理
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
    /// ワープ終了時の処理
    /// </summary>
    void FinshWarp()
    {
        // 剣の親と位置の再設定
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
    /// パーティ黒を止める
    /// </summary>
    /// <returns></returns>
    IEnumerator StopParticles()
    {
        yield return new WaitForSeconds(.2f);
        blueTrail.Stop();
        whiteTrail.Stop();
    }

}
