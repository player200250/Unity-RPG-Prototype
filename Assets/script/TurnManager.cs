using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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
    // 方法:結束回合，切換玩家和敵人回合
    public void EndTurn()
    {
        IsPlayerTurn = !IsPlayerTurn;
        TurnText.text = IsPlayerTurn ? "玩家回合" : "敵人回合";
        
        Debug.Log("回合結束，現在是 " + (IsPlayerTurn ? "玩家回合" : "敵人回合"));

        // 每次回合切換都倒數CD
        UpdateAllUnitsCoolDown();

        if (!IsPlayerTurn)
        {
            StartCoroutine(AllEnemiesTakeTurn());
        }
        else
        {
            // 玩家回合開始時重置移動
            FindObjectOfType<TileSelecter>().hasMoved = false;
        }
    }
    // 方法: 確認玩家死亡或敵人死亡，決定遊戲結束
    public void CheckGameOver()
    {
        // 檢查玩家是否死亡
        if (GameObject.FindWithTag("Player") == null)
        {
            TurnText.text = "遊戲結束!你輸了!";
            Debug.Log("遊戲結束!你輸了!");
            return;
        }
        // 檢查敵人是否死亡(不是一個)
        EnemyAI[] enemies = FindObjectsOfType<EnemyAI>();
        if (enemies.Length <= 0) //因為 應該是 <= 0 或 == 0
        {
            TurnText.text = "恭喜!你贏了!";
            Debug.Log("恭喜!你贏了!");
            return;
        }
    }

    // 方法:所有敵人執行回合
    public IEnumerator AllEnemiesTakeTurn()
    {
        EnemyAI[] enemies = FindObjectsOfType<EnemyAI>();
        foreach (EnemyAI enemy in enemies)
        {
            yield return StartCoroutine(enemy.TakeTurn());
        }
        CheckGameOver();

        // 玩家還活著才切回玩家回合
        if (GameObject.FindWithTag("Player") != null)
        {
            EndTurn();
            
        }
    }

    // 方法; 更新所有單位的技能冷卻
    public void UpdateAllUnitsCoolDown()
    {
        UnitSkillHandler[] handlers = FindObjectsOfType<UnitSkillHandler>();
        foreach (UnitSkillHandler handler in handlers)
        {
            handler.UpdateOnTurnEndCoolDown();
        }
    }

    // 方法:重新開始遊戲
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
