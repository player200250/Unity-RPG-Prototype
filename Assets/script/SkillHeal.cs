using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill Heal", menuName = "Skills/Heal")]
public class SkillHeal : SkillBase
{

   public override void UseSkill(UnitStats user, UnitStats target)
    {
        if (!CanUse()) return; // 再次檢查技能是否可用（基類可能已經設置了冷卻時間）
        base.UseSkill(user, target); // 先呼叫基類的 UseSkill 來處理冷卻時間
        int healAmount = 3; // 固定治療量，可以根據需要調整
        target.Heal(healAmount); // 呼叫 UnitStats 的 Heal 方法來治療目標
        //Debug.Log($"{user.unitName} 使用 {skillName} 治療了 {target.unitName} {healAmount} 點生命值！");
    }
}
