using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button PlayButton;

    public Button QuitButton;

    private PlayerAttribute _playerAttribute;
    void Start()
    {
        PlayButton.onClick.AddListener(Play);
        QuitButton.onClick.AddListener(Quit);
        _playerAttribute = SaveManager.ReadFormFile<PlayerAttribute>(PlayerManager.PlayerAttributeJson, true);
    }

    private void Quit()
    {
        Application.Quit();
    }

    private void Play()
    {
        if (_playerAttribute?.SaveSceneIndex != 0)
        {
            SceneManager.LoadScene(_playerAttribute.SaveSceneIndex);
            return;
        }
        SceneManager.LoadScene(1);
    }
}
