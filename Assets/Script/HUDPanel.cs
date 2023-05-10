using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HUDPanel : MonoBehaviour
{
    [SerializeField] private Button restartBtn;
    private void Start()
    {
        restartBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.RestartGame();
        });
    }
}
