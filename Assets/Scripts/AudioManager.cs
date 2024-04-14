using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSourceInitialLoop;
    [SerializeField] private AudioSource audioSourceAttackLoop;
    [SerializeField] private AudioSource audioSourceOneShot;

    public void SwitchToAttackMusic()
    {
        double introTime = AudioSettings.dspTime + audioSourceOneShot.clip.length;
        audioSourceInitialLoop.Stop();
        audioSourceOneShot.PlayOneShot(audioSourceOneShot.clip);
        audioSourceOneShot.SetScheduledEndTime(introTime);
        audioSourceAttackLoop.PlayScheduled(introTime);
    }

    public void SwitchToInitialMusic()
    {
        audioSourceAttackLoop.Stop();
        audioSourceOneShot.Stop();
        audioSourceInitialLoop.Play();
    }
}
