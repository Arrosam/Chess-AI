namespace Chess.Game {
	using System.Threading.Tasks;
	using System.Threading;

	public class AIPlayer : Player
	{
		private PlayerSearch _playerSearch;
		

		public AIPlayer (Board board, AISettings settings)
		{
			_playerSearch = new PlayerSearch(board, settings);
		}
        
		public override void Update () {
			if (turnPhaseFinished && _playerSearch.IfMoveFound())
			{
                _playerSearch.ResetMoveFound();
                ChoseMove(_playerSearch.GetMoveFound());
			}
		}

		public override void StartTurnPhase () {
			_playerSearch.StartThreadedSearch ();
			// Only trigger after skill casted
			turnPhaseFinished = true;
		}
	}
}