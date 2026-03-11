using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    public bool IsPlayerTurn = true;
    public TextMeshProUGUI TurnText;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        TurnText.text = "玩家回合";
    }

    public void EndTurn() 
    {
        IsPlayerTurn = !IsPlayerTurn;
        TurnText.text = IsPlayerTurn ? "玩家回合" : "敵人回合";
        Debug.Log("回合結束，現在是 " + (IsPlayerTurn ? "玩家回合" : "敵人回合"));

        if (!IsPlayerTurn)
        {
            // 如果現在是敵人回合，則開始敵人行動
            EnemyAI enemy = FindObjectOfType<EnemyAI>();
            if (enemy != null)
            {
                // 開始敵人回合
                StartCoroutine(enemy.TakeTurn());
            }
        }
    }




}
