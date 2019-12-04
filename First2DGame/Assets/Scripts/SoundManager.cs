using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    /// <summary>
    /// 音效播放器
    /// </summary>
    private AudioSource _audioSource;

    /// <summary>
    /// 背景音乐播放器
    /// </summary>
    private AudioSource _bgmAudioSource;

    [Header("跳起的音效")]
    public AudioClip JumpAudioClip;

    [Header("受伤的音效")]
    public AudioClip HurtAudioClip;

    [Header("捡起樱桃的音效")]
    public AudioClip CherryAudioClip;

    [Header("播放玩家死亡的音效")]
    public AudioClip DeathAudioClip;
    /// <summary>
    /// 当前声音管理单例
    /// </summary>
    public static SoundManager Instance;

    void Awake()
    {
        Instance = this;
        var sources = GetComponents<AudioSource>();
        _audioSource = sources[0];
        _bgmAudioSource = sources[1];
    }

    /// <summary>
    /// 播放跳起的音效
    /// </summary>
    public void JumpAudio()
    {
        _audioSource.clip = JumpAudioClip;
        _audioSource.Play();
    }

    /// <summary>
    /// 播放捡起樱桃的音效
    /// </summary>
    public void CherryAudio()
    {
        _audioSource.clip = CherryAudioClip;
        _audioSource.Play();
    }

    /// <summary>
    /// 播放受伤的音效
    /// </summary>
    public void HurtAudio()
    {
        _audioSource.clip = HurtAudioClip;
        _audioSource.Play();
    }

    public void DeathAudio()
    {
        _audioSource.clip = DeathAudioClip;
        _audioSource.Play();
    }

    public void StopBgm()
    {
        _bgmAudioSource.Stop();
    }
}
