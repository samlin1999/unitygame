using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectClear : MonoBehaviour {


    ParticleSystem PS
    {
        get { return GetComponent<ParticleSystem>(); }
    }

	// Use this for initialization
	void Start () {
        Destroy(gameObject,PS.main.duration);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
