using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public static void PlayMusic(string _nameString)
    {
        MusicSO musicSO = GetMusicSO(_nameString);
        if (!musicSO)
        {
            Debug.Log("Can't find musicSO with name: " + _nameString);
            return;
        }
        AudioSource musicPlayerAudioSource = GetMusicPlayerAudioSource();
        SetUpAudioSource(musicPlayerAudioSource, musicSO);
    }
    private static void SetUpAudioSource(AudioSource _audioSource ,MusicSO _musicSO)
    {
        _audioSource.clip = _musicSO.clip;
        _audioSource.volume = _musicSO.volume;
        _audioSource.pitch = _musicSO.pitch;
        _audioSource.loop = _musicSO.loop;
        _audioSource.Play();
    } 
    private static MusicSO GetMusicSO(string _nameString)
    {
        MusicSO musicSO = Resources.Load<MusicSO>("Musics/" + _nameString);
        return musicSO;
    }
    private static AudioSource GetMusicPlayerAudioSource()
    {
        GameObject musicPlayer = GameObject.Find("MusicPlayer");
        if (!musicPlayer)
        {
            musicPlayer = new GameObject("MusicPlayer", typeof(AudioSource));
            DontDestroyOnLoad(musicPlayer);
        } 

        return musicPlayer.GetComponent<AudioSource>();
    }
    public static void StopMusic()
    {
        GameObject musicPlayer = GameObject.Find("MusicPlayer");
        if (musicPlayer) musicPlayer.GetComponent<AudioSource>().Stop();
    }
}
