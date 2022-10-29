using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBagPanelCtrl : MonoBehaviour {
    public static UIBagPanelCtrl ctrl;
    /// <summary>
    /// 自動抓取物件上的內容
    /// </summary>
    public RectTransform RT
    {
        get { return GetComponent<RectTransform>(); }
    }
    public GameObject slotTMP;
    private void Awake()
    {
        ctrl = this;
    }
    public int colCount;
    public int rowCount = 4;
    public float cellSize = 187.5f;
    public Sprite DefaultItem;
    List<Image> imgList = new List<Image>();
    List<Text> textList = new List<Text>();

	// Use this for initialization
	public void Initial (int bagCount)
    {
        colCount = bagCount / rowCount;
        RT.sizeDelta = Vector2.up * cellSize * colCount;

        for(int i = 0;i < bagCount;i++)
        {
            GameObject tmp = Instantiate(slotTMP, transform);
            tmp.SetActive(true);
            tmp.name = "Slot (" + (i + 1).ToString() + ")";
            imgList.Add(tmp.GetComponent<Image>());//UI格子欄位的影像控制=>加入清單
            imgList[i].sprite = DefaultItem;//用序列號控制UI格子欄位的影像
            textList.Add(tmp.GetComponentInChildren<Text>());
            textList[i].text = "";
        }
        UpdateUI(GameManager.ctrl.bagSystem.bagSlots);
	}
	
	public void UpdateUI (SlotData[] bagSlots) {
		for(int i = 0;i < bagSlots.Length;i++)
        {
            if(bagSlots[i].count != 0)
            {
                //print(bagSlots[i].itemName);
                imgList[i].sprite = bagSlots[i].icon;
                textList[i].text = bagSlots[i].count.ToString();
            }
            
        }
	}
}
