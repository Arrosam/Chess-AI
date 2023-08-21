using System.Threading;
using System.Threading.Tasks;

namespace Chess.Game
{
    public class HybridPlayer : Player
    {
        private PlayerSearch _playerSearch;
        private Board _searchBoard;
        private bool _aiAssistantMode;

        public HybridPlayer(Board board, Board searchBoard, AISettings settings, GameSettings gameSettings)
        {
            _searchBoard = searchBoard;
            _playerSearch = new PlayerSearch(searchBoard, settings);
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