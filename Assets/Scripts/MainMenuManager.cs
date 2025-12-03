using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panel Settings")]
    public GameObject PanelMainMenu;
    public GameObject PanelOption;

    public void OpenOption() 
    {
        PanelMainMenu.SetActive(false);
        PanelOption.SetActive(true);
    }

    public void CloseOption() 
    {
        PanelMainMenu.SetActive(true);
        PanelOption.SetActive(false);
    }

    public void QuitGame()
    {
        PanelMainMenu.SetActive(false);
        PanelOption.SetActive(false);
        Application.Quit();
    }

    public void OpenGamePlay() 
    {
        SceneManager.LoadScene("MazeScene");
    }

    void Start()
    {
        PanelMainMenu.SetActive(true);
        PanelOption.SetActive(false);
    }

    void Update()
    {
        
    }
}
