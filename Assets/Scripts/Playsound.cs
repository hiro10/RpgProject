using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playsound : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // ƒƒCƒ“BGM‚ğ—¬‚·
        SoundManager.instance.PlayBGM(SoundManager.BGM.Title);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
