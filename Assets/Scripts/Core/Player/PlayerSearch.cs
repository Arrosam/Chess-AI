using System.Threading;
using System.Threading.Tasks;

namespace Chess.Game
{
    public class PlayerSearch
    {
        Search _search;
        AISettings _settings;
        bool _moveFound;
        Move _move;
        CancellationTokenSource _cancelSearchTimer;

        public PlayerSearch(Board board, AISettings aiSettings)
        {
            _settings = aiSettings;
            _search = new Search(board, _settings);
            _search.onSearchComplete += OnSearchComplete;
        }

        public void StartThreadedSearch()
        {
            _moveFound = false;
            Task.Factory.StartNew(() => _search.StartSearch(), TaskCreationOptions.LongRunning);
            
            _cancelSearchTimer = new CancellationTokenSource();
            Task.Delay(_settings.searchTimeMillis, _cancelSearchTimer.Token)
                .ContinueWith((t) => TimeOutThreadedSearch());
        }

        // Note: called outside of Unity main thread
        void TimeOutThreadedSearch()
        {
            if (_cancelSearchTimer == null || !_cancelSearchTimer.IsCancellationRequested)
            {
                _search.EndSearch();
            }
        }

        void OnSearchComplete(Move move)
        {
            // Cancel search timer in case search finished before timer ran out (can happen when a mate is found)
            _cancelSearchTimer?.Cancel();
            _moveFound = true;
            _move = move;
        }

        public Move GetMoveFound()
        {
            return _move;
        }

        public bool IfMoveFound()
        {
            return _moveFound;
        }

        public void ResetMoveFound()
        {
            _moveFound = false;
        } 
    }
}