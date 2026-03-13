using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : MonoBehaviour
{
    //顯示單位狀態
    public int MaxHP = 10;
    public int CurrentHP;
    public int AttackPower = 1;
    public int AttackRange = 3;
    public UnityEngine.UI.Image HPBarFill; // 用於顯示HP的UI元素

    void Start()
    {
        CurrentHP = MaxHP; // 初始化當前HP為最大HP
    }

    // 玩家受到攻擊敵人攻擊(受到傷害與死亡判定)
    public void TakeDamage(int damage)
    {
        CurrentHP -= damage;
        if (HPBarFill != null)
        {
            HPBarFill.fillAmount = (float)CurrentHP / MaxHP; // 更新HP條的填充量
        }
        Debug.Log($"{gameObject.name} 受到 {damage} 點傷害，剩餘 HP: {CurrentHP}");
        if (CurrentHP <= 0)
        {
            if (CompareTag("Player"))
            {
                TurnManager.Instance.TurnText.text = "遊戲結束!你輸了!";
                Debug.Log("遊戲結束!你輸了!");
            }
            else
            {
                TurnManager.Instance.CheckGameOver();
            }
            Destroy(gameObject);
        }
    }

}
