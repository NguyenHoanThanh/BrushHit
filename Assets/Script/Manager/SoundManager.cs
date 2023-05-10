using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }

    private AudioSource shortAudio;
    private AudioSource longAudio;

    [SerializeField] private AudioClip triggerTextClip;
    [SerializeField] private AudioClip scaleUpClip;
    [SerializeField] private AudioClip earnStarClip;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip moveUpClip;
    [SerializeField] private AudioClip moveDownClip;
    [SerializeField] private AudioClip loseClip;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        shortAudio = GetComponent<AudioSource>();
        longAudio = transform.GetChild(0).GetComponent<AudioSource>();
    }
    public void ScaleUpSound()
    {
        shortAudio.PlayOneShot(scaleUpClip, 1);
    }
    public void PerfectTextSound()
    {
        shortAudio.PlayOneShot(triggerTextClip, 1);
    }
    public void EarnStarSound()
    {
        shortAudio.PlayOneShot(earnStarClip, 1);
    }
    public void SoundWhenClick()
    {
        shortAudio.PlayOneShot(clickSound, 1);
    }
    public void SoundWhenLose()
    {
        shortAudio.PlayOneShot(loseClip, 1);
    }
    public void SoundWhenMoveUp()
    {
        shortAudio.PlayOneShot(moveUpClip, 1);
    }
    public void SoundWhenMoveDown()
    {
        shortAudio.PlayOneShot(moveDownClip, 1);
    }
    public void StopSoundWhenLose()
    {
        longAudio.Stop();
    }

}
