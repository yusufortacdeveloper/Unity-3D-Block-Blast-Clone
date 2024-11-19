using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour, IEventListener
{
    public enum SoundID
    {
        Game,
        Point,
        Drag,
        Drop,
        Combo,
        ReturnPos
    }

    [System.Serializable]
    public class SoundList
    {
        public SoundID id;
        public AudioSource audio;
    }

    public List<SoundList> soundList = new List<SoundList>();

    private void OnEnable()
    {
        EventManager.Instance.RegisterListener(this);
    }

    private void OnDisable()
    {
        EventManager.Instance.UnregisterListener(this);
    }

    public void PlaySound(SoundID id)
    {
        foreach (var sound in soundList)
        {
            if (sound.id == id)
            {
                sound.audio.Play();
                break;
            }
        }
    }
}
