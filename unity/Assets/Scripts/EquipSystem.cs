using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipSystem : MonoBehaviour {
    public static EquipSystem ctrl;
    public EquipInfo helmet;
    public EquipInfo body;
    public EquipInfo gloves;
    public EquipInfo mainweapon;
    public EquipInfo secweapon;
    public EquipInfo leg;
    public EquipInfo boots;

    private delegate bool Checker(Item item);//委派的方法格式定義
    private Checker checker;//正式宣告
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
    public bool CheckEquiped(Item item)
    {
        switch (item.itemType)
        {
            case ItemType.Weapon:
                checker = weaponCheck((Weapon)item);
                break;
            case ItemType.Armor:
                checker = armorCheck((Armor)item);
                break;
        }

        return checker(item);//回傳比對結果
    }
    Checker weaponCheck(Weapon item)
    {
        switch(item.handType)
        {
            case HandType.Mainhand:
                return mainweapon.Checker;
            case HandType.Offhand:
                return secweapon.Checker;
            case HandType.Bothhand:
                return mainweapon.Checker;
            default:
                return null;
        }
    }
    Checker armorCheck(Armor item)
    {
        switch (item.partType)
        {
            case PartType.Helmet:
                return helmet.Checker;
            case PartType.Body:
                return body.Checker;
            case PartType.Gloves:
                return gloves.Checker;
            case PartType.Leg:
                return leg.Checker;
            case PartType.Boots:
                return boots.Checker;
            default:
                return null;
        }

    }
    
}
