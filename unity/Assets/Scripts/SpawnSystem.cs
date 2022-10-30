using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 生怪系統
/// </summary>
public class SpawnSystem : MonoBehaviour {
    public GameObject spawnobj;
    public List<Transform> points
    {
        get { return new List<Transform>(GetComponentsInChildren<Transform>()); }
    }
    private bool spawnSwitch = true;
    public void spawn()
    {
        if(spawnSwitch)
        {
            spawnSwitch = false;
            for(int i = 1;i < points.Count;i++)
            {
                Instantiate(spawnobj,points[i].position,points[i].rotation);
            }
        }
        
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
