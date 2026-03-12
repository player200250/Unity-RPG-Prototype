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

    void Start()
    {
        CurrentHP = MaxHP; // 初始化當前HP為最大HP
    }

    // 玩家受到攻擊敵人攻擊(受到傷害與死亡判定)
    public void TakeDamage(int damage)
    {
        CurrentHP -= damage;
        Debug.Log($"{gameObject.name} 受到 {damage} 點傷害，剩餘 HP: {CurrentHP}");
        if (CurrentHP <= 0)
        {
            // 確認單位死亡
            Debug.Log($"{gameObject.name} 已經死亡！");
            // 這裡可以添加死亡動畫、掉落物品等邏輯

            // 勝負判定：如果玩家死亡，顯示遊戲結束；如果敵人死亡，檢查是否還有其他敵人，如果沒有則顯示勝利
            TurnManager.Instance.CheckGameOver();

            Destroy(gameObject); // 暫時直接刪除物件
        }
    }

}
