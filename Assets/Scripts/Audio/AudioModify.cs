using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioModify : MonoBehaviour
{
    AudioManager audioManager;

    void Awake(){
        audioManager = GameManager.Instance.gameObject.GetComponent<AudioManager>();
    }

    public void SetSoundVolume(float _volume){
        audioManager.SetSoundVolume(_volume);
    }

    public void SetMusicVolume(float _volume){
        audioManager.SetMusicVolume(_volume);
    }

    public void Play(string name){
        audioManager.Play(name);
    }
}
