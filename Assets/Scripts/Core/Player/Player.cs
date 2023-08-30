using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess.Game {
	public abstract class Player {
		
		protected bool turnPhaseFinished;
		private GameManager _gameManager = Object.FindObjectOfType<GameManager>();

		public abstract void Update ();

		public abstract void StartTurnPhase ();

		public virtual bool AnimateMoving()
		{
			return true;
		}
		
		protected virtual void ChoseMove (Move move)
		{
			_gameManager.OnMoveChosen(move);
			turnPhaseFinished = false;
		}
	}
}