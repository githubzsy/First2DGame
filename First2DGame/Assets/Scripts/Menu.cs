using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [Header("暂停界面")]
    public GameObject PauseMenu;

    [Header("主Mixer")]
    public AudioMixer AudioMixer;
    /// <summary>
    /// 开始游戏
    /// </summary>
    public void Play()
    {
        SceneManager.LoadScene("Start");
    }

    /// <summary>
    /// 结束游戏
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }

    /// <summary>
    /// 显示UI界面
    /// </summary>
    public void ShowUi()
    {
        GameObject.Find("Canvas/MainMenu/UI").SetActive(true);
    }

    /// <summary>
    /// 暂停游戏
    /// </summary>
    public void PauseGame()
    {
        PauseMenu.SetActive(true);
        //暂停时间
        Time.timeScale = 0f;
    }

    /// <summary>
    /// 恢复游戏
    /// </summary>
    public void ResumeGame()
    {
        PauseMenu.SetActive(false);
        //暂停时间
        Time.timeScale = 1f;
    }

    /// <summary>
    /// 设置主音量
    /// </summary>
    /// <param name="value">音量值</param>

    public void SetVolume(float value)
    {
        AudioMixer.SetFloat("MainVolume", value);
    }
}
