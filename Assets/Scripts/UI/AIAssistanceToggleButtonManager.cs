using System;
using Chess;
using Chess.Game;
using UnityEngine;

namespace UI
{
    public class AIAssistanceToggleButtonManager : MonoBehaviour
    {
        public GameObject toggleButton;
        public GameManager gameManager;
        private GamePlayerManager _playerManager;
        private GameSettings gameSettings;
        private bool _aiAssistanceState;
        public bool isWhiteToSwitch = true;

        public void Start()
        {
            _playerManager = gameManager._gamePlayerManager;
            gameSettings = gameManager.gameSettings;
            _aiAssistanceState = gameSettings.defaultAIAssistance;
        }

        public void TriggerButton()
        {
            _aiAssistanceState = !_aiAssistanceState;
            _playerManager.SwitchAIAssistanceMode(isWhiteToSwitch, _aiAssistanceState);
        }
    }
}

