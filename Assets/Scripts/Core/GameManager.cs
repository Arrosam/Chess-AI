using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chess.Game {
	public class GameManager : MonoBehaviour {

		public enum Result { Playing, WhiteIsMated, BlackIsMated, Stalemate}

		public event System.Action OnPositionLoaded;
		public event System.Action<Move> OnMoveMade;

		public enum PlayerType { Human, AI, Hybrid }

		public bool loadCustomPosition;
		public string customPosition = "1rbq1r1k/2pp2pp/p1n3p1/2b1p3/R3P3/1BP2N2/1P3PPP/1NBQ1RK1 w - - 0 1";

		public PlayerType whitePlayerType;
		public PlayerType blackPlayerType;
		public AISettings aiSettings;

		public TMPro.TMP_Text resultUI;

		Result _gameResult;

		Player _whitePlayer;
		Player _blackPlayer;
		Player _playerToMove;
		BoardUI _boardUI;

		public Board Board { get; private set; }
		Board _searchBoard; // Duplicate version of board used for ai search

		void Start () {
			Application.targetFrameRate = 60;

			_boardUI = FindObjectOfType<BoardUI> ();
			Board = new Board ();
			_searchBoard = new Board ();

			NewGame (whitePlayerType, blackPlayerType);

		}

		void Update () {
			if (_gameResult == Result.Playing) {
				_playerToMove.Update ();
			}

		}

		void OnMoveChosen (Move move) {
			bool animateMove = _playerToMove is AIPlayer;
			Board.MakeMove (move);
			_searchBoard.MakeMove (move);
            
			OnMoveMade?.Invoke (move);
			_boardUI.OnMoveMade (Board, move, animateMove);

			StartTurnPhase ();
		}

		public void NewGame (PlayerType whitePlayerType, PlayerType blackPlayerType) {
			if (loadCustomPosition) {
				Board.LoadPosition (customPosition);
				_searchBoard.LoadPosition (customPosition);
			} else {
				Board.LoadStartPosition ();
				_searchBoard.LoadStartPosition ();
			}
			OnPositionLoaded?.Invoke ();
			_boardUI.UpdatePosition (Board);
			_boardUI.ResetSquareColours ();

			CreatePlayer (ref _whitePlayer, whitePlayerType);
			CreatePlayer (ref _blackPlayer, blackPlayerType);

			_gameResult = Result.Playing;
			PrintGameResult (_gameResult);

			StartTurnPhase ();

		}

		public void QuitGame () {
			Application.Quit ();
		}

		void StartTurnPhase () {
			_gameResult = GetGameState ();
			PrintGameResult (_gameResult);

			if (_gameResult == Result.Playing) {
				_playerToMove = (Board.WhiteToMove) ? _whitePlayer : _blackPlayer;
				_playerToMove.StartTurnPhase ();

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

		Result GetGameState () {
			MoveGenerator moveGenerator = new MoveGenerator ();
			var moves = moveGenerator.GenerateMoves (Board);

			// Look for mate/stalemate
			if (moves.Count == 0) {
				if (moveGenerator.InCheck ()) {
					return (Board.WhiteToMove) ? Result.WhiteIsMated : Result.BlackIsMated;
				}
				return Result.Stalemate;
			}

			return Result.Playing;
		}

		void CreatePlayer (ref Player player, PlayerType playerType) {
			if (player != null) {
				player.onMoveChosen -= OnMoveChosen;
			}

			if (playerType == PlayerType.Human) {
				player = new HumanPlayer (Board);
			} else {
				player = new AIPlayer (_searchBoard, aiSettings);
			}
			player.onMoveChosen += OnMoveChosen;
		}
	}
}