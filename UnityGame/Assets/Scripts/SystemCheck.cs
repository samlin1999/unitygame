using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemCheck : MonoBehaviour {
    public GameObject GM;

    /// <summary>
    /// 搜尋GM是否存在場上
    /// </summary>
    private void Awake()
    {
        if(!FindObjectOfType<GameManager>())
        {
            Instantiate(GM);
        }
    }
}
