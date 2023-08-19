using System.Threading;
using System.Threading.Tasks;

namespace Chess.Game
{
    public class HybridPlayer : Player
    {
        private PlayerSearch _playerSearch;
        private bool _aiAssistantMode;
        private bool _turnPhaseFinished;

        public HybridPlayer(Board board, AISettings settings, GameSettings gameSettings)
        {
            _playerSearch = new PlayerSearch(board, settings);
            _aiAssistantMode = gameSettings.defaultAIAssistance;
            _turnPhaseFinished = false;
        }
        public override void Update()
        {
            if (_aiAssistantMode && _turnPhaseFinished && _playerSearch.IfMoveFound())
            {
                ChoseMove(_playerSearch.GetMoveFound());
                _turnPhaseFinished = false;
            }
        }

        public override void StartTurnPhase()
        {
            if (_aiAssistantMode)
            {
                ActivateAiAssistanceSearch();
            }
        }

        public void ActivateAiAssistanceSearch()
        {
            _aiAssistantMode = true;
            _playerSearch.StartThreadedSearch();
        }
    }
}