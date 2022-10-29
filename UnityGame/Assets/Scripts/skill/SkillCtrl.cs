using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCtrl : MonoBehaviour
{
    public SkilllBase skilllBase;
    
    void Start()
    {
        if (skilllBase) skilllBase.ActionStart();
        else Debug.Log("技能腳本未設置");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (skilllBase == null) return;
        skilllBase.Action();
    }
}