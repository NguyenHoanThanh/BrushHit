using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobberController : MonoBehaviour
{
    private MeshRenderer render;
    private bool colored;
    private void Awake()
    {
        render = gameObject.GetComponent<MeshRenderer>();
        colored = false;
    }
    private void OnEnable()
    {
        GameManager.onResetAction += SetColor;
    }
    private void OnDisable()
    {
        GameManager.onResetAction -= SetColor;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !colored)
        {
            render.material.SetColor("_Color", Color.red);
            GameManager.Instance.IndexPlus();
            GameManager.Instance.ConditionToNextRound();
            GameManager.Instance.CreateStar();
            GameManager.Instance.CheckRobberEachTurn();
            UIManager.Instance.DisplayScore();
            colored = true;
        }    
    }
    public void SetColor()
    {
        render.material.SetColor("_Color", Color.white);
        colored = false;
    }
}
