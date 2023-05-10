using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    public static MenuPanel Instance { get; private set; }
    [SerializeField] private Button startGameBtn;
    [SerializeField] private Button resumeBtn;
    [SerializeField] private Button exitBtn;
    private bool isResume = false;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        startGameBtn.onClick.AddListener(() =>
        {
            MapSpawner.Instance.LoadStartMap();
            UIManager.Instance.MenuControl(true);
        });
        resumeBtn.onClick.AddListener(() =>
        {
            isResume = true;
            GameManager.Instance.ResumeGame();
            UIManager.Instance.MenuControl(true);
        });
        exitBtn.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
    public bool IsResume()
    {
        return isResume;
    }
    public void SetResume()
    {
        isResume = !isResume;
    }
}
