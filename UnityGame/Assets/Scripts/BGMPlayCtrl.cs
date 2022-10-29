using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMPlayCtrl : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Invoke("PlayBGM",1);
	}
	
	
	void PlayBGM () {
        GameManager.ctrl.PlayBGM(SceneManager.GetActiveScene().name);
	}
}
