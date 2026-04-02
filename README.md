# Unity RPG Prototype
> 戰棋 RPG 原型 | Unity 2022 | C#

<!-- 建議在這裡放一張遊戲截圖或 GIF -->
<!-- ![gameplay](docs/gameplay.gif) -->

<img width="1596" height="893" alt="image" src="https://github.com/user-attachments/assets/6d210cf3-fe07-41f8-8fd6-ac2cfa86c3de" />


## 🎮 Demo
[▶ YouTube Gameplay 影片](https://youtu.be/XCu5_yI1qCM) ・ [🎮 試玩 Demo](https://player200250.itch.io/last-walk-prototype) ・ [📁 GitHub Repo](https://github.com/player200250/Unity-RPG-Prototype)

---

## 專案說明
使用 Unity 2022 開發的回合制戰棋 RPG 原型，實作從地圖生成、回合管理、AI 行為到戰鬥系統的完整遊戲流程，作為遊戲工程師職位的作品集展示。

---

## 核心系統架構

### 🗺️ 地圖系統
- 程序式格子地圖生成（支援任意尺寸）
- `TileInfo` 元件動態掛載，以 `Dictionary<Vector2Int, TileInfo>` 管理格子資料
- 地板點擊選取（Raycast 射線偵測）
- 單位佔位判定（`IsOccupied` 旗標）

### 🧭 移動系統
- 曼哈頓距離計算移動範圍，菱形範圍視覺化顯示
- 玩家可移動範圍（藍色）、敵人威脅範圍（紅色）即時預覽
- `Vector3.Lerp` 平滑移動動畫（Coroutine 實作）
- 每回合限移動一次

### ⚔️ 戰鬥系統
- 玩家主動攻擊：點選敵人單位觸發，距離判定後扣血
- 每回合限攻擊一次，防止重複攻擊
- 敵人自動攻擊：回合結束後於攻擊範圍內自動觸發
- 受擊紅光閃爍特效（`FlashRed` Coroutine）
- HP 條 UI 即時更新（`fillAmount` 動態計算）

### 🎯 技能系統
- 基於 `ScriptableObject` 架構，技能資料與邏輯分離
- 支援主動技能（治療）與被動技能（反擊）
- `SkillTargetType` 列舉區分技能目標類型（自身 / 敵人）
- 技能 CD 冷卻管理，每回合結束自動倒數
- 技能 UI 面板動態生成按鈕，CD 中自動灰掉不可點擊

### 🤖 敵人 AI
- 每回合自動追蹤最近玩家，往玩家方向移動一格
- 使用 `AllEnemiesTakeTurn()` 集中協程管理所有敵人行動，確保回合順序正確

### 🔄 回合系統
- `TurnManager` 單例管理回合狀態
- 玩家回合 → 敵人回合依序執行 → 切回玩家回合
- 勝負判定：消滅所有敵人獲勝 / 玩家死亡失敗
- 遊戲重置功能（`SceneManager.LoadScene`）

---

## 技術重點

| 技術 | 應用場景 |
|------|---------|
| Singleton Pattern | `TurnManager`、`MapGenerater` 全域管理 |
| ScriptableObject | 技能資料與邏輯分離，支援 Inspector 直接設定 |
| Coroutine 協程鏈 | 移動動畫、敵人AI行動、受擊特效依序執行 |
| Raycast 射線偵測 | 地板點擊、角色選取（分層偵測 Layer Mask）|
| Manhattan Distance | 移動範圍與攻擊範圍計算 |
| Dictionary | 格子座標快速查詢 `O(1)` |
| TextMeshPro | 中文 UI 顯示 |
| Abstract Class | `SkillBase` 抽象基底，子類別強制實作技能效果 |

---

## 開發中功能
- [ ] WebGL 打包上傳 itch.io
- [ ] currentCooldown 共用問題修正
- [ ] 移動動畫路徑優化
- [ ] 多敵人 AI 優先目標選擇
- [ ] 攻擊特效強化

---

## 環境需求
- Unity 2022.x
- TextMeshPro（Package Manager 安裝）
