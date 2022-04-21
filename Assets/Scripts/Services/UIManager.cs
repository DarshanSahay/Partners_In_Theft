using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : GenericSingleton<UIManager>
{
    public Button resumeBtn;
    public Button retryBtn1;
    public Button retryBtn2;
    public Button menuBtn1;
    public Button menuBtn2;

    public GameObject pausePanel;
    public GameObject gameOverPanel;

    private void Start()
    {
        resumeBtn.onClick.AddListener(ClosePausePanel);
        retryBtn1.onClick.AddListener(ReloadScene);
        retryBtn2.onClick.AddListener(ReloadScene);
        menuBtn1.onClick.AddListener(OpenMainMenu);
        menuBtn2.onClick.AddListener(OpenMainMenu);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenPausePanel();
        }
    }

    public void OpenPausePanel()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void OpenGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }

    public void ClosePausePanel()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void ReloadScene()
    {
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        SceneTransition.Instance.StartTransition(SceneManager.GetActiveScene().buildIndex);
    }

    public void OpenMainMenu()
    {
        SceneTransition.Instance.StartTransition(1);
    }
}
