using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    // 音を鳴らす
    // AudioSource:: スピーカー
    // AudioClip:: CD（素材）

    //BGM
    [SerializeField] AudioSource audioSourceBGM;
    [SerializeField] AudioClip[] audioClipsBGM;

    //SE
    [SerializeField] AudioSource audioSourceSE;
    [SerializeField] AudioClip[] audioClipsSE;

    private const float BGM_VOLUME_DEFULT = 1.0f;
    private const float SE_VOLUME_DEFULT = 1.0f;

    /// <summary>
    /// BGMの列挙型
    /// </summary>
    public enum BGM
    {
        Title, // タイトル画面用の曲
        Main   // ゲーム画面用の曲
    }

    /// <summary>
    /// BGMの列挙型
    /// </summary>
    public enum SE
    {
        Destoy, // ボールが破裂するとき
        Touch,   // ボールに触れた時
        Explosion, // 爆弾が爆発するとき
        Decision, // ボタンの決定音
        Close, // ボタンの閉じる音
        CountDownSe,// カウントダウンの音
        CountZero,// カウント0の音
    }

    // シングルトンにする
    // ゲーム内にただ一つのだけのもの（シーンが変わっても破壊されない）
    // どのコードからもアクセスしやすい
    public static SoundManager instance;
    /// <summary>
    ///  シングルトンのお約束
    /// </summary>
    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        // 音量データの読み込み（からの場合は１を入れる）
       audioSourceBGM.volume = PlayerPrefs.GetFloat("BGM_VOLUME", BGM_VOLUME_DEFULT);
       audioSourceSE.volume = PlayerPrefs.GetFloat("SE_VOLUME", SE_VOLUME_DEFULT);
    }

    /// <summary>
    /// BGMの再生
    /// </summary>
    public void PlayBGM(BGM bgm)
    {
        // 列挙型から流したいBGMを選ぶ（intでキャスト）
        audioSourceBGM.clip = audioClipsBGM[(int)bgm];
        audioSourceBGM.Play();
       
    }

    public void StopBgm()
    {
        audioSourceBGM.Stop();
    }

    /// <summary>
    /// SEの再生
    /// </summary>
    /// <param name="se"></param>
    public void PlaySE(SE se)
    {
        audioSourceSE.PlayOneShot(audioClipsSE[(int)se]);
        
    }

    /// <summary>
    /// BGMの音量変更
    /// </summary>
    /// <param name="BGMVolume"> スライダーのボリューム </param>
    public void ChangeVolumeBGM(float BGMVolume)
    {
        audioSourceBGM.volume = BGMVolume;
        
        // 音量の保存
        PlayerPrefs.SetFloat("BGM_VOLUME", audioSourceBGM.volume);
       
    }

    /// <summary>
    /// SEの音量変更
    /// </summary>
    /// <param name="SEVolume"> スライダーのボリューム </param>
    public void ChangeVolumeSE(float SEVolume)
    {
        audioSourceSE.volume = SEVolume;

        // 音量の保存
        PlayerPrefs.SetFloat("SE_VOLUME", audioSourceSE.volume);

    }
}
