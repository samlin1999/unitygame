using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// 自動建置音效
    /// </summary>
    public static AudioSource audioSource
    {
        get 
        { 
            if(!sound) sound = ctrl.gameObject.AddComponent<AudioSource>();
            return sound;
        }
    }
    /// <summary>
    /// 自動建置音樂
    /// </summary>
    public static AudioSource musicSource
    {
        get 
        {
            if (!music) music = ctrl.gameObject.AddComponent<AudioSource>();
            return music; 
        }
    }
    /// <summary>
    /// 自動建置音樂控制器
    /// </summary>
    public static AudioManager ctrl
    {
        get
        {
            if(!audioManager){
                audioManager = new GameObject("AudioManager").AddComponent<AudioManager>();
            }
            return audioManager;
        }
    }
    private static AudioSource sound, music;
    private static AudioManager audioManager;
    private float startTime;
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="audioClip"></param>
    /// <param name="loop"></param>
    /// <param name="volume"></param>
    /// <param name="pitch"></param>
    public void PlaySFX(AudioClip audioClip,bool loop = false,float volume = 1,float pitch = 1){
        if (IsPlaying(audioClip) && loop) return;//循環音效不重複觸發
        startTime = Time.time;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.PlayOneShot(audioClip);//音效可疊加

    }
    /// <summary>
    /// 停止音效
    /// </summary>
    public void StopSFX(){
        audioSource.Stop();
    }

    /// <summary>
    /// 用來檢測當前音效播放完畢
    /// </summary>
    /// <returns><c>true</c>, if playing was ised, <c>false</c> otherwise.</returns>
    /// <param name="clip">音效檔</param>
    bool IsPlaying(AudioClip clip)
    {
        return (Time.time - startTime) <= clip.length;
    }
    /// <summary>
    /// 播放音樂
    /// </summary>
    /// <param name="audioClip"></param>
    /// <param name="loop"></param>
    /// <param name="volume"></param>
    /// <param name="pitch"></param>
    public void PlayMusic(AudioClip audioClip, bool loop = true, float volume = 1, float pitch = 1)
    {
        musicSource.clip = audioClip;
        musicSource.loop = loop;
        musicSource.volume = volume;
        musicSource.pitch = pitch;
        musicSource.Play();
    }
    /// <summary>
    /// 停止音樂
    /// </summary>
    public void StopMusic()
    {
        musicSource.Stop();
    }

}
