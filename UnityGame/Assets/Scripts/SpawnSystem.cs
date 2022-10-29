using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem : MonoBehaviour {
    public MonsterCtrl monsterObj;//怪物物件
    /// <summary>
    /// 生怪位置點
    /// </summary>
    List<Transform> points = new List<Transform>();
    /// <summary>
    /// 是否直接生怪
    /// </summary>
    public bool auto;
	void Start () {
	    for(int i = 0;i < transform.childCount;i++)
        {
            points.Add(transform.GetChild(i));//將位置點加入清單
        }
        if(auto)
        {
            Spawn();
        }
	}
	/// <summary>
    /// 自動生成怪物物件
    /// </summary>
    public void Spawn()
    {
        for(int i = 0;i < transform.childCount;i++)
        {
            Instantiate(monsterObj, points[i].position, points[i].rotation);
        }
    }
}
