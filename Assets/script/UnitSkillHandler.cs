using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSkillHandler : MonoBehaviour
{
    // 單位技能處理器，負責管理單位的技能
    public List<SkillBase> skills = new List<SkillBase>();

    // 主動技能觸發
    public void UseActiveSkill(int index, UnitStats target)
    {
        if (index < 0 || index >= skills.Count)
        {
            Debug.LogWarning("技能索引超出範圍！");
            return;
        }
        UnitStats self = GetComponent<UnitStats>();
        skills[index].UseSkill(self, target);
    }
    // 被動技能觸發
    public void TriggerPassive(UnitStats attacker)
    {
        UnitStats self = GetComponent<UnitStats>();
        foreach (SkillBase skill in skills)
        {
            if (skill is SkillPassiveCounterAttack counterAttack)
            {
                counterAttack.OnTakeDamage(attacker,self);
            }
            // 可以在這裡添加更多被動技能的觸發條件
        }
    }
    // 每回合結束時更新技能冷卻
    public void UpdateOnTurnEndCoolDown()
    {
        foreach (SkillBase skill in skills)
        {
            skill.OnTurnEnd();
        }
    }

}
