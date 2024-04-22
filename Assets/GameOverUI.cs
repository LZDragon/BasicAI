using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button restartButton;

    [SerializeField] private Button quitButton;
    // Start is called before the first frame update
    void Start()
    {
        restartButton.onClick.AddListener(OnRestartButton);
        quitButton.onClick.AddListener(OnQuitButton);
    }


    void OnRestartButton()
    {
        SceneManager.LoadScene(1);
    }

    void OnQuitButton()
    {
        Application.Quit();
    }
}
