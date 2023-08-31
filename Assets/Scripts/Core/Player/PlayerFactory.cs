using System;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Chess.Game
{
    public static class PlayerFactory
    {
        static AISettings aiSettings;
        static GameSettings gameSettings;

        public static void LoadAISettings(AISettings aiSettings)
        {
            PlayerFactory.aiSettings = aiSettings;
        }
        
        public static void LoadGameSettings(GameSettings gameSettings)
        {
            PlayerFactory.gameSettings = gameSettings;
        }

        public static Player CreatePlayer(PlayerType playerType)
        {
            Player player;
            if (playerType == PlayerType.Human)
            {
                player = new HumanPlayer();
            }else if (playerType == PlayerType.Hybrid)
            {
                player = new HybridPlayer(aiSettings, gameSettings);
            }else if (playerType == PlayerType.AI)
            {
                player = new AIPlayer(aiSettings);
            }
            else
            {
                throw new Exception("Player type not recognized");
            }   
            return player;
        }
    }
}