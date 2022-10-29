using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct SoundData
{
    public string soundName;
    public AudioClip audioClip;
}
[CreateAssetMenu(fileName = "SoundDB",menuName = "DB/SoundDB",order = 4)]
public class SoundDB : ScriptableObject {
    public List<SoundData> sounds;

    public AudioClip SearchBGM(string name)
    {
        AudioClip audioTMP = null;
        foreach(SoundData SD in sounds)
        {
            if(SD.soundName == name)
            {
                audioTMP =  SD.audioClip;
                break;
            }
        }
        return audioTMP;
    }
}
