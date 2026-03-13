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

    // Update is called once per frame
    void Update()
    {
        // 如果滑鼠在UI上，就不執行以下的點擊偵測
        if (!TurnManager.Instance.IsPlayerTurn) return;
        if (clickCooldown > 0)
        {
            clickCooldown -= Time.deltaTime;
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            // 重置冷卻時間
            clickCooldown = 0.5f;

            // 1. 建立射線
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // 2. 宣告一個空容器來裝結果
            RaycastHit hit;

            // 在偵測地板之前，先偵測角色
            int unitLayer = LayerMask.GetMask("Unit");
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, unitLayer))
            {
                UnitMover unit = hit.collider.GetComponent<UnitMover>();
                if (unit != null)
                {
                    if (!hit.collider.CompareTag("Player"))
                    {
                        UnitStats attackerStats = unitSelected ? selectedUnit.GetComponent<UnitStats>() : null;
                        UnitMover attackerMover = unitSelected ? selectedUnit : null;
                        UnitStats targetStats = unit.GetComponent<UnitStats>();
                        UnitMover targetMover = unit.GetComponent<UnitMover>();

                        unitSelected = false;
                        selectedUnit = null;

                        if (attackerStats != null && attackerMover != null)
                        {
                            int distance = Mathf.Abs(targetMover.CurrentTile.Grid_X - attackerMover.CurrentTile.Grid_X)
                                         + Mathf.Abs(targetMover.CurrentTile.Grid_Y - attackerMover.CurrentTile.Grid_Y);

                            if (distance <= attackerStats.AttackRange)
                                targetStats.TakeDamage(attackerStats.AttackPower);
                            else
                                unit.ShowMoveRange(Color.red);
                        }
                        else
                        {
                            unit.ShowMoveRange(Color.red);
                        }
                        return;
                    }
                    if (unitSelected && selectedUnit == unit)
                    {

                        // 再次點擊同一個角色 → 取消選取
                        unitSelected = false;
                        Debug.Log("取消選取角色");
                    }
                    else
                    {
                        // 選取角色
                        selectedUnit = unit;
                        unitSelected = true;
                        unit.ShowMoveRange(Color.blue); // 顯示移動範圍
                        Debug.Log("選取角色：" + unit.name);
                    }
                    return; // 點到角色就不繼續偵測地板
                }
            }

            // 3. 執行射線偵測
            // 偵測地板"Tile 層"
            int groundLayer = LayerMask.GetMask("Tile"); // 只偵測 Tile 層

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                GameObject HitObject = hit.collider.gameObject;
                Renderer tileRenderer = HitObject.GetComponent<Renderer>();



                if (tileRenderer != null)
                {
                    // 判斷重複點擊同一方塊 (取消選取)
                    if (HitObject == lastSelectedTile)
                    {
                        tileRenderer.material.color = Color.white;
                        lastSelectedTile = null;
                        Debug.Log("取消選取：" + HitObject.name);
                    }
                    else
                    {
                        // 點擊新方塊
                        // 先還原舊的
                        if (lastSelectedTile != null)
                        {
                            // 如果之後要效能優化，這裡的 Renderer 也可以事先存起來
                            lastSelectedTile.GetComponent<Renderer>().material.color = Color.white;
                        }

                        // 設定新的
                        tileRenderer.material.color = Color.yellow;
                        lastSelectedTile = HitObject;

                        TileInfo Info = HitObject.GetComponent<TileInfo>();

                        if (Info != null)
                        {
                            //Debug.Log("選取新格子：" + HitObject.name);
                            Debug.Log($"<color=lime >選取座標: [{Info.Grid_X},{Info.Grid_Y}]</color>");
                            //點擊時生成物件
                            if (unitSelected && selectedUnit != null)
                            {
                                //檢查是否有單位在上面
                                //計算位置: 生成在地板上方
                                //Vector3 spawnObject = HitObject.transform.position + Vector3.up * 1.5f;

                                ////生成物件:
                                //Instantiate(prefab, spawnObject, Quaternion.identity);
                                //Info.IsOccupied = true; //避免重複生成

                                if (hasMoved)
                                {
                                    Debug.LogWarning("這回合已經移動過了，無法再移動！");
                                    return;
                                }
                                hasMoved = true; //標記已經移動過了
                                Debug.Log("已經移動過了");
                                StartCoroutine(selectedUnit.MoveTo(Info));
                            }
                        }
                        else
                        {
                            Debug.LogWarning($"{HitObject.name} 身上沒有掛載 TileInfo ！");
                        }
                    }
                }
            }


        }
    }
}