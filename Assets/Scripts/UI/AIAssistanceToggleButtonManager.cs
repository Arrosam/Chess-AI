using System;
using Chess;
using Chess.Game;
using UnityEngine;

namespace UI
{
    public class AIAssistanceToggleButtonManager : MonoBehaviour
    {
        public GameObject toggleButton;
        private bool _aiAssistanceState;
        public bool isWhiteToSwitch = true;

        public void TriggerButton()
        {
            GamePlayerManager.Instance.TriggerAIAssistanceMode(isWhiteToSwitch);
        }
    }
}

