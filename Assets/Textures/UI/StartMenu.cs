using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject levelSelect;
    public GameObject startButton;

    void Start()
    {
        levelSelect.SetActive(false);
        startButton.GetComponent<Button>().onClick.AddListener(StartOnClick);
    }

    void StartOnClick()
    {
        levelSelect.SetActive(true);
        mainMenu.SetActive(false);
    }
}
