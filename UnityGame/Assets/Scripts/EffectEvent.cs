using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectEvent : MonoBehaviour {

    public GameObject effect;
    public Transform pos;

    Vector3 effectPos;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CallEffect()
    {
        Instantiate(effect, pos.position, pos.rotation);
    }
}
