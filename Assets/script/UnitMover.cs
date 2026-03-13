using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMover : MonoBehaviour
{
    // 這個單位目前站在哪個格子
    public TileInfo CurrentTile;
    // 默認的移動範圍
    public int MoveRange = 3;

    IEnumerator Start()
    {
        yield return null;
        // 初始化單位的位置到目前的格子
        // 用 Raycast 往下打，找到腳下的格子
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            TileInfo tile = hit.collider.GetComponent<TileInfo>();
            if (tile != null)
            {
                CurrentTile = tile;
                CurrentTile.IsOccupied = true;
                transform.position = CurrentTile.transform.position + Vector3.up * 1.5f;
            }
        }
    }


    // 移動到新的格子
    // 添加動畫功能：在移動過程中，讓單位平滑地移動到新的位置，而不是瞬間跳轉 把普通方法改成協程，使用 Lerp 來實現平滑移動
    public IEnumerator MoveTo(TileInfo newTile)
    {
        // 先檢查新的格子是否被佔用
        if (newTile.IsOccupied)
        {
            Debug.Log("這個格子已經被佔用了！");
            yield break;
            // (!重要)運算式裡面加上 yield break; 來停止協程的執行(不是return) 
            // (參考)這裡可以添加一些提示，比如閃爍格子或者播放音效，來告訴玩家這個格子不能移動
        }

        // 使用演算法:曼哈頓距離來檢查新的格子是否在移動範圍內
        int distance = Mathf.Abs(newTile.Grid_X - CurrentTile.Grid_X) 
                     + Mathf.Abs(newTile.Grid_Y - CurrentTile.Grid_Y);

        if(distance > MoveRange)
        {
            Debug.Log("這個格子超出移動範圍了！");
            yield break;
            // (參考)這裡同樣可以添加一些提示，比如閃爍格子或者播放音效，來告訴玩家這個格子不能移動
        }
        // 如果有目前的格子，先把它標記為未佔用
        if (CurrentTile != null)
        {
            CurrentTile.IsOccupied = false;
        }
        // 更新目前的格子為新的格子
        CurrentTile = newTile;
        CurrentTile.IsOccupied = true;

        // 添加動畫功能：在移動過程中，讓單位平滑地移動到新的位置，而不是瞬間跳轉
        Vector3 startPosition = transform.position;
        Vector3 endPosition = newTile.transform.position + Vector3.up * 1.5f;
        float elapsedTime = 0f;
        float moveDuration = 0.5f; // 移動持續時間，可以根據需要調整

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null; // 等待下一幀
        }
        // 更新單位的位置到新的格子的位置
        transform.position = endPosition;


    }

    //呈現移動範圍的功能
    public void ShowMoveRange(UnityEngine.Color color)
    {
        // 這裡可以用不同顏色的方塊來表示移動範圍
        // 這裡的邏輯是：從目前的格子開始，往四個方向擴散，直到達到移動範圍為止
        // 生成半透明的紅色方塊在可移動的格子上

        for (int x = -MoveRange; x <= MoveRange; x++)
        {
            for (int y = -MoveRange; y <= MoveRange; y++)
            {
                int checkX = CurrentTile.Grid_X + x;
                int checkY = CurrentTile.Grid_Y + y;
                // 檢查這個格子是否在地圖範圍內
                if (checkX >= 0 && checkX < MapGenerater.Instance.Width &&
                    checkY >= 0 && checkY < MapGenerater.Instance.Height)
                {
                    // 計算曼哈頓距離
                    int distance = Mathf.Abs(x) + Mathf.Abs(y);
                    if (distance <= MoveRange)
                    {
                        // 在這裡生成一個半透明的紅色方塊來表示可移動的格子
                        Vector3 spawnPosition = new Vector3(checkX * MapGenerater.Instance.Spacing, 0.6f, checkY * MapGenerater.Instance.Spacing);
                        GameObject rangeIndicator = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        rangeIndicator.transform.position = spawnPosition;
                        rangeIndicator.transform.localScale = new Vector3(0.9f, 0.1f, 0.9f) * MapGenerater.Instance.Spacing;
                        rangeIndicator.GetComponent<Renderer>().material.color = new UnityEngine.Color(color.r, color.g, color.b, 0.5f);
                        Destroy(rangeIndicator, 1f); // 一秒後自動銷毀
                    }
                }
            }
        }
    }

}
