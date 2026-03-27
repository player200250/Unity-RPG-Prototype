using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            UnitSkillHandler handler = FindObjectOfType<UnitSkillHandler>();
            handler.UseActiveSkill(0, handler.GetComponent<UnitStats>());
        }
    }
}
