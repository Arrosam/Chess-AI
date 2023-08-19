using System.Threading;
using System.Threading.Tasks;

namespace Chess.Game
{
    public class HybridPlayer : Player
    {
        private PlayerSearch _playerSearch;
        private bool _aiAssistantMode;

        public HybridPlayer(Board board, AISettings settings, GameSettings gameSettings)
        {
            _playerSearch = new PlayerSearch(board, settings);
            _aiAssistantMode = gameSettings.defaultAIAssistance;
        }
        public override void Update()
        {
            if (_aiAssistantMode && turnPhaseFinished && _playerSearch.IfMoveFound())
            {
                ChoseMove(_playerSearch.GetMoveFound());
                turnPhaseFinished = false;
            }
        }

        public override void StartTurnPhase()
        {
            if (_aiAssistantMode)
            {
                ActivateAiAssistanceSearch();
            }
            turnPhaseFinished = true;
        }

        public void ActivateAiAssistanceSearch()
        {
            _aiAssistantMode = true;
            _playerSearch.StartThreadedSearch();
        }

        public override bool AnimateMoving()
        {
            return _aiAssistantMode;
        }
    }
}