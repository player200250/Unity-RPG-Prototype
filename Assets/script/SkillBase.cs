using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 技能基類，所有技能都繼承自這個類(因為技能是資料不是場景物件)
public abstract class SkillBase : ScriptableObject
{
    //[SerializeField]
    //public UnitStats owner;  // 技能使用者的狀態，這裡直接放在技能裡面，方便使用
    public string skillName;
    public string skillDescription;
    public int skillCooldown;
    public int currentCooldown;
    // 技能目標類型，決定技能是對敵人、自己還是隊友使用
    public enum SkillTargetType
    {
        Enemy,
        Self,
        Ally
    }

    public SkillTargetType targetType = SkillTargetType.Enemy;

    // 方法:使用技能
    public virtual void UseSkill(UnitStats user, UnitStats target)
    {
        if (!CanUse()) return;  // 先檢查
        currentCooldown = skillCooldown;
        // 子類別覆寫這裡加效果
    }

    // 方法:每回合結束時呼叫，用來倒數 CD
    public void OnTurnEnd()
    {
        if (currentCooldown > 0)
        {
            currentCooldown--; // 每回合結束時減少冷卻時間
        }
        return;
    }
    // 方法:檢查技能是否可用
    public bool CanUse()
    {
        return currentCooldown <= 0;
    }
}
