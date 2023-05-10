using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    public delegate void ResetAction();
    public static event ResetAction onResetAction;

    private const string PLAYER_PREFS_LAST_SCORE = "lastScore";


    [SerializeField] private GameObject oneWayObject;

    [SerializeField] private int index;
    [SerializeField] private int maxIndex;
    [SerializeField] private int score;
    [SerializeField] private bool starGame;

    private int currentPoint;
    private int lastPoint;
    private int lastScore;

    private bool finish;
    private bool textTrigger;

    public int[] pos = new int[3];

    private bool[] tick = new bool[3];
    public bool ready { get;set; }


    private void Awake()
    {
        instance = this;
        lastScore = PlayerPrefs.GetInt(PLAYER_PREFS_LAST_SCORE, 0);
        index = 0;
        starGame = false;
        finish = true;
    }

    public void ConditionToNextRound()
    {
        if (index == maxIndex && !finish)
        {
            lastScore = score;
            PlayerPrefs.SetInt(PLAYER_PREFS_LAST_SCORE, lastScore);
            PlayerPrefs.Save();
            StartCoroutine(FinishGame());
            UIManager.Instance.AwesomeTextDisplay();
            finish = true;
        }
    }
    public void CheckRobberEachTurn()   // check number of robbers each turn. If current turn > last turn =>  trigger text and more point
    {
        if (currentPoint > lastPoint && !textTrigger)
        {
            if (lastPoint != 0)
            {
                score += currentPoint;   //plus point equal point at last turn
                UIManager.Instance.PerfectTextDisplay();
            }
            textTrigger = true;
        }
    }
    public void IndexPlus()
    {
        index++;
        score++;
        //help calculate last robber colored and current point colored
        currentPoint++;
    }
    public void SetMaxIndex(int i)  //set number robbers need to next round
    {
        maxIndex += i;
    }
    public void GetPlus()   // when earn a star. count in player
    {
        Player.Instance.SetStarNumber();
    }
    public void SetStartGame()  //
    {
        starGame = true;
        UIManager.Instance.DisplayHUD(true);    // turn on HUD
        RandomIndexToCreateStar();              // choose number for spawn star
        SetTickFalse();                         // after start game. set all tick is false to new game can spawn star
    }
    public void SetPauseGame()  //pause game make camera not follow player anymore
    {
        starGame = false;
    }
    public bool StarGame()  //get start game state
    {
        return starGame;
    }
    IEnumerator FinishGame()
    {
        SetPauseGame();     //pause camera
        UIManager.Instance.DisplayHUD(false);   // turn off HUD
        //player move to origin position
        Player.Instance.BackToOrigin(false);
        yield return new WaitForSeconds(0.5f);
        index = 0;
        maxIndex = 0;
        MapSpawner.Instance.NextLevel();    // SPAWN next map
        
    }
    public void SetFinish()
    {
        finish = false;
    }
    public int GetCurrentScore()
    {
        return score;
    }
    public void SetScore(int plus)
    {
        score += plus;
    }
    #region calculate number of robbers in last turn and current turn
    // bat dau dam so point cho current point
    public void SetCurrentPointLastPoint()
    {
        lastPoint = currentPoint;
    }
    public void ResetCurrentPoint()
    {
        currentPoint = 0;
    }
    public void ToggleTextTrigeer()
    {
        textTrigger = false;
    }
    #endregion

    #region Restart Button action. replay current round
    public void RestartGame()
    {
        StartCoroutine(RestartAction());
    }
    IEnumerator RestartAction()
    {
        SetPauseGame();
        Player.Instance.BackToOrigin(true);
        index = 0;
        UIManager.Instance.DisplayHUD(false);
        yield return new WaitForSeconds(1);
        onResetAction();
        if (MenuPanel.Instance.IsResume())
        {
            score = lastScore;
        }
        else
        {
            score = MapSpawner.Instance.GetScoreEveryRounds();
        }
            
    }
    #endregion

    public void ResetScene(bool lose)   //main method for reset game and only called when lose or Win last round
    {
        SoundManager.Instance.StopSoundWhenLose();
        SoundManager.Instance.SoundWhenLose();
        lastScore = 0;    
        StartCoroutine(RepairForNewGame(lose));
    }
    IEnumerator RepairForNewGame(bool lose)
    {
        UIManager.Instance.WinLosePanelActione(lose);

        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("GameScene");
    }
    public void ResumeGame()
    {
        score = lastScore;
        MapSpawner.Instance.LoadMapWhenResume();
    }
    public void SetMaxIndexToZero()
    {
        maxIndex = 0;
    }
    public void RandomIndexToCreateStar()
    {
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = Random.Range(100, 200);
        }
    }
    public void CreateStar()
    {
        if(MapSpawner.Instance.GetGameLevel() < 3)
        {
            return;
        }
        if(index != 0)
        {
            if (pos[0] + 2 > index && index > pos[0] - 2 && tick[0] == false)
            {
                Vector3 pos1 = Player.Instance.PositionFromSwim();
                Vector3 pos2 = Player.Instance.RandomPositionFromSwim();
                StarSpawner.Instance.SpawnStar(pos1, pos2);
                tick[0] = true;
            }
            if (pos[1] + 2 > index && index > pos[1] - 2 && tick[1] == false)
                {
                Vector3 pos1 = Player.Instance.PositionFromSwim();
                Vector3 pos2 = Player.Instance.RandomPositionFromSwim();
                StarSpawner.Instance.SpawnStar(pos1, pos2);
                tick[1] = true;
            }
            if (pos[2] + 2 > index && index > pos[2] - 2 && tick[2] == false)
                {
                Vector3 pos1 = Player.Instance.PositionFromSwim();
                Vector3 pos2 = Player.Instance.RandomPositionFromSwim();
                StarSpawner.Instance.SpawnStar(pos1, pos2);
                tick[2] = true;
            }
        }
    }
    public void SetTickFalse()
    {
        for (int i = 0; i < tick.Length; i++)
        {
            tick[i] = false;
        }
    }
}
