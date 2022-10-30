using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharacterManager : MonoBehaviour {
    public GameObject[] models;
    public CharaterData charaterData;
	public GameDB DB;
    //public ToggleGroup toggleGroup;
	// Use this for initialization
	void Start () {
        charaterData.DataSet(0);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void SelectCharater(Toggle toggle)
    {
        switch (toggle.name)
        {
			case "A":
			models [(int)ClassType.Warrior].SetActive (toggle.isOn);
			charaterData.DataSet ((int)ClassType.Warrior);
			DB.classType = ClassType.Warrior;
                break;
            case "B":
                models[(int)ClassType.Archer].SetActive(toggle.isOn);
                charaterData.DataSet((int)ClassType.Archer);
			DB.classType = ClassType.Archer;
                break;
            case "C":
                models[(int)ClassType.Mage].SetActive(toggle.isOn);
                charaterData.DataSet((int)ClassType.Mage);
			DB.classType = ClassType.Mage;
                break;
            default:
                break;
        }

    }
}
