using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SkillTreeSystem : ISystem {
    public static SkillTreeSystem ctrl;
    public SkillGroup skillGroup;
    public CharSkillDB skillDB;
    private void Awake()
    {
        ctrl = this;
    }
    
    void Start () {
		
	}
	
	
	void Update () {
		
	}

    public override void Inital()
    {
        switch (GameManager.ctrl.characterInfo.classType)
        {
            case ClassType.Warrior:
                skillDB = AssetDatabase.LoadAssetAtPath<CharSkillDB>("Assets/Scripts/DataBase/WarriorSkillDB.asset");//Resources.Load<CharSkillDB>("WarriorSkillDB.asset");
                break;
            case ClassType.Archer:
                skillDB = AssetDatabase.LoadAssetAtPath<CharSkillDB>("Assets/Scripts/DataBase/ArcherSkillDB.asset");//Resources.Load<CharSkillDB>("ArcherSkillDB.asset");// 
                break;
            case ClassType.Mage:
                skillDB = AssetDatabase.LoadAssetAtPath<CharSkillDB>("Assets/Scripts/DataBase/MageSkillDB.asset");//Resources.Load<CharSkillDB>("MageSkillDB.asset");// 
                break;
        }

        foreach(SkillGroupinfo SGI in skillDB.skillGroupinfos)
        {
            Instantiate(skillGroup, transform).setData(SGI);
        }

    }

}
