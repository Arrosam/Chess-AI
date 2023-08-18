namespace Chess.Game {
	using System.Threading.Tasks;
	using System.Threading;

	public class AIPlayer : Player
	{
		private PlayerSearch _playerSearch;
		

		public AIPlayer (Board board, AISettings settings)
		{
			_playerSearch = new PlayerSearch(board, settings, ChoseMove);
		}

		// Update running on Unity main thread. This is used to return the chosen move so as
		// not to end up on a different thread and unable to interface with Unity stuff.
		public override void Update () {
			_playerSearch.onUpdate();
		}

		public override void NotifyTurnToMove () {
			_playerSearch.StartThreadedSearch ();
		}
	}
}