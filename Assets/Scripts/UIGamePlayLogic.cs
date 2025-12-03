using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGamePlayLogic : MonoBehaviour
{
    [Header("Health Bar Setup")]
    public Image healthBar;
    public Text healthText;

    [Header("Power/Strength Bar Setup")]
    public Image powerBar;
    public Text powerText;

    [Header("Game Result Settings")]
    public GameObject panelGameResult;
    public Text gameResultText;

    public void GameResult(bool isWin)
    {
        panelGameResult.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (isWin) {
            gameResultText.color = Color.green;
            gameResultText.text = "Mission Complete";
        } else {
            gameResultText.color = Color.red;
            gameResultText.text = "Mission Failed";
        }
    }

    public void GameResultDecision(bool tryAgain)
    {
        if (tryAgain) SceneManager.LoadScene("MazeScene");
        else SceneManager.LoadScene("MainMenu");
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        healthBar.fillAmount = currentHealth / maxHealth;
        healthText.text = currentHealth.ToString();
        
        if (currentHealth <= 0) GameResult(false);
    }

    public void UpdatePowerBar(float currentPower, float maxPower)
    {
        powerBar.fillAmount = currentPower / maxPower;
        powerText.text = currentPower.ToString();
    }
    
    void Start()
    {
        panelGameResult.SetActive(false);
    }

    void Update()
    {
        
    }
}
