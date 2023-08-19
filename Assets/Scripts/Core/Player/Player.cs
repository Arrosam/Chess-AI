using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess.Game {
	public abstract class Player {
		public event System.Action<Move> onMoveChosen;
		protected bool turnPhaseFinished;

		public abstract void Update ();

		public abstract void StartTurnPhase ();

		public virtual bool AnimateMoving()
		{
			return true;
		}

		protected virtual void ChoseMove (Move move) {
			onMoveChosen?.Invoke (move);
			turnPhaseFinished = false;
		}
	}
}