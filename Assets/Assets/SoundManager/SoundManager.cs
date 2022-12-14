using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static GameObject PlaySound(string nameString, Vector3 position, bool isLooped = false)
    {
        SoundSO soundSO = GetSoundSO(nameString);
        if (!soundSO)
        {
            Debug.Log("Cannot find sound named: " + nameString);
            return null;
        }
        GameObject soundPlayerGameObject = SetUpSoundPlayerGameObject(nameString, position, soundSO, isLooped);
        ParentSoundGameObject(soundPlayerGameObject);
        
        if(!isLooped) Destroy(soundPlayerGameObject, soundSO.clip.length);

        return soundPlayerGameObject;
    }
    private static SoundSO GetSoundSO(string nameString)
    {
        SoundSO soundSO = Resources.Load<SoundSO>("Sounds/" + nameString);
        return soundSO;
    }
    private static GameObject SetUpSoundPlayerGameObject(string nameString, Vector3 position, SoundSO soundSO, bool isLooped = false)
    {
        GameObject soundPlayerGameObject = new GameObject("Sound/" + nameString, typeof(AudioSource));
        soundPlayerGameObject.transform.position = position;
        SetUpAudioSource(soundSO, soundPlayerGameObject, isLooped);

        return soundPlayerGameObject;
    }
    private static void SetUpAudioSource(SoundSO soundSO, GameObject soundPlayerGameObject, bool isLooped = false)
    {
        AudioSource soundPlayerAudioSource = soundPlayerGameObject.GetComponent<AudioSource>();
        soundPlayerAudioSource.clip = soundSO.clip;
        soundPlayerAudioSource.volume = soundSO.volume;
        soundPlayerAudioSource.pitch = soundSO.pitch;
        soundPlayerAudioSource.loop = isLooped;
        soundPlayerAudioSource.Play();
    }
    private static void ParentSoundGameObject(GameObject soundPlayerGameObject)
    {
        Transform soundParentTransform;
        
        try
        {
            soundParentTransform = GameObject.Find("SoundParent").transform;
        }
        catch (NullReferenceException e)
        {
            soundParentTransform = new GameObject("SoundParent").transform;        
        }
        soundPlayerGameObject.transform.SetParent(soundParentTransform);
    }
    public static void StopSound(GameObject soundPlayerGameObject)
    {
        AudioSource soundPlayerAudioSource = soundPlayerGameObject.GetComponent<AudioSource>();
        soundPlayerAudioSource.Stop();
        Destroy(soundPlayerGameObject);
    }
}
