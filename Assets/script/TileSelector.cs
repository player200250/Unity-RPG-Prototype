using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileSelecter : MonoBehaviour
{
    private GameObject lastSelectedTile;
    public GameObject prefab;

    //繼承UnitMover的功能
    public UnitMover selectedUnit;
    //用來檢查是否有單位被選取
    private bool unitSelected = false;
    //設定冷卻時間，避免玩家快速點擊造成問題
    private float clickCooldown = 0f;
    //設定一回合只能動一次
    public bool hasMoved = false;
    //攻擊一個回合也只能攻擊一次，(普通攻擊)
    public bool hasAttacked = false;
    //技能面板引用
    public SkillUIPanel skillUIPanel;

    // Update is called once per frame
    void Update()
    {
        if (!TurnManager.Instance.IsPlayerTurn) return;
        if (clickCooldown > 0) { clickCooldown -= Time.deltaTime; return; }
        if (Input.GetMouseButtonDown(0))
        {
            clickCooldown = 0.5f;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (TryHitUnit(ray, out hit)) return;
            TryHitTile(ray, out hit);
        }
    }

    bool TryHitUnit(Ray ray, out RaycastHit hit)
    {
        int unitLayer = LayerMask.GetMask("Unit");
        if (!Physics.Raycast(ray, out hit, Mathf.Infinity, unitLayer)) return false;

        UnitMover unit = hit.collider.GetComponent<UnitMover>();
        if (unit == null) return false;

        if (!hit.collider.CompareTag("Player"))
        {
            HandleEnemyClick(unit);
            return true;
        }

        HandlePlayerClick(unit);
        return true;
    }

    void HandleEnemyClick(UnitMover unit)
    {
        if (!unitSelected) { unit.ShowMoveRange(Color.red); return; }

        UnitStats attackerStats = selectedUnit.GetComponent<UnitStats>();
        UnitStats targetStats = unit.GetComponent<UnitStats>();
        UnitMover targetMover = unit.GetComponent<UnitMover>();

        int distance = Mathf.Abs(targetMover.CurrentTile.Grid_X - selectedUnit.CurrentTile.Grid_X)
                     + Mathf.Abs(targetMover.CurrentTile.Grid_Y - selectedUnit.CurrentTile.Grid_Y);

        if (skillUIPanel.isWaitingForTarget)
        {
            UnitSkillHandler handler = selectedUnit.GetComponent<UnitSkillHandler>();
            SkillBase skill = handler.skills[skillUIPanel.selectedSkillIndex];

            if (skill.targetType == SkillBase.SkillTargetType.Self)
            {
                // 治療自己，不需要距離判斷
                UnitStats selfStats = selectedUnit.GetComponent<UnitStats>();
                handler.UseActiveSkill(skillUIPanel.selectedSkillIndex, selfStats);
            }
            else if (distance <= attackerStats.AttackRange)
            {
                // 攻擊敵人
                handler.UseActiveSkill(skillUIPanel.selectedSkillIndex, targetStats);
            }
            else Debug.LogWarning("目標超出技能範圍！");

            skillUIPanel.isWaitingForTarget = false;
        }
        else if (distance <= attackerStats.AttackRange)
        {
            // 普通攻擊
            if(hasAttacked) { Debug.LogWarning("這回合已經攻擊過了！"); return; }
            hasAttacked = true;
            targetStats.TakeDamage(attackerStats.AttackPower, attackerStats);
            Debug.Log($"{attackerStats.unitName} 攻擊了 {targetStats.unitName}，造成 {attackerStats.AttackPower} 點傷害！");
        }
        else unit.ShowMoveRange(Color.red);

        unitSelected = false;
        selectedUnit = null;
    }

    void HandlePlayerClick(UnitMover unit)
    {
        if (unitSelected && selectedUnit == unit)
        {
            unitSelected = false;
            selectedUnit = null;
            skillUIPanel.HideSkills();
            Debug.Log("取消選取角色");
        }
        else
        {
            selectedUnit = unit;
            unitSelected = true;
            skillUIPanel.ShowSkills(unit.GetComponent<UnitSkillHandler>());
            unit.ShowMoveRange(Color.blue);
            Debug.Log("選取角色：" + unit.name);
        }
    }

    void TryHitTile(Ray ray, out RaycastHit hit)
    {
        int groundLayer = LayerMask.GetMask("Tile");
        if (!Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer)) return;

        GameObject hitObject = hit.collider.gameObject;
        Renderer tileRenderer = hitObject.GetComponent<Renderer>();
        if (tileRenderer == null) return;

        if (hitObject == lastSelectedTile)
        {
            tileRenderer.material.color = Color.white;
            lastSelectedTile = null;
            return;
        }

        if (lastSelectedTile != null)
            lastSelectedTile.GetComponent<Renderer>().material.color = Color.white;

        tileRenderer.material.color = Color.yellow;
        lastSelectedTile = hitObject;

        TileInfo info = hitObject.GetComponent<TileInfo>();
        if (info == null) { Debug.LogWarning($"{hitObject.name} 沒有 TileInfo！"); return; }

        if (unitSelected && selectedUnit != null)
        {
            if (hasMoved) { Debug.LogWarning("這回合已經移動過了！"); return; }
            hasMoved = true;
            StartCoroutine(selectedUnit.MoveTo(info));
        }
    }
}