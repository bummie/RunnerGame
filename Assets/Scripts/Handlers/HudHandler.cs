using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudHandler : MonoBehaviour
{
    // HUD
    [Header("HUD Text Elements")]
    public Text tCoins;
    public Text tTime;
    public GameObject[] iHealth;

    [Header("GameOver")]
    public GameObject PanelGameOver;
    public Text tGameOverTime;
    public Text tGameOverCoins;

    [Header("Pause")]
    public GameObject PanelPause;
    public Text tPausedTime;
    public Text tPausedCoins;

    // Show Game over menu
    public void showGameOverMenu(bool shouldShow)
    {
        if (PanelGameOver != null)
            PanelGameOver.SetActive(shouldShow);
    }

    // Show pause menu
    public void showPauseMenu(bool shouldShow)
    {
        if (PanelPause != null)
            PanelPause.SetActive(shouldShow);
    }

    // Set HUD Coins text
    public void setTextCoins(string text)
    {
        if (tCoins != null)
            tCoins.text = text;
    }

    // Set HUD Time text
    public void setTextTime(string text)
    {
        if (tTime != null)
            tTime.text = text;
    }

    // Set HUD Coins text
    public void setTextCoinsGamOver(string text)
    {
        if (tGameOverCoins != null)
            tGameOverCoins.text = text;
    }

    // Set HUD Time text
    public void setTextTimeGamOver(string text)
    {
        if (tGameOverTime != null)
            tGameOverTime.text = text;
    }

    // Set HUD Time text
    public void setTextPausedTime(string text)
    {
        if (tPausedTime != null)
            tPausedTime.text = text;
    }

    // Set HUD Coins text
    public void setTextPausedCoins(string text)
    {
        if (tPausedCoins != null)
            tPausedCoins.text = text;
    }

    public void setHealth(int health)
    {
        if (iHealth != null)
        {
            // Dirty løsning
            switch (health)
            {
                case 0:
                    iHealth[0].SetActive(false);
                    iHealth[1].SetActive(false);
                    iHealth[2].SetActive(false);
                    break;
                case 1:
                    iHealth[0].SetActive(false);
                    iHealth[1].SetActive(false);
                    iHealth[2].SetActive(true);
                    break;
                case 2:
                    iHealth[0].SetActive(false);
                    iHealth[1].SetActive(true);
                    iHealth[2].SetActive(true);
                    break;
                case 3:
                    iHealth[0].SetActive(true);
                    iHealth[1].SetActive(true);
                    iHealth[2].SetActive(true);
                    break;
            }
        }
    }
}
