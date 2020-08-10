using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject levelSelect;
    public Button backButton;

    private Button[] levels;

    void Start()
    {
        backButton.onClick.AddListener(ReturnOnClick);

        levels = levelSelect.transform.Find("Levels").GetComponentsInChildren<Button>();
        
        foreach (Button level in levels)
            level.onClick.AddListener(() => { Transition(level.GetComponentInChildren<Text>().text); });
    }

    void Transition(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    void ReturnOnClick()
    {
        mainMenu.SetActive(true);
        levelSelect.SetActive(false);
    }
}
