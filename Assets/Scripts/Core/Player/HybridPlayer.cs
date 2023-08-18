using System.Threading;
using System.Threading.Tasks;

namespace Chess.Game
{
    public class HybridPlayer : Player
    {
        private bool aiAssistanceMode = false;
        private Search search;
        private AISettings settings;
        private bool moveFound;
        private Move move;
        private CancellationTokenSource cancelSearchTimer;

        public HybridPlayer(Board board, AISettings settings)
        {
            this.settings = settings;
            settings.requestAbortSearch += TimeOutThreadedSearch;
            search = new Search (board, settings);
            search.onSearchComplete += OnSearchComplete;
        }
        public override void Update()
        {
            
        }

        public override void NotifyTurnToMove()
        {
            moveFound = false;
            StartThreadedSearch();
        }
        
        void StartSearch () {
            search.StartSearch ();
            moveFound = true;
        }
        
        void StartThreadedSearch () {
            Task.Factory.StartNew (() => search.StartSearch (), TaskCreationOptions.LongRunning);

            if (!settings.endlessSearchMode) {
                cancelSearchTimer = new CancellationTokenSource ();
                Task.Delay (settings.searchTimeMillis, cancelSearchTimer.Token).ContinueWith ((t) => TimeOutThreadedSearch ());
            }

        }
        
        void TimeOutThreadedSearch () {
            if (cancelSearchTimer == null || ! cancelSearchTimer.IsCancellationRequested) {
                search.EndSearch ();
            }
        }
        
        void OnSearchComplete (Move move) {
            // Cancel search timer in case search finished before timer ran out (can happen when a mate is found)
            cancelSearchTimer?.Cancel();
            moveFound = true;
            this.move = move;
        }
    }
}