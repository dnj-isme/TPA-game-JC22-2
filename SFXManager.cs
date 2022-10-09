using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SFXManager : MonoBehaviour
{
    [Header("Player Movement & Interaction")]
    [SerializeField] private AudioClip walkAudio;
    [SerializeField] private AudioClip equipAudio;

    [Header("Attack")]
    [SerializeField] private List<AudioClip> attackVoice;
    [SerializeField] private List<AudioClip> punchHit;
    [SerializeField] private List<AudioClip> swordWoosh;
    [SerializeField] private List<AudioClip> swordHit;

    [Header("Mr. Bear")]
    [SerializeField] private AudioClip bearAttack;
    [SerializeField] private AudioClip bearDeath;

    [Header("Combat Misc.")]
    [SerializeField] private List<AudioClip> levelUp;
    [SerializeField] private List<AudioClip> receiveHit;
    [SerializeField] private AudioClip deathSound;
    public bool Walking { get; set; }

    private AudioSource walkSFX;

    private void Awake()
    {
        walkSFX = new GameObject().AddComponent<AudioSource>();
        walkSFX.clip = walkAudio;
        walkSFX.loop = true;
    }

    private void Update()
    {
        if (Walking && !walkSFX.isPlaying) walkSFX.Play();
        if (!Walking && walkSFX.isPlaying) walkSFX.Stop();
    }

    public void PlayEquip() => PlayClip(equipAudio, .5f, 2);
    public void PlayDeath() => PlayClip(deathSound, .5f, 2);
    public void PlayAttack() => PlayRandomClip(attackVoice, .5f, 2);
    public void PlayPunchHit() => PlayRandomClip(punchHit, .5f, 2);
    public void PlaySwordWoosh() => PlayRandomClip(swordWoosh, .5f, 2);
    public void PlaySwordHit() => PlayRandomClip(swordHit, .5f, 2);
    public void PlayLevelUp() => PlayAllClip(levelUp, .5f, 2);
    public void PlayReceiveHit() => PlayRandomClip(receiveHit, .5f, 2);
    public void PlayBearAttack() => PlayClip(bearAttack, .5f, 2);
    public void PlayBearDeath() => PlayClip(bearDeath, .5f, 3);

    private void PlayClip(AudioClip clip, float volume, int killTime)
    {
        AudioSource temp = new GameObject().AddComponent<AudioSource>();
        temp.clip = clip;
        temp.volume = volume;
        if (!temp.isPlaying) temp.Play();
        Destroy(temp.gameObject, killTime);
    }

    private void PlayRandomClip(List<AudioClip> clips, float volume, int killTime)
    {
        PlayClip(clips[Random.Range(0, clips.Count)], volume, killTime);
    }
    private void PlayAllClip(List<AudioClip> clips, float volume, int killTime)
    {
        foreach (AudioClip clip in clips)
        {
            PlayClip(clip, volume, killTime);
        }
    }
}
