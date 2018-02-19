using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControlScript : MonoBehaviour {

    public const int STATE_INIT = 0, STATE_RUNNING = 1, STATE_PAUSED = 2, STATE_GAMEOVER = 3;
    private int _gameState;
    public GameObject Player, LevelHandler;

    private AudioSource _aud;

    private IOHandler IO;

    private float _timePlayed = 0f;

    private void Start()    
    {
        IO = new IOHandler();
        setGameState(STATE_INIT);
        _aud = GetComponent<AudioSource>();
    }

    void Update()
    {
        switch (_gameState)
        {
            case STATE_INIT:
                break;

            case STATE_RUNNING:
                onRunning();
                togglePause();
                break;

            case STATE_PAUSED:
                togglePause();
                break;

            case STATE_GAMEOVER:
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
                {
                    buttonRetry();
                }
                break;
        }

	}

    // Runs once every change of gamestate
    public void setGameState(int state)
    {
        _gameState = state;

        switch(_gameState)
        {
            case STATE_INIT:
                Debug.Log("GameState: Init");
                setGameState(STATE_RUNNING);
                if (_aud != null)
                    if (IO.getMusic())
                        _aud.Play();
                    else
                        _aud.Stop();
                break;

            case STATE_RUNNING:
                Debug.Log("GameState: Running");
                Time.timeScale = 1f;
                if (_aud != null)
                    if (IO.getMusic())
                        _aud.Play();
                    else
                        _aud.Stop();
                break;

            case STATE_PAUSED:
                Debug.Log("GameState: Paused");
                onPaused();
                if (_aud != null)
                        _aud.Pause();

                if (Player.GetComponent<PlayerScript>().getAudioSource() != null)
                    Player.GetComponent<PlayerScript>().getAudioSource().Pause();
                break;

            case STATE_GAMEOVER:
                Debug.Log("GameState: GameOver");
                Debug.Log("Played for: " + getTimePretty(_timePlayed));
                onGameOver();
                if (_aud != null)
                        _aud.Stop();
                break;
        }
    }

    // Print out timePlayed in 00:00 format
    private string getTimePretty(float timePlayed)
    {
        System.TimeSpan t = System.TimeSpan.FromSeconds(timePlayed);
        return string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
    }

    public void buttonRetry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void buttonMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void buttonResume()
    {
        levelMove(true);
        Time.timeScale = 1f;
        GetComponent<HudHandler>().showPauseMenu(false);
        setGameState(STATE_RUNNING);
    }

    private void togglePause()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (!GetComponent<HudHandler>().PanelPause.activeSelf)
                setGameState(STATE_PAUSED);
            else
                buttonResume();
        }
    }

    private void onRunning()
    {
        // Add and display time running
        _timePlayed += Time.deltaTime;
        GetComponent<HudHandler>().setTextTime(getTimePretty(_timePlayed));
        GetComponent<HudHandler>().setTextCoins(Player.GetComponent<PlayerScript>().getCoins() + "kr");
        GetComponent<HudHandler>().setHealth(Player.GetComponent<PlayerScript>().getHealth());

        if (_aud != null)
            if (!_aud.isPlaying)
            {
                if (IO.getMusic())
                    _aud.Play();
                else
                    _aud.Stop();
            }

        // Check if player is dead
        if (Player.GetComponent<PlayerScript>().isDead())
            setGameState(STATE_GAMEOVER);

    }

    private void onGameOver()
    {
        int coins = Player.GetComponent<PlayerScript>().getCoins();

        // Save amount coins gained
        IO.setCoins(IO.getCoins() + coins);
        Debug.Log("COINS_LAGRES: " + IO.getCoins());

        if (IO.getHighscore() < _timePlayed)
        {
            // NEW HIGHSCORE YO
            Debug.Log("NY HIGHSCORE!");
            IO.setHighscore((int)_timePlayed);
        }

        levelMove(false);

        GetComponent<HudHandler>().showGameOverMenu(true);
        GetComponent<HudHandler>().setTextTimeGamOver(getTimePretty(_timePlayed));
        GetComponent<HudHandler>().setTextCoinsGamOver( coins + "kr");
    }

    private void onPaused()
    {
        levelMove(false);
        Time.timeScale = 0f;
        int coins = Player.GetComponent<PlayerScript>().getCoins();
        GetComponent<HudHandler>().showPauseMenu(true);
        GetComponent<HudHandler>().setTextPausedTime(getTimePretty(_timePlayed));
        GetComponent<HudHandler>().setTextPausedCoins(coins + "kr");
    }

    private void levelMove(bool shouldMove)
    {
        if (LevelHandler != null)
        {
            LevelHandler.GetComponent<LevelHandler>().shouldMoveLevel(shouldMove);
        }
    }
}