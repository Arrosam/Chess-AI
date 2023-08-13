namespace Chess.Game {
	using System.Threading.Tasks;
	using System.Threading;

	public class AIPlayer : Player {

		Search search;
		AISettings settings;
		bool moveFound;
		Move move;
		CancellationTokenSource cancelSearchTimer;

		public AIPlayer (Board board, AISettings settings) {
			this.settings = settings;
			settings.requestAbortSearch += TimeOutThreadedSearch;
			search = new Search (board, settings);
			search.onSearchComplete += OnSearchComplete;
		}

		// Update running on Unity main thread. This is used to return the chosen move so as
		// not to end up on a different thread and unable to interface with Unity stuff.
		public override void Update () {
			if (moveFound) {
				moveFound = false;
				ChoseMove (move);
			}
            

		}

		public override void NotifyTurnToMove () {
			moveFound = false;
			if (settings.useThreading) {
				StartThreadedSearch ();
			} else {
				StartSearch ();
			}
			
		}

		void StartSearch () {
			search.StartSearch ();
			moveFound = true;
		}

		void StartThreadedSearch () {
			//Thread thread = new Thread (new ThreadStart (search.StartSearch));
			//thread.Start ();
			Task.Factory.StartNew (() => search.StartSearch (), TaskCreationOptions.LongRunning);

			if (!settings.endlessSearchMode) {
				cancelSearchTimer = new CancellationTokenSource ();
				Task.Delay (settings.searchTimeMillis, cancelSearchTimer.Token).ContinueWith ((t) => TimeOutThreadedSearch ());
			}

		}

		// Note: called outside of Unity main thread
		void TimeOutThreadedSearch () {
			if (cancelSearchTimer == null || !cancelSearchTimer.IsCancellationRequested) {
				search.EndSearch ();
			}
		}

		void OnSearchComplete (Move move) {
			// Cancel search timer in case search finished before timer ran out (can happen when a mate is found)
			cancelSearchTimer?.Cancel ();
			moveFound = true;
			this.move = move;
		}
	}
}