using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] sounds;
    public Sound[] music;

    int lastMusicIndex = -1;

    //public Button musicButton, soundButton;

    void Awake(){
        Instance = this;

        foreach(Sound s in sounds){
            s.source = gameObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }

        foreach(Sound s in music){
            s.source = gameObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = true;
        }
    }

    /*void Start(){
        //music[1].source.Play();

        if(!GameManager.Instance.SoundActivated){
            soundButton.onClick.Invoke();
        }

        if(!GameManager.Instance.MusicActivated){
            musicButton.onClick.Invoke();
        }
    }*/

    public void Play(string name){
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(!s.source.isPlaying) s.source.Play();
    }

    public void SetSoundVolume(float _volume){
        foreach(Sound s in sounds){
            s.source.volume = _volume;
        }

        if(_volume == 0) GameManager.Instance.SoundActivated = false;
        else GameManager.Instance.SoundActivated = true;
    }

    public void SetMusicVolume(float _volume){
        foreach(Sound s in music){
            s.source.volume = _volume;
        }

        if(_volume == 0) GameManager.Instance.MusicActivated = false;
        else GameManager.Instance.MusicActivated = true;
    }

    public void PlayMusic(int index){
        if(lastMusicIndex != index){
            lastMusicIndex = index;
            foreach(Sound s in music){
                s.source.Stop();
            }

            music[index].source.Play();
        }
    }
}
