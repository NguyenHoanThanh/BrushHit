using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance { get { return instance; } }

    [SerializeField] private GameObject nextLevelPanel;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject hudPanel;
    [SerializeField] private GameObject gameoverPanel;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI gameovertext;
    [SerializeField] private TextMeshProUGUI perfectText;
    [SerializeField] private TextMeshProUGUI awesomeText;
    [SerializeField] private TextMeshProUGUI beginText;
    [SerializeField] private TextMeshProUGUI finishText;

    public List<Image> minorList;

    public bool check;

    RectTransform recTranformNextLevel;
    RectTransform recTranformMenu;

    private void Awake()
    {
        instance = this;
        recTranformNextLevel = nextLevelPanel.GetComponent<RectTransform>();
        recTranformMenu = menuPanel.GetComponent<RectTransform>();
    }
    private void Start()
    {
        // hide cac panel ko can thiet
        hudPanel.SetActive(false);
        gameoverPanel.SetActive(false);
        

        recTranformNextLevel.localPosition = new Vector3(2000,0,0);
        perfectText.text = " ";
        scoreText.text = " ";
        SetBeginAndFinishText(0);
        minorList[0].color = Color.red;
    }

    #region Next level panel control open or close
    public void NextLevelControl(bool open)
    {
        StartCoroutine(NextLevelMove(open));
    }
    IEnumerator NextLevelMove(bool open)
    {
        //open = set level panel is moving to local position x = 3000
        if (open)
        {
            while (recTranformNextLevel.localPosition.x < 3000)
            {
                recTranformNextLevel.localPosition = Vector3.MoveTowards(recTranformNextLevel.localPosition, new Vector3(3001, 0, 0), Time.deltaTime * 1500);
                yield return null;
            }
            if (GameManager.Instance.ready == true)
            {
                Player.Instance.GetDown();   // control player move down when level panel at local position x = 3000
            }
        }
        else
        {
            nextLevelPanel.GetComponent<NextLevelPanel>().SetLevelText(MapSpawner.Instance.GetGameLevel() + 1);   //set level appear in level panel
            //move level panel to local position x = 0. to annouce next level is coming
            while (recTranformNextLevel.localPosition.x > 1)
            {
                recTranformNextLevel.localPosition = Vector3.MoveTowards(recTranformNextLevel.localPosition, Vector3.zero, Time.deltaTime * 1500);
                yield return null;
            }
            CameraFollow.Instance.MoveToZeroPoint();
        }
    }
    #endregion
    #region Menu control panel. open when start new game
    public void MenuControl(bool open)
    {
        StartCoroutine(MenuMove(open));
    }
    IEnumerator MenuMove(bool open)
    {
        if (open)   //move menu out
        {
            while (recTranformMenu.localPosition.x < 3000)
            {
                recTranformMenu.localPosition = Vector3.MoveTowards(recTranformMenu.localPosition, new Vector3(3001, 0, 0), Time.deltaTime * 3000);
                yield return null;
            }
            Player.Instance.GetDown();
        }
        else
        {
            while (recTranformMenu.localPosition.x > 1)
            {
                recTranformMenu.localPosition = Vector3.MoveTowards(recTranformMenu.localPosition, Vector3.zero, Time.deltaTime * 3000);
                yield return null;
            }
        }

    }
    #endregion

    public void DisplayScore()
    {
        scoreText.text = GameManager.Instance.GetCurrentScore().ToString();
    }
    public void DisplayHUD(bool b)
    {
        hudPanel.SetActive(b);
    }
    public void PerfectTextDisplay()    //appear when colored current turn more than last turn
    {
        StartCoroutine(TriggerPerfectText());
    }
    IEnumerator TriggerPerfectText()
    {
        SoundManager.Instance.PerfectTextSound();
        perfectText.text = "perfect";
        yield return new WaitForSeconds(0.5f);
        perfectText.text = " ";
    }
    public void AwesomeTextDisplay()    //appear when colored current turn more than last turn
    {
        StartCoroutine(TriggerAwesomeText());
    }
    IEnumerator TriggerAwesomeText()
    {
        awesomeText.transform.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        awesomeText.transform.gameObject.SetActive(false);
    }
    public void SetBeginAndFinishText(string begin, string end)
    {
        beginText.text = begin;
        finishText.text = end;
    }
    public void SetBeginAndFinishText(int x)
    {
        int numberOfStages = 4;
        x = x / numberOfStages;
        beginText.text = (x + 1).ToString();
        finishText.text = (x + 2).ToString();
    }
    public void SetColorForMinor(int gameLv)
    {

        gameLv = gameLv % 4;
        if(gameLv == 0)
        {
            foreach (var item in minorList)
            {
                item.color = Color.white;
            }
        }
        for (int i = 0; i <= gameLv; i++)
        {
            minorList[i].color = Color.red;
        }
    }
    #region Win - Lose Panel Area
    public void WinLosePanelActione(bool lose)
    {
        StartCoroutine(WinLoseLogic(lose));
    }
    IEnumerator WinLoseLogic(bool lose)
    {
        if (lose)
        {
            gameovertext.text = "lose";
            gameoverPanel.SetActive(true);
            yield return null;
        }
        else
        {
            gameovertext.text = "win";
            gameoverPanel.SetActive(true);
            yield return null;
        }
    }
    #endregion
}
