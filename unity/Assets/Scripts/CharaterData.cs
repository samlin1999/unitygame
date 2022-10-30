using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharaterData : MonoBehaviour {
    public Image[] bars;
    public ClassType classType;
    public CharAttr[] charAttr;

	public void DataSet (int index) {
        bars[0].fillAmount = charAttr[index].CON / 20f;
        bars[1].fillAmount = charAttr[index].STR / 20f;
        bars[2].fillAmount = charAttr[index].INT / 20f;
        bars[3].fillAmount = charAttr[index].DEX / 20f;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    
}
