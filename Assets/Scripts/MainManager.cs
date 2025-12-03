using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public GameObject PanelMainMenu;
    public GameObject PanelSettings;

    // Start is called before the first frame update
    void Start()
    {
        PanelMainMenu.SetActive(true);
        PanelSettings.SetActive(false);
    }

    public void OpenSettings()
    {
        PanelSettings.SetActive(true);
        PanelMainMenu.SetActive(false);
    }

    public void CloseSettings()
    {
        PanelSettings.SetActive(false);
        PanelMainMenu.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
