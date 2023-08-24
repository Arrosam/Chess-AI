using System;
using Unity.VisualScripting;

namespace Chess.Game
{
    public class GamePlayerManager
    {
        PlayerType whitePlayerType;
        PlayerType blackPlayerType;
        public AISettings whiteAISettings;
        public AISettings blackAISettings;
        public GameSettings gameSettings;
        
        Player _whitePlayer;
        Player _blackPlayer;
        Player _playerToMove;

        public GamePlayerManager(AISettings whiteAISettings, AISettings blackAISettings, GameSettings gameSettings)
        {
            this.whiteAISettings = whiteAISettings;
            this.blackAISettings = blackAISettings;
            this.gameSettings = gameSettings;
        }

        public void InitializePlayer(GameBoardManager gameBoardManager, Action<Move> OnMoveChosen)
        {
            PlayerFactory.LoadGameSettings(gameSettings);
            PlayerFactory.LoadAISettings(whiteAISettings);
            _whitePlayer = PlayerFactory.CreatePlayer(whitePlayerType, gameBoardManager, OnMoveChosen);
            PlayerFactory.LoadAISettings(blackAISettings);
            _blackPlayer = PlayerFactory.CreatePlayer(blackPlayerType, gameBoardManager, OnMoveChosen);
        }

        public void UpdatePlayerToMove()
        {
            _playerToMove.Update();
        }

        public bool IfCurrentPlayerAnimateMoving()
        {
            return _playerToMove.AnimateMoving();
        }

        public void UpdateCurrentPlayer(bool whitePlayerToMove)
        {
            _playerToMove = whitePlayerToMove ? _whitePlayer : _blackPlayer;
            _playerToMove.StartTurnPhase ();
        }

        public void LoadPlayerType(PlayerType whitePlayerType, PlayerType blackPlayerType)
        {
            this.whitePlayerType = whitePlayerType;
            this.blackPlayerType = blackPlayerType;
        }

        public void SwitchAIAssistanceMode(bool isWhiteToSwitch, bool aiAssistanceMode)
        {
            HybridPlayer hybridPlayer = (HybridPlayer)(isWhiteToSwitch ? ref _whitePlayer : ref _blackPlayer);
            hybridPlayer.SwitchAIAssistanceSearch(aiAssistanceMode);
        }
    }
}