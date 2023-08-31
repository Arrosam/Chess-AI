using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess.Game {
	public abstract class Player {
		
		protected bool turnPhaseFinished;

		public abstract void Update ();

		public abstract void StartTurnPhase ();

		public virtual bool AnimateMoving()
		{
			return true;
		}
		
		public virtual void Deregister()
		{
			
		}
		
		protected virtual void ChoseMove (Move move)
		{
			EventManager.OnMoveChosen(move);
			turnPhaseFinished = false;
		}
	}
}