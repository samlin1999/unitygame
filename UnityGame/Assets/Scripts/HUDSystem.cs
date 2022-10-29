using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDSystem : MonoBehaviour {
    public static HUDSystem ctrl;
    public Image hpBar;
    public Text hpText, moneyText;
    private void Awake()
    {
        ctrl = this;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void UpdateHp(float hp,float hpMax)
    {
        hpBar.fillAmount = hp / hpMax;
        hpText.text = hp.ToString() + " / " + hpMax.ToString();
    }
    public void UpdateMoney(int money)
    {
        moneyText.text = money.ToString();
    }
}
