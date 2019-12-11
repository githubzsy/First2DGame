using System;
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

    [Header("获取技能的音效")]
    public AudioClip SkillAudioClip;

    [Header("播放玩家死亡的音效")]
    public AudioClip DeathAudioClip;
    /// <summary>
    /// 当前声音管理单例
    /// </summary>
    private static SoundManager _instance;

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        var sources = GetComponents<AudioSource>();
        _audioSource = sources[0];
        _bgmAudioSource = sources[1];
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    /// <summary>
    /// 播放跳起的音效
    /// </summary>
    public static void JumpAudio()
    {
        _instance._audioSource.clip = _instance.JumpAudioClip;
        _instance._audioSource.Play();
    }

    /// <summary>
    /// 播放捡起樱桃的音效
    /// </summary>
    public static void CherryAudio()
    {
        _instance._audioSource.clip = _instance.CherryAudioClip;
        _instance._audioSource.Play();
    }

    /// <summary>
    /// 播放受伤的音效
    /// </summary>
    public static void HurtAudio()
    {
        _instance._audioSource.clip = _instance.HurtAudioClip;
        _instance._audioSource.Play();
    }

    public static void DeathAudio()
    {
        _instance._audioSource.clip = _instance.DeathAudioClip;
        _instance._audioSource.Play();
    }

    public static void PlayBgm()
    {
        _instance._bgmAudioSource.Play();
    }

    public static void StopBgm()
    {
        _instance._bgmAudioSource.Stop();
    }

    internal static bool IsPlayingBgm()
    {
        return _instance._bgmAudioSource.isPlaying;
    }

    internal static void SkillAudio()
    {
        _instance._audioSource.clip = _instance.SkillAudioClip;
        _instance._audioSource.Play();
    }
}
