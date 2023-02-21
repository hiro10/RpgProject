using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOn : MonoBehaviour
{

    [SerializeField] GameObject Cm1;
    [SerializeField] GameObject Cm2;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            Cm2.SetActive(true);
        }
        else if(Input.GetKeyDown(KeyCode.P))
        {
            Cm2.SetActive(false);
        }
    }
}
