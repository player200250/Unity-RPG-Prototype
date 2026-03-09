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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 1. 建立射線
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // 2. 宣告一個空容器來裝結果
            RaycastHit hit;

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
                            if (prefab != null)
                            {
                                //檢查是否有單位在上面
                                //計算位置: 生成在地板上方
                                //Vector3 spawnObject = HitObject.transform.position + Vector3.up * 1.5f;

                                ////生成物件:
                                //Instantiate(prefab, spawnObject, Quaternion.identity);
                                //Info.IsOccupied = true; //避免重複生成

                                //使用 UnitMover 的 MoveTo 功能來移動單位
                                selectedUnit.MoveTo(Info);
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