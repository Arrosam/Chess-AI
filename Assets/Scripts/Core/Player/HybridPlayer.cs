﻿using System.Threading;
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
            EventManager.ToggleAIAssistanceSearch += TriggerAIAssistanceSearch;
        }

        public override void Deregister()
        {
            EventManager.ToggleAIAssistanceSearch -= TriggerAIAssistanceSearch;
            base.Deregister();
        }

        public override void Update()
        {
            _playerInputManager.HandleInput();
            if (_aiAssistantMode && turnPhaseFinished && _playerSearch.IfMoveFound())
            {
                _aiAssistantMode = false;
            }
        }

        public override void StartTurnPhase()
        {
            if (_aiAssistantMode)
            {
                _playerSearch.StartThreadedSearch();
            }
            turnPhaseFinished = true;
        }

        public void TriggerAIAssistanceSearch()
        {
            _aiAssistantMode = !_aiAssistantMode;
            if (_aiAssistantMode && GamePlayerManager.Instance.PlayerToMove == this)
            {
                _playerSearch.StartThreadedSearch();
            }
        }

        public override bool AnimateMoving()
        {
            return _aiAssistantMode;
        }
    }
}