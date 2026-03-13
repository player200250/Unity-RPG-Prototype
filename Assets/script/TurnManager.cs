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

    public void EndTurn()
    {
        IsPlayerTurn = !IsPlayerTurn;
        TurnText.text = IsPlayerTurn ? "玩家回合" : "敵人回合";
        Debug.Log("回合結束，現在是 " + (IsPlayerTurn ? "玩家回合" : "敵人回合"));

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

    public void CheckGameOver()
    {
        // 檢查玩家是否死亡
        if (GameObject.FindWithTag("Player") == null)
        {
            TurnText.text = "遊戲結束!你輸了!";
            Debug.Log("遊戲結束!你輸了!");
            return;
        }
        // 檢查敵人是否死亡
        EnemyAI[] enemies = FindObjectsOfType<EnemyAI>();
        if (enemies.Length <= 1)
        {
            TurnText.text = "恭喜!你贏了!";
            Debug.Log("恭喜!你贏了!");
            return;
        }
    }


    public IEnumerator AllEnemiesTakeTurn()
    {
        EnemyAI[] enemies = FindObjectsOfType<EnemyAI>();
        foreach (EnemyAI enemy in enemies)
        {
            yield return StartCoroutine(enemy.TakeTurn());
        }
        CheckGameOver();
        EndTurn(); // 敵人回合結束，切換回玩家回合
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
