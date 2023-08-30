using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chess.Game {
	public class GameManager : MonoBehaviour
	{

		public static GameManager Instance { get; private set; }
		public static event Action OnPositionLoaded;
		public static event Action<Move> OnMoveMade;

		public bool loadCustomPosition;
		public string customPosition = "1rbq1r1k/2pp2pp/p1n3p1/2b1p3/R3P3/1BP2N2/1P3PPP/1NBQ1RK1 w - - 0 1";

		public TMPro.TMP_Text resultUI;

		Result _gameResult;

		public PlayerType defaultWhitePlayerType = PlayerType.Human;
		public PlayerType defaultBlackPlayerType = PlayerType.AI;

		private void Awake()
		{
			Instance = this;
		}

		void Start () {
			Application.targetFrameRate = 60;
			NewGame (defaultWhitePlayerType, defaultBlackPlayerType);
		}

		void Update () {
			if (_gameResult == Result.Playing) {
				GamePlayerManager.Instance.UpdatePlayerToMove();
			}
		}

		public void OnMoveChosen (Move move)
		{
			OnMoveMade?.Invoke (move);
			StartTurnPhase ();
		}

		public void NewGame (PlayerType whitePlayerType, PlayerType blackPlayerType) {
			if (loadCustomPosition) {
				GameBoardManager.Instance.LoadPosition(customPosition);
			} else {
				GameBoardManager.Instance.LoadPosition();
			}
			OnPositionLoaded?.Invoke();
            
			GamePlayerManager.Instance.LoadPlayerType(whitePlayerType, blackPlayerType);
			GamePlayerManager.Instance.InitializePlayer();

			_gameResult = Result.Playing;
			PrintGameResult (_gameResult);

			StartTurnPhase ();

		}

		public void QuitGame () {
			Application.Quit ();
		}

		void StartTurnPhase () {
			_gameResult = GameBoardManager.Instance.GetGameState ();
			PrintGameResult (_gameResult);
			if (_gameResult == Result.Playing)
			{
				GamePlayerManager.Instance.UpdateCurrentPlayer(GameBoardManager.Instance.IfWhitePlayerToMove());
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