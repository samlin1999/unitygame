using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ISystem : MonoBehaviour {

    public GameManager GM
    {
        get { return GameManager.ctrl; }
    }
    /// <summary>
    /// 初始化
    /// </summary>
    public abstract void Inital();//<=不可有內容,被繼承者必須定義方法內容，用override覆寫

}
