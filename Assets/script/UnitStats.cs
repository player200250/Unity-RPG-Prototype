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

    //浮動文字相關
    public GameObject floatingTextPrefab; // 拖入 Prefab
    public Canvas canvas; // 拖入 Canvas

    //單位名稱（可選）
    public string unitName;


    void Start()
    {
        CurrentHP = MaxHP; // 初始化當前HP為最大HP
    }

    // 玩家受到攻擊敵人攻擊(包含受到傷害與死亡判定)
    public void TakeDamage(int damage, UnitStats attacker = null)
    {
        CurrentHP -= damage;
        StartCoroutine(FlashRed());
        if (HPBarFill != null)
        {
            HPBarFill.fillAmount = (float)CurrentHP / MaxHP;
        }
        Debug.Log($"{gameObject.name} 受到 {damage} 點傷害，剩餘 HP: {CurrentHP}");
        SpawnFloatingText($"-{damage}", Color.red);



        if (CurrentHP <= 0)
        {
            if (CompareTag("Player"))
            {
                TurnManager.Instance.TurnText.text = "遊戲結束!你輸了!";
                TurnManager.Instance.gameOverPanel.SetActive(true);
                TurnManager.Instance.IsPlayerTurn = false;
                Debug.Log("遊戲結束!你輸了!");
            }
            else
            {
                TurnManager.Instance.CheckGameOver();
            }
            Destroy(gameObject);
            return; // 死了就不觸發被動
        }

        // 觸發被動技能
        UnitSkillHandler handler = GetComponent<UnitSkillHandler>();
        if (handler != null && attacker != null)
        {
            handler.TriggerPassive(attacker);
        }
    }

    // 攻擊特效(紅光閃爍)
    private IEnumerator FlashRed()
    {
        Renderer renderer = GetComponent<Renderer>();
        Color originalColor = renderer.material.color; // 記住原本顏色
        renderer.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        renderer.material.color = originalColor; // 還原原本顏色
    }

    // 治療方法
    public void Heal(int healAmount)
    {
        CurrentHP += healAmount;
        if (CurrentHP > MaxHP)
        {
            CurrentHP = MaxHP; // 確保當前HP不超過最大HP
        }
        if (HPBarFill != null)
        {
            HPBarFill.fillAmount = (float)CurrentHP / MaxHP; // 更新HP條的填充量
        }
        Debug.Log($"{gameObject.name} 被治療了 {healAmount} 點生命值，當前 HP: {CurrentHP}");
        SpawnFloatingText($"+{healAmount}", Color.green);
    }

    void SpawnFloatingText(string message, Color color)
    {
        GameObject obj = Instantiate(floatingTextPrefab, canvas.transform);
        // 把世界座標轉換成 UI 座標
        Vector2 screenPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 2f);
        obj.transform.position = screenPos;
        obj.GetComponent<FloatingText>().Setup(message, color);
    }

}
