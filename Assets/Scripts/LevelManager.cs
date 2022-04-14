using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public int currentLevel = 1;
    public Button[] levelButtons;
    public GameObject buttonParent;

    void Start()
    {
        levelButtons = buttonParent.GetComponentsInChildren<Button>();
        OnLevelCompletion();
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

    void OnLevelCompletion()
    {
        currentLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

        if(currentLevel >= PlayerPrefs.GetInt("CurrentLevel"))
        {
            PlayerPrefs.SetInt("CurrentLevel", currentLevel + 1);
        }
    }
}
