using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IOHandler : MonoBehaviour {

    private bool _debug = true;

    // Settings
    public const string SETTINGS_SOUND_SFX = "settings_sound_sfx",
                        SETTINGS_SOUND_MUSIC = "settings_sound_music";

    // Playerdata
    public const string PLAYERDATA_COINS = "playerdata_coins",
                        PLAYERDATA_HIGHSCORE = "playerdata_highscore";

    //
    // Shop item IDS
    //

    // Hatter
    public const string SHOP_HATS = "shop_hats", SELECTED_HAT = "shop_selected_hat";
    public static int HAT_UNLOCK_NONE = 0, HAT_UNLOCK_TOPHAT = 1, HAT_UNLOCK_CAPS = 2, HAT_UNLOCK_AFRO = 3, HAT_UNLOCK_SOMBRERO = 4, HAT_UNLOCK_FEZ = 5, HAT_UNLOCK_SNOWHEAD = 6;

    // Baner
    public const string SHOP_LEVELS = "shop_levels", SELECTED_LEVEL = "shop_selected_level";
    public static int LEVEL_UNLOCK_FORREST = 0, LEVEL_UNLOCK_DESERT = 1, LEVEL_UNLOCK_SNOW = 2;

    private char _seperator = ':';

    public IOHandler()
    {

        // Init data if never stored before
       if (!PlayerPrefs.HasKey(SELECTED_HAT))
       //if(true)
        {
            // First init
            setSFX(true);
            setMusic(true);

            setCoins(0);
            setHighscore(0);

            // First init shop
            setHatsUnlocked("1:0:0:0:0:0:0");
            setLevelsUnlocked("1:0:0");
            setSelectedLevel(0);
            setSelectedHat(0);
        }
        //setCoins(5050);

    }


    // Get/Set Sound effects
    public void setSFX(bool isOn)
    {
        int i = 0;
        if (isOn)
            i = 1;

        PlayerPrefs.SetInt(SETTINGS_SOUND_SFX, i);
        PlayerPrefs.Save();
    }

    public bool getSFX()
    {
        if (PlayerPrefs.HasKey(SETTINGS_SOUND_SFX))
        {
            bool ison = false;
            if (PlayerPrefs.GetInt(SETTINGS_SOUND_SFX) == 1)
                ison = true;
            return ison;
        }
        else
            return false;
    }


    // Get/Set Music
    public void setMusic(bool isOn)
    {
        int i = 0;
        if (isOn)
            i = 1;

        PlayerPrefs.SetInt(SETTINGS_SOUND_MUSIC, i);
        PlayerPrefs.Save();
    }

    public bool getMusic()
    {
        if (PlayerPrefs.HasKey(SETTINGS_SOUND_MUSIC))
        {
            bool ison = false;
            if (PlayerPrefs.GetInt(SETTINGS_SOUND_MUSIC) == 1)
                ison = true;
            return ison;
        }
        else
            return false;
    }


    // Get/Set Coins
    public void setCoins(int coins)
    {
        PlayerPrefs.SetInt(PLAYERDATA_COINS, coins);
        PlayerPrefs.Save();
    }

    public int getCoins()
    {
        int coins = 0;
        if (PlayerPrefs.HasKey(PLAYERDATA_COINS))
        {
            coins = PlayerPrefs.GetInt(PLAYERDATA_COINS);
        }
        return coins;
    }

    // Get/Set Highscore
    public void setHighscore(int time)
    {
        PlayerPrefs.SetInt(PLAYERDATA_HIGHSCORE, time);
        PlayerPrefs.Save();
    }

    public int getHighscore()
    {
        int time = 0;
        if (PlayerPrefs.HasKey(PLAYERDATA_HIGHSCORE))
        {
            time = PlayerPrefs.GetInt(PLAYERDATA_HIGHSCORE);
        }
        return time;
    }


    //
    // SHOP
    //

    // Get/Set Selected items
    public bool setSelectedHat(int id)
    {
        if (getHatUnclokedById(id))
        {
            Debug.Log("Hat: " + id + " selected");
            PlayerPrefs.SetInt(SELECTED_HAT, id);
            PlayerPrefs.Save();
            return true;
        }
        else
        {
            Debug.Log("Does not own hat!");
            return false;
        }
    }

    public int getSelectedHat()
    {
        int id = 0;
        if (PlayerPrefs.HasKey(SELECTED_HAT))
        {
            id = PlayerPrefs.GetInt(SELECTED_HAT);
        }
        return id;
    }

    public bool setSelectedLevel(int id)
    {
        if (getLevelsUnclokedById(id))
        {
            Debug.Log("Level: " + id + " selected");
            PlayerPrefs.SetInt(SELECTED_LEVEL, id);
            PlayerPrefs.Save();
            return true;
        }
        else
        {
            Debug.Log("Does not own level!");
            return false;
        }
    }

    public int getSelectedLevel()
    {
        int id = 0;
        if (PlayerPrefs.HasKey(SELECTED_LEVEL))
        {
            id = PlayerPrefs.GetInt(SELECTED_LEVEL);
        }
        return id;
    }

    // Get/Set Hats
    public void setHatsUnlocked(string data)
    {
        PlayerPrefs.SetString(SHOP_HATS,  data);
        PlayerPrefs.Save();
    }

    public string getHatsUnlocked()
    {
        string data = "";
        if (PlayerPrefs.HasKey(SHOP_HATS))
        {
            data = PlayerPrefs.GetString(SHOP_HATS);
        }
        return data;
    }

    // Setter hat unclocked by id
    public void setHatUnclokedById(int id, bool isUnlocked)
    {
        string[] hats = splitData(getHatsUnlocked());
        if (id >= 0 && hats.Length > 0 && (hats.Length - 1) >= id)
        {
            hats[id] = isUnlocked ? "1" : "0";
            setHatsUnlocked(mergeData(hats));
        }
    }

    // Getter hat unclocked by id
    public bool getHatUnclokedById(int id)
    {
        string[] hats = splitData(getHatsUnlocked());
        if (id >= 0 && hats.Length > 0 && (hats.Length - 1) >= id)
        {
            if (hats[id].Equals("1"))
                return true;
            else
                return false;
        }
        else return false;
    }

    // Get/Set Levels
    public void setLevelsUnlocked(string data)
    {
        PlayerPrefs.SetString(SHOP_LEVELS, data);
        PlayerPrefs.Save();
    }

    public string getLevelsUnlocked()
    {
        string data = "";
        if (PlayerPrefs.HasKey(SHOP_LEVELS))
        {
            data = PlayerPrefs.GetString(SHOP_LEVELS);
        }
        return data;
    }

    // Setter levels unclocked by id
    public void setLevelsUnclokedById(int id, bool isUnlocked)
    {
        string[] levels = splitData(getLevelsUnlocked());
        if (id >= 0 && levels.Length > 0 && (levels.Length - 1) >= id)
        {
            levels[id] = isUnlocked ? "1" : "0";
            setLevelsUnlocked(mergeData(levels));
        }
    }

    // Getter levels unclocked by id
    public bool getLevelsUnclokedById(int id)
    {
        string[] levels = splitData(getLevelsUnlocked());
        if (id >= 0 && levels.Length > 0 && (levels.Length - 1) >= id)
        {
            if (levels[id].Equals("1"))
                return true;
            else
                return false;
        }
        else return false;
    }

    // Combine string array into string with seperator
    private string mergeData(string[] data)
    {
        string mergedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            mergedData += data[i];
            if (i != data.Length - 1)
                mergedData += _seperator;
        }
        Debug.Log("MergedData: " + mergedData);
        return mergedData;
    }

    // Split string into array
    private string[] splitData(string data)
    {
        if (!string.IsNullOrEmpty(data))
        {
            return data.Split(_seperator);
        }
        else
            return null;
    }

    public int buyHat(int id, int price)
    {
        // 0 godkejent kjøp, 1 Ikke nok penger, 2 eier allerede gjenstand
        if (!getHatUnclokedById(id))
        {
            if (getCoins() >= price)
             {
           
                setCoins(getCoins()-price);
                setHatUnclokedById(id, true);
                Debug.Log("Bought hat " + id + " for :" + price + " money left: " + getCoins());
                return 0;
            }
            else return 1;
        }
        else
            return 2;
    }
    public int buyLevel(int id, int price)
    {
        // 0 godkejent kjøp, 1 Ikke nok penger, 2 eier allerede gjenstand
        if (!getLevelsUnclokedById(id))
        {
            if (getCoins() >= price)
            {
           
                setCoins(getCoins() - price);
                setLevelsUnclokedById(id, true);
                Debug.Log("Bought level " + id + " for :" + price + " money left: " + getCoins());
                return 0;
            }
            else return 1;
        }
        else
            return 2;
    }
}
