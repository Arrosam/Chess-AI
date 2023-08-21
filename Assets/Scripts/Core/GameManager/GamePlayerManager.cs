using System;
using Unity.VisualScripting;

namespace Chess.Game
{
    public class GamePlayerManager
    {
        public PlayerType whitePlayerType;
        public PlayerType blackPlayerType;
        public AISettings aiSettings;
        public GameSettings gameSettings;
        
        Player _whitePlayer;
        Player _blackPlayer;
        Player _playerToMove;

        public void InitializePlayer(PlayerType whitePlayerType, PlayerType blackPlayerType, GameBoardManager gameBoardManager, Action<Move> OnMoveChosen)
        {
            _whitePlayer = PlayerFactory.CreatePlayer(whitePlayerType, aiSettings, gameSettings, gameBoardManager,
                OnMoveChosen);
            _blackPlayer = PlayerFactory.CreatePlayer(blackPlayerType, aiSettings, gameSettings, gameBoardManager,
                OnMoveChosen);
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
    }
}