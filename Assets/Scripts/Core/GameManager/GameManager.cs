using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chess.Game {
	public class GameManager : MonoBehaviour
	{

		public event System.Action OnPositionLoaded;
		public event System.Action<Move> OnMoveMade;

		public bool loadCustomPosition;
		public string customPosition = "1rbq1r1k/2pp2pp/p1n3p1/2b1p3/R3P3/1BP2N2/1P3PPP/1NBQ1RK1 w - - 0 1";

		public TMPro.TMP_Text resultUI;

		Result _gameResult;
		BoardUI _boardUI;

		public AISettings whiteAISettings;
		public AISettings blackAISettings;
		public GameSettings gameSettings;

		public GameBoardManager _gameBoardManager { get; private set; }

		public GamePlayerManager _gamePlayerManager { get; private set; }

		public PlayerType defaultWhitePlayerType = PlayerType.Human;
		public PlayerType defaultBlackPlayerType = PlayerType.AI;

		void Start () {
			Application.targetFrameRate = 60;

			_boardUI = FindObjectOfType<BoardUI> ();
			_gameBoardManager = new GameBoardManager();
			_gamePlayerManager = new GamePlayerManager(whiteAISettings, blackAISettings, gameSettings);

			NewGame (defaultWhitePlayerType, defaultBlackPlayerType);
		}

		void Update () {
			if (_gameResult == Result.Playing) {
				_gamePlayerManager.UpdatePlayerToMove();
			}
		}

		void OnMoveChosen (Move move)
		{
			bool animateMove = _gamePlayerManager.IfCurrentPlayerAnimateMoving();
			_gameBoardManager.MakeMove(move);
            
			//OnMoveMade?.Invoke (move);
			_boardUI.OnMoveMade (_gameBoardManager.Board, move, animateMove);

			StartTurnPhase ();
		}

		public void NewGame (PlayerType whitePlayerType, PlayerType blackPlayerType) {
			if (loadCustomPosition) {
				_gameBoardManager.LoadPosition(customPosition);
			} else {
				_gameBoardManager.LoadPosition();
			}
			OnPositionLoaded?.Invoke ();
			_boardUI.UpdatePosition (_gameBoardManager.Board);
			_boardUI.ResetSquareColours ();
            
			_gamePlayerManager.LoadPlayerType(whitePlayerType, blackPlayerType);
			_gamePlayerManager.InitializePlayer(_gameBoardManager, OnMoveChosen);

			_gameResult = Result.Playing;
			PrintGameResult (_gameResult);

			StartTurnPhase ();

		}

		public void QuitGame () {
			Application.Quit ();
		}

		void StartTurnPhase () {
			_gameResult = _gameBoardManager.GetGameState ();
			PrintGameResult (_gameResult);
			if (_gameResult == Result.Playing)
			{
				_gamePlayerManager.UpdateCurrentPlayer(_gameBoardManager.IfWhitePlayerToMove());
			} else {
				Debug.Log ("Game Over");
			}
		}

		void PrintGameResult (Result result) {
			float subtitleSize = resultUI.fontSize * 0.75f;
			string subtitleSettings = $"<color=#787878> <size={subtitleSize}>";

			if (result == Result.Playing) {
				resultUI.text = "";
			} else if (result == Result.WhiteIsMated || result == Result.BlackIsMated) {
				resultUI.text = "Checkmate!";
			} else if (result == Result.Stalemate) {
				resultUI.text = "Draw";
				resultUI.text += subtitleSettings + "\n(Stalemate)";
			}
		}
	}
}