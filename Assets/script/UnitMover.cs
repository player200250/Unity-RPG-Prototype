using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMover : MonoBehaviour
{
    // 這個單位目前站在哪個格子
    public TileInfo CurrentTile;

    // 移動到新的格子  
    public void MoveTo(TileInfo newTile)
    {
        // 先檢查新的格子是否被佔用
        if (newTile.IsOccupied)
        {
            Debug.Log("這個格子已經被佔用了！");
            return;
        }
        // 如果有目前的格子，先把它標記為未佔用
        if (CurrentTile != null)
        {
            CurrentTile.IsOccupied = false;
        }
        // 更新目前的格子為新的格子
        CurrentTile = newTile;
        CurrentTile.IsOccupied = true;
        // 更新單位的位置到新的格子的位置
        transform.position = newTile.transform.position + Vector3.up * 1.5f;
    }

}
