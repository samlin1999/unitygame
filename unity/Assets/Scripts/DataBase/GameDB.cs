using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "GameDB",menuName = "DB / GameDB",order = 1)]
public class GameDB : ScriptableObject {
	public ClassType classType;
	public List<Transform> models;
    public CharAttr[] charAttrs;
    public AudioClip audioClip;
}
