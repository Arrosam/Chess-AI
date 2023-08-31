using System;
using Chess.Game;
using UnityEngine;

namespace Chess
{

    [CreateAssetMenu (menuName = "Settings/Game")]
    public class GameSettings : MonoBehaviour
    {
        public static GameSettings Instance;
        public bool defaultAIAssistance = false;
        public int targetFrameRate = 60;

        private void Awake()
        {
            Instance = this;
        }
    }
}