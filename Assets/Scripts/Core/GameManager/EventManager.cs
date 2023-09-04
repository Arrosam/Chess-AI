using System;
using UnityEngine;

namespace Chess.Game
{
    public class EventManager : MonoBehaviour
    {
        public static event Action OnPositionLoaded;
        public static event Action<Move> OnMoveMade;
        public static event Action AfterMoveMade;
        public static event Action OnBoardUpdate; 
        public static event Action ToggleAIAssistanceSearch;  

        public static void OnMoveChosen(Move move)
        {
            OnMoveMade?.Invoke(move);
            OnBoardUpdate?.Invoke();
            AfterMoveMade?.Invoke();
        }

        public static void OnPositionLoad()
        {
            OnPositionLoaded?.Invoke();
        }

        public static void ToggleAI()
        {
            ToggleAIAssistanceSearch?.Invoke();
        }
    }
}