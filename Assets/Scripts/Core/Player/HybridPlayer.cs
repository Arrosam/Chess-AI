using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Chess.Game
{
    public class HybridPlayer : Player
    {
        private PlayerSearch _playerSearch;
        private PlayerInputManager _playerInputManager;
        private bool _aiAssistantMode;

        public HybridPlayer(AISettings settings, GameSettings gameSettings)
        {
            _playerSearch = new PlayerSearch(settings);
            _playerInputManager = new PlayerInputManager();
            _aiAssistantMode = gameSettings.defaultAIAssistance;
        }
        public override void Update()
        {
            _playerInputManager.HandleInput();
            if (_aiAssistantMode && turnPhaseFinished && _playerSearch.IfMoveFound())
            {
                _aiAssistantMode = false;
                _playerSearch.ResetMoveFound();
                ChoseMove(_playerSearch.GetMoveFound());
            }
        }

        public override void StartTurnPhase()
        {
            _playerSearch.StartThreadedSearch();
            turnPhaseFinished = true;
        }

        public void TriggerAIAssistanceSearch()
        {
            Debug.Log("Assistance Triggered");
            _aiAssistantMode = true;
        }
        


        public override bool AnimateMoving()
        {
            return _aiAssistantMode;
        }
    }
}