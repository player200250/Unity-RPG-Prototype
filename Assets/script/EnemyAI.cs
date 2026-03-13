using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private UnitMover unitMover;

    void Start()
    {
        unitMover = GetComponent<UnitMover>();
    }

    // 敵人回合時呼叫這個方法
    // 這裡的邏輯非常簡單：隨機移動一格，然後結束回合
    // 你可以根據需要添加更多的行為，例如攻擊玩家、尋找最近的玩家等
    // 注意：這裡的邏輯非常簡單，實際遊戲中可能需要更複雜的AI行為
    // 這裡使用協程來模擬敵人行動的過程，讓它看起來更自然 
    public IEnumerator TakeTurn()
    {
        yield return new WaitForSeconds(1f); // 等一秒再行動


        #region 舊邏輯(太笨了)
        // 這裡的邏輯非常簡單：隨機移動一格，然後結束回合(舊方法)
        //// 隨機移動一格
        //int randomX = Random.Range(-1, 2);
        //int randomY = Random.Range(-1, 2);

        //int targetX = unitMover.CurrentTile.Grid_X + randomX;
        //int targetY = unitMover.CurrentTile.Grid_Y + randomY;

        //Vector2Int key = new Vector2Int(targetX, targetY);
        //if (MapGenerater.Instance.TileMap.ContainsKey(key))
        //{
        //    TileInfo targetTile = MapGenerater.Instance.TileMap[key];
        //    unitMover.MoveTo(targetTile);
        //    Debug.Log($"敵人移動到:({targetX}, {targetY})");
        //}

        //// 行動完畢，切換回玩家回合
        //TurnManager.Instance.EndTurn();
        #endregion
        #region
        // 新方法：尋找最近的玩家並移動
        // 這裡的邏輯是：找到所有玩家，計算距離，選擇最近的玩家，然後朝那個玩家移動一格 
        #endregion

        // 用Tag找到玩家
        UnitMover player = GameObject.FindWithTag("Player")?.GetComponent<UnitMover>();

        // 加這個null檢查
        if (player == null)
        {

            TurnManager.Instance.EndTurn();
            yield break; // 缺少這行！沒有return會繼續執行下面的程式碼
        }


        UnitStats enemyStats = GetComponent<UnitStats>();
        UnitStats playerStats = player.GetComponent<UnitStats>();


        // 計算距離並選擇最近的玩家
        // 邏輯：直接選擇第一個玩家，實際遊戲中可能需要更複雜的邏輯來選擇最近的玩家
        int dx = player.CurrentTile.Grid_X - unitMover.CurrentTile.Grid_X; // 實際遊戲中需要根據玩家的位置來決定移動x方向
        int dy = player.CurrentTile.Grid_Y - unitMover.CurrentTile.Grid_Y; // 實際遊戲中需要根據玩家的位置來決定移動y方向

        int moveX = dx == 0 ? 0 : (dx > 0 ? 1 : -1);
        int moveY = dy == 0 ? 0 : (dy > 0 ? 1 : -1);

        int targetX = unitMover.CurrentTile.Grid_X + moveX;
        int targetY = unitMover.CurrentTile.Grid_Y + moveY;

        Vector2Int key = new Vector2Int(targetX, targetY);
        if (MapGenerater.Instance.TileMap.ContainsKey(key))
        {
            TileInfo targetTile = MapGenerater.Instance.TileMap[key];
            yield return StartCoroutine(unitMover.MoveTo(targetTile));
            Debug.Log($"敵人移動到:({targetX}, {targetY})");
        }
        // 計算距離並攻擊玩家(因為單找(dx)和(dy)不太準)
        int distance = Mathf.Abs(dx) + Mathf.Abs(dy);
        if (distance <= enemyStats.AttackRange)
        {
            playerStats.TakeDamage(enemyStats.AttackPower);
            Debug.Log($"敵人攻擊玩家，造成 {enemyStats.AttackPower} 點傷害");

            // 檢查玩家是否死亡
            if (playerStats.CurrentHP <= 0)
            {
                yield break;
            }
        }

        //// 行動完畢，切換回玩家回合
        TurnManager.Instance.EndTurn();
    }
}