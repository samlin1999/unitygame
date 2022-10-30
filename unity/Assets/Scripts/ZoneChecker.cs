using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(BoxCollider))]
public class ZoneChecker : MonoBehaviour {
    public UnityEvent events;
    public BoxCollider touch
    {
        get { return GetComponent<BoxCollider>(); }
    }
    public GameObject wall;
    public GameObject linkObj;

    private void OnEnable()
    {
        if (linkObj) linkObj.SetActive(true);
    }
    void Start () {
		
	}
	

	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")//只會和Player標籤的物件互動
        {
            events.Invoke();
            //print(other.name);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            touch.isTrigger = false;
            //Instantiate(wall);
        }
    }
}
