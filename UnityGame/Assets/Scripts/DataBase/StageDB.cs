using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageDB",menuName = "DB/StageDB",order = 0)]
public class StageDB : ScriptableObject {

    public List<SceneInfo> sceneInfos = new List<SceneInfo>();


}
