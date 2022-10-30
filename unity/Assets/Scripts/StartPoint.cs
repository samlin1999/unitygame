using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour {

	public static StartPoint ctrl;
	public Vector3 pos
	{
		get { return transform.position; }
	}
    public Quaternion rotation
    {
        get { return transform.rotation; }
    }
	private void Awake()
	{
		ctrl = this;
	}

}
