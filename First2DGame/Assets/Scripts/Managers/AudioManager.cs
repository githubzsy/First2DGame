using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// 音效播放器
    /// </summary>
    private AudioSource _fxSource;

    /// <summary>
    /// 玩家动作声音播放器
    /// </summary>
    private AudioSource _playerActionSource;

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

    [Header("玩家死亡的音效")]
    public AudioClip DeathAudioClip;
    /// <summary>
    /// 当前声音管理单例
    /// </summary>
    private static AudioManager _instance;

    [Tooltip("最大Hp增长时的音效")]
    public AudioClip MaxHpAudio;

    [Tooltip("敌人死亡时的音效")]
    public AudioClip EnemyDeathAudioClip;

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        var sources = GetComponents<AudioSource>();
        _fxSource = sources[0];
        _bgmAudioSource = sources[1];
        _playerActionSource = sources[2];

        _instance = this;
    }

    /// <summary>
    /// 播放跳起的音效
    /// </summary>
    public static void JumpAudio()
    {
        _instance._playerActionSource.clip = _instance.JumpAudioClip;
        _instance._playerActionSource.Play();
    }

    /// <summary>
    /// 播放捡起樱桃的音效
    /// </summary>
    public static void CherryAudio()
    {
        _instance._fxSource.clip = _instance.CherryAudioClip;
        _instance._fxSource.Play();
    }

    /// <summary>
    /// 播放受伤的音效
    /// </summary>
    public static void HurtAudio()
    {
        _instance._playerActionSource.clip = _instance.HurtAudioClip;
        _instance._playerActionSource.Play();
    }

    public static void DeathAudio()
    {
        _instance._playerActionSource.clip = _instance.DeathAudioClip;
        _instance._playerActionSource.Play();
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
        _instance._fxSource.clip = _instance.SkillAudioClip;
        _instance._fxSource.Play();
    }

    /// <summary>
    /// 最大Hp增长时的音效
    /// </summary>
    internal static void MaxHpIncreaseAudio()
    {
        _instance._fxSource.clip = _instance.MaxHpAudio;
        _instance._fxSource.Play();
    }

    /// <summary>
    /// 敌人死亡时的音效
    /// </summary>
    internal static void EnemyDeadAudio()
    {
        _instance._fxSource.clip = _instance.EnemyDeathAudioClip;
        _instance._fxSource.Play();
    }
}
