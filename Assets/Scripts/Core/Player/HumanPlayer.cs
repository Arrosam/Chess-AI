namespace Chess.Game {
	public class HumanPlayer : Player {
		
		private PlayerInputManager _playerInputManager = new PlayerInputManager();

		public override void StartTurnPhase () {

		}

		public override void Update () {
			_playerInputManager.HandleInput ();
			if (_playerInputManager.IfMoveFound())
			{
				_playerInputManager.ResetMoveFound();
				ChoseMove(_playerInputManager.GetMoveFound());
			}
		}
        
		public override bool AnimateMoving()
		{
			return false;
		}
	}
}