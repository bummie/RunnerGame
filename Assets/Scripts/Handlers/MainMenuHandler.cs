using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{

    [Header("Menus")]
    public GameObject PanelMain;
    public GameObject PanelShop;
    public GameObject PanelSettings;
    public GameObject PanelShopAlert;

    [Header("ShopText")]
    public Text AlertTitle;
    public Text AlertText;

    [Header("Settings_Buttons")]
    public Button ToggleMusic;
    public Button ToggleSFX;

    [Header("ScoreText")]
    public Text ScoreTextCoins;
    public Text ScoreTextTime;

    [Header("Hat")]
    public GameObject HatHandler;

    private const int PANEL_MAIN = 0, PANEL_SHOP = 1, PANEL_SETTINGS = 2;

    // Load and store data
    private IOHandler IO;

    void Start()
    {
        IO = new IOHandler();
        displayPanel(PANEL_MAIN);
        setButtonState(ToggleMusic, IO.getMusic());
        setButtonState(ToggleSFX, IO.getSFX());
        updateScoreText();
    }

    //Knapper
    public void onClickPlay()
    {
        SceneManager.LoadScene("Main");
    }

    public void onClickShop()
    {
        displayPanel(PANEL_SHOP);
    }

    public void onClickSettings()
    {
        displayPanel(PANEL_SETTINGS);
    }

    public void onClickExit()
    {
        displayPanel(PANEL_MAIN);
    }

    public void onClickExitAlart()
    {
        PanelShopAlert.SetActive(false);
    }

    public void onClickToggleMusic()
    {
        IO.setMusic(!IO.getMusic());
        setButtonState(ToggleMusic, IO.getMusic());
    }

    public void onClickToggleSFX()
    {
        IO.setSFX(!IO.getSFX());
        setButtonState(ToggleSFX, IO.getSFX());
    }

    private void displayPanel(int panel)
    {
        updateScoreText();

        // Dirty
        if (panel == PANEL_MAIN)
        {
            PanelMain.SetActive(true);
            PanelShop.SetActive(false);
            PanelSettings.SetActive(false);
            PanelShopAlert.SetActive(false);

        }
        else if (panel == PANEL_SHOP)
        {
            PanelMain.SetActive(false);
            PanelShop.SetActive(true);
            PanelSettings.SetActive(false);
            PanelShopAlert.SetActive(false);
        }
        else if (panel == PANEL_SETTINGS)
        {
            PanelMain.SetActive(false);
            PanelShop.SetActive(false);
            PanelSettings.SetActive(true);
            PanelShopAlert.SetActive(false);
        }
    }

    private void setButtonState(Button b, bool isOn)
    {
        Color On = new Color(0.678f, 1, 0.572f);
        Color Off = new Color(1, 0.572f, 0.572f);
        if (isOn)
        {
            b.GetComponent<Image>().color = On;
            b.GetComponentInChildren<Text>().text = "On";
        }
        else
        {
            b.GetComponent<Image>().color = Off;
            b.GetComponentInChildren<Text>().text = "Off";
        }
    }

    private void updateScoreText()
    {
        ScoreTextCoins.text = IO.getCoins() + "kr";
        ScoreTextTime.text = getTimePretty(IO.getHighscore());
    }

    private void displayAlert(string title, string text)
    {
        if (AlertText != null && AlertTitle != null)
        {
            PanelShopAlert.SetActive(true);
            AlertText.text = text;
            AlertTitle.text = title;
        }
    }

    private string getTimePretty(float timePlayed)
    {
        System.TimeSpan t = System.TimeSpan.FromSeconds(timePlayed);
        return string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);

        //return (timePlayed / 60).ToString("00") + ":" + (timePlayed % 60).ToString("00");
    }

    //
    // Shop
    //

    public void buyHat(string idPrice)
    {
        string[] splittedData = idPrice.Split(':');
        int id = int.Parse(splittedData[0]);
        int price = int.Parse(splittedData[1]);
        Debug.Log("ID: " + id + "Cost: " + price);

        int valid = IO.buyHat(id, price);
        if (valid == 0)
            displayAlert("Success", "Have fun with your new hat!");
        else if(valid == 1)
            displayAlert("Poor", "Come back when you've managed to get more coins!");
        else if (valid == 2)
            displayAlert("Richkid", "You already own hat item silly!");
        updateScoreText();
    }

    public void selectHat(string hatid)
    {
        int id = int.Parse(hatid);
        bool status = IO.setSelectedHat(id);
        if(status)
            displayAlert("Great", "Hat has been equipped!");
        else
            displayAlert("Fool", "You can't wear what you dont own!");

        HatHandler.GetComponent<HatHandler>().loadHat();
    }

    public void buyLevel(string idPrice)
    {
        string[] splittedData = idPrice.Split(':');
        int id = int.Parse(splittedData[0]);
        int price = int.Parse(splittedData[1]);
        Debug.Log("ID: " + id + "Cost: " + price);

        int valid = IO.buyLevel(id, price);
        if (valid == 0)
            displayAlert("Success", "Have fun with your new level!");
        else if (valid == 1)
            displayAlert("Poor", "Come back when you've managed to get more coins!");
        else if (valid == 2)
            displayAlert("Richkid", "You already own this level silly!");
        updateScoreText();
    }

    public void selectLevel(string levelid)
    {
        int id = int.Parse(levelid);
        bool status = IO.setSelectedLevel(id);
        if (status)
            displayAlert("Cool", "Level has changed!");
        else
            displayAlert("Fool", "You can't play a level you dont own!");
    }

    public void buttonCredits()
    {
        displayAlert("Credits", "Koding: Erlend og Seb \n Playermodel: https://www.assetstore.unity3d.com/en/#!/content/51662 Animations: https://www.assetstore.unity3d.com/en/#!/content/65284 \n Fire: https://www.assetstore.unity3d.com/en/#!/content/21587 \n Andre modeller og ikoner av Seb \n Lyder fra FreeSound.org og dl-sounds.com/royalty-free");
    }

}