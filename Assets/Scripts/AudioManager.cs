using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<AudioWithClipsNames> clips;
    public AudioSource source;

    public void OnEnable()
    {
        EventManager.StartListening("WatcherHere", WatcherHere);
    }

    public void OnDisable()
    {
        EventManager.StopListening("WatcherHere", WatcherHere);
    }

    public void WatcherHere(object arg0)
    {
        source.PlayOneShot(clips.Find(x => x.Audioname == "WatcherHere").clip);
    }

}

[System.Serializable]
public class AudioWithClipsNames
{
    public string Audioname;
    public AudioClip clip;
}
