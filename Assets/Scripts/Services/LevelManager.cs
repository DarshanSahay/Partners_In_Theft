using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : GenericSingleton<LevelManager>
{
    public int currentLevel = 1;
    public Button[] levelButtons;
    public GameObject buttonParent;

    void Start()
    {
        //PlayerPrefs.DeleteAll();
        DontDestroyOnLoad(gameObject);
        levelButtons = buttonParent.GetComponentsInChildren<Button>();
        currentLevel = PlayerPrefs.GetInt("CurrentLevel",1);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].interactable = false;
        }
        for (int i = 0; i < currentLevel; i++)
        {
            levelButtons[i].interactable = true;
        }
    }

    public void OnLevelCompletion()
    {
        currentLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

        if(currentLevel >= PlayerPrefs.GetInt("CurrentLevel"))
        {
            PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        }
    }
}
