namespace Chess.Game {
	public class HumanPlayer : Player {
		
		private PlayerInputManager _playerInputManager;
		public HumanPlayer (Board board) {
			_playerInputManager = new PlayerInputManager(board, ChoseMove);
		}

		public override void StartTurnPhase () {

		}

		public override void Update () {
			_playerInputManager.HandleInput ();
		}
        
		public override bool AnimateMoving()
		{
			return false;
		}
	}
}