using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("暂停界面")]
    public GameObject PauseMenu;

    [Header("主Mixer")]
    public AudioMixer AudioMixer;

    /// <summary>
    /// 暂停按钮
    /// </summary>
    [Tooltip("暂停按钮")]
    public Button PauseButton;

    /// <summary>
    /// 恢复按钮
    /// </summary>
    [Tooltip("恢复按钮")]
    public Button ResumeButton;


    /// <summary>
    /// 保存退出按钮
    /// </summary>
    [Tooltip("保存退出按钮")]
    public Button SaveExitButton;

    /// <summary>
    /// 对话框
    /// </summary>
    [Tooltip("对话框")]
    public Dialog Dialog;

    /// <summary>
    /// 樱桃数量文本
    /// </summary>
    [Tooltip("樱桃数量文本")]
    public Text CherryText;

    /// <summary>
    /// 玩家血条
    /// </summary>
    [Tooltip("玩家血条")]
    public GameObject HpLine;

    /// <summary>
    /// 表示血的星星
    /// </summary>
    [Tooltip("表示血的星星")]
    public GameObject HpStarPrefab;

    private static UIManager _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
    }

    private void Start()
    {
        EventManager.StartListening("PauseGame", PauseGame);
        PauseButton.onClick.AddListener(()=> EventManager.TriggerEvent("PauseGame"));

        EventManager.StartListening("ResumeGame", ResumeGame);
        ResumeButton.onClick.AddListener(() => EventManager.TriggerEvent("ResumeGame"));

        EventManager.StartListening("SaveGame",SaveManager.SaveGame);
        SaveExitButton.onClick.AddListener(()=>EventManager.TriggerEvent("SaveGame"));
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    public static void Play()
    {
        LevelManager.LoadNext();
    }

    /// <summary>
    /// 结束游戏
    /// </summary>
    public static void Quit()
    {
        Application.Quit();
    }

    /// <summary>
    /// 显示UI界面
    /// </summary>
    public static void ShowUi()
    {
        GameObject.Find("Canvas/MainMenu/UI").SetActive(true);
    }

    /// <summary>
    /// 暂停游戏
    /// </summary>
    public static void PauseGame()
    {
        _instance.PauseMenu.SetActive(true);
        //暂停时间
        Time.timeScale = 0f;
    }

    /// <summary>
    /// 恢复游戏
    /// </summary>
    public static void ResumeGame()
    {
        _instance.PauseMenu.SetActive(false);
        //暂停时间
        Time.timeScale = 1f;
    }

    public static void SaveAndExitGame()
    {
        PlayerManager.SavePlayer();
        CollectionManager.SaveCollections();
        Application.Quit();
    }

    /// <summary>
    /// 设置主音量
    /// </summary>
    /// <param name="value">音量值</param>

    public static void SetVolume(float value)
    {
        _instance.AudioMixer.SetFloat("MainVolume", value);
    }

    /// <summary>
    /// 将对话框设置为显示
    /// </summary>
    internal static void SetDialogActive()
    {
        _instance.Dialog.gameObject.SetActive(true);
    }

    /// <summary>
    /// 刷新樱桃数量
    /// </summary>
    /// <param name="cherryCount"></param>
    internal static void RefreshCherryCount(int cherryCount)
    {
        _instance.CherryText.text = cherryCount.ToString();
    }

    /// <summary>
    /// 刷新血量
    /// </summary>
    /// <param name="hp"></param>
    internal static void RefreshHp(int hp)
    {
        int count = _instance.HpLine.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            Destroy(_instance.HpLine.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < hp; i++)
        {
            Instantiate(_instance.HpStarPrefab, _instance.HpLine.transform);
        }
    }
}
