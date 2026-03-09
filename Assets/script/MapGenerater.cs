using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerater : MonoBehaviour
{
    //定義地圖物件與地圖長寬度
    public GameObject MapSlice;
    public int Width = 5;
    public int Height = 5;
    public float Spacing = 1.1f;

    //定義可以儲存遊戲地板的陣列
    private GameObject[,] MapGrid;

    void Start()
    {
        // 確保數值不會太大，防止電腦當機
        if (Width > 50 || Height > 50)
        {
            Debug.LogWarning("地圖太大了，為了安全我幫妳縮小到 10x10 喔！");
            Width = 10;
            Height = 10;
        }

        MapGrid = new GameObject[Width, Height];
        //避免太多格子，所以進行分組
        GameObject Parent = new GameObject("MapParent");


        // 正確的雙重迴圈：只在最內層生成一次
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {

                // X軸用i，Z軸用j，這樣就會平鋪開來
                GameObject tile = Instantiate(MapSlice, new Vector3(i * Spacing, 0, j * Spacing), Quaternion.identity, Parent.transform);
                //存陣列遊戲物件
                //MapGrid[i,j] = tile;
                TileInfo info = tile.AddComponent<TileInfo>(); //優化成動態掛載
                info.Grid_X = i;
                info.Grid_Y = j;

            }
        }
        Debug.Log("地圖生成完畢！總共生成了 " + (Width * Height) + " 個格子。");
    }

}
