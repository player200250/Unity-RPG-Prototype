using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

[CreateAssetMenu(fileName = "New Skill Passive Counter Attack", menuName = "Skills/Passive Counter Attack")]
public class SkillPassiveCounterAttack : SkillBase
{
    // 被動技能：反擊
    public void OnTakeDamage(UnitStats attacker ,UnitStats owner)
    {
        if (!CanUse()) return; // 如果技能無法使用，則不執行反擊
        attacker.TakeDamage(owner.AttackPower); // 反擊傷害等於使用者的攻擊力
        //技能重置
        currentCooldown = skillCooldown;
        Debug.Log($"{owner.unitName} 反擊了 {attacker.unitName}！");
    }

}
