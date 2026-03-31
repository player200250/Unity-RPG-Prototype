using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SkillUIPanel : MonoBehaviour
{
    //技能面板互動

    public GameObject skillButtonPrefab;  // 拖入 SkillButton Prefab
    public Transform skillPanelParent;    // 拖入 SkillPanel
    private UnitSkillHandler currentHandler; // 目前選取的角色
    public bool isWaitingForTarget = false; // 假設需要選擇目標，這裡可以根據技能類型決定是否需要選擇目標
    public int selectedSkillIndex = -1;  // 選取了第幾個技能，-1 = 沒選

    // 點擊技能按鈕的回調
    public void ShowSkills(UnitSkillHandler handler)
    {
        currentHandler = handler;
        ClearSkillButtons();
        for (int i = 0; i < handler.skills.Count; i++)
        {
            int index = i; // 捕獲當前的索引
            GameObject buttonObj = Instantiate(skillButtonPrefab, skillPanelParent);
            buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = handler.skills[i].skillName;
            buttonObj.GetComponent<Button>().onClick.AddListener(() => OnSkillButtonClicked(index));
            // 補上技能CD顯示
            TextMeshProUGUI cdText = buttonObj.transform.Find("CDText").GetComponent<TextMeshProUGUI>();

            // 設定技能CD倒數判定
            int cd = handler.skills[i].currentCooldown;
            cdText.text = cd > 0 ? $"CD: {cd}" : "Ready";

            // 如果技能正在冷卻中，禁用按鈕(顏色灰掉)
            buttonObj.GetComponent<Button>().interactable = cd <= 0;
        }
    }

    // 點擊技能按鈕的處理
    // 這裡可以根據遊戲邏輯決定是否需要選擇目標等
    public void HideSkills()
    {
        ClearSkillButtons();
        currentHandler = null;
        selectedSkillIndex = -1;
    }

    void OnSkillButtonClicked(int index)
    {
        selectedSkillIndex = index;
        SkillBase skill = currentHandler.skills[index];

        if (skill.targetType == SkillBase.SkillTargetType.Self)
        {
            // 直接對自己施放，不需要等待選目標
            currentHandler.UseActiveSkill(index, currentHandler.GetComponent<UnitStats>());
            isWaitingForTarget = false;
        }
        else
        {
            // 需要選目標
            isWaitingForTarget = true;
            Debug.Log("選擇了技能: " + skill.skillName + "，請選擇目標");
        }
    }

    void ClearSkillButtons()
    {
        // 把 skillPanelParent 底下所有子物件 Destroy 掉
        foreach (Transform child in skillPanelParent)
        {
            Destroy(child.gameObject);
        }
    }
}
