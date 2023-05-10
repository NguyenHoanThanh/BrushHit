using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    public static MapSpawner Instance { get; private set; }

    public List<Transform> mapLevel;
    private const string PLAYER_PREFS_GAME_LEVEL = "gameLevel";

    private int gameLevel;
    private int scoreEveryRounds;
    private void Awake()
    {
        Instance = this;
        gameLevel = PlayerPrefs.GetInt(PLAYER_PREFS_GAME_LEVEL, 0);
    }
    public void NextLevel()
    {
        if (gameLevel + 1 == mapLevel.Count)     //IF next map is null => WIN the game
        {
            GameManager.Instance.ResetScene(false);
            return;
        }

        StartCoroutine(LevelUp());
    }
    IEnumerator LevelUp()   //spawn next map when win a round
    {
        mapLevel[gameLevel].gameObject.SetActive(false);
        yield return new WaitForSeconds(5);
        gameLevel++;
        SetGameLevel(gameLevel);
        mapLevel[gameLevel].gameObject.SetActive(true);
        scoreEveryRounds = GameManager.Instance.GetCurrentScore();
        MenuPanel.Instance.SetResume();
        UIManager.Instance.SetColorForMinor(gameLevel);
        UIManager.Instance.SetBeginAndFinishText(gameLevel);
    }
    public int GetGameLevel()
    {
        return gameLevel;
    }
    public void SetGameLevel(int i)
    {
        gameLevel = i;
        PlayerPrefs.SetInt(PLAYER_PREFS_GAME_LEVEL, gameLevel);
        PlayerPrefs.Save();
    }
    public void LoadMapWhenResume()
    {
        foreach (var map in mapLevel)
        {
            map.gameObject.SetActive(false);
        }
        mapLevel[gameLevel].gameObject.SetActive(true);
        UIManager.Instance.SetColorForMinor(gameLevel);
        UIManager.Instance.SetBeginAndFinishText(gameLevel);
    }
    public void LoadStartMap()
    {
        foreach (var map in mapLevel)
        {
            map.gameObject.SetActive(false);
        }
        gameLevel = 0;
        mapLevel[gameLevel].gameObject.SetActive(true);
        UIManager.Instance.SetColorForMinor(gameLevel);
        UIManager.Instance.SetBeginAndFinishText(gameLevel);
    }
    public int GetScoreEveryRounds()
    {
        return scoreEveryRounds;
    }
}
