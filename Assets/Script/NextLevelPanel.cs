using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NextLevelPanel : MonoBehaviour
{
    [SerializeField] private Button nextLevelBtn;
    [SerializeField]  private TextMeshProUGUI levelText;
    [SerializeField]  private TextMeshProUGUI roundText;

    private void Start()
    {
        nextLevelBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.NextLevelControl(true);
        });
    }
    public void SetLevelText(int gamelv)
    {
        int level = gamelv / 4;
        int round = gamelv % 4;
        roundText.text = "round " + (round + 1).ToString();
        levelText.text = "level " + (level + 1).ToString();
    }
}
