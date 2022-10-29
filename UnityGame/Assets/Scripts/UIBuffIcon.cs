using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBuffIcon : MonoBehaviour {
    public Image iconImg;
    public Image cdImg;
    public Text cdText;
    public float duration, time;

    public void SetBuffData(Buff buff)
    {
        gameObject.SetActive(true);
        this.duration = buff.duration;
        iconImg.sprite = Resources.Load<Sprite>(buff.IconCode);
    }
	
    public void UpdateBuffIcon(float time)
    {
        this.time = time;
        cdImg.fillAmount = 1f - (this.time / duration);
        cdText.text = this.time.ToString("F0");

    }
}
