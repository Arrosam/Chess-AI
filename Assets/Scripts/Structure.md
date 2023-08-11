```mermaid
    classDiagram
    class GameManager{
        enum Result
        enum PlayerType
        ---
        event System.Action onPositionLoaded
        event System.Action onMoveMade
        ---
        bool loadCustomPosition
        string customPosition
        PlayerType whitePlayerType;
        PlayerType blackPlayerType;
        AISettings aiSettings;
        Color[] colors;
        bool useClocks;
        Clock whiteClock;
        Clock blackClock;
        TMPro.TMP_Text aiDiagnosticsUI;
        TMPro.TMP_Text resultUI;
        ---
        Result gameResult;
        //存储游戏状态，如游戏中，游戏结束
        ---
        Player whitePlayer;
        Player blackPlayer;
        Player playerToMove;
        //存储玩家类型，仅在开始游戏时使用
        ---
        List<Move> gameMoves;
        BoardUI boardUI;
        
        Start()
        //关闭计时器，新建boardUI，game Moves，board，searchBoard，aiSettings()
        //调用NewGame()
        Update()
        //如果还在游戏中，就持续更新当前操作的玩家()
        OnMoveChosen()
        //调用BoardUI.OnMoveMoade显示移动棋子动画()
        //手动移动棋子时调用board和searchBoard来移动棋子，并生成移动棋子事件()
        NewGame()
        //新建游戏，控制棋盘方向。()
        NewCompoterVersusComputerGame()
        
        //()
    }
```