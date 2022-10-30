using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewCtrl : MonoBehaviour {
    public bool loop;
    private int count
    {
        get { return transform.childCount - 1; }
    }

    public RectTransform parentRT;
    public RectTransform RT
    {
        get { return GetComponent<RectTransform>(); }
    }
    private float cellSize
    {
        get { return parentRT.sizeDelta.x; }
    }
    private Vector2 size
    {
        get { return new Vector2(cellSize * count, 0); }
    }
	void Start () {
        RT.sizeDelta = size;
	}
    private Vector2 pos;
    private int index = 0;
	// Update is called once per frame
	void Update () {
		

	}

    public void Left()
    {
        index--;
        index = (index < 0) ? (loop ? (count - 1) : 0) : index;
        pos.x = -cellSize * index;
        RT.anchoredPosition = pos;
    }
    public void Right()
    {
        index++;
        index = (index >= count) ? (loop ? 0 : (count - 1)) : index;
        pos.x = -cellSize * index;
        RT.anchoredPosition = pos;
    }
}
