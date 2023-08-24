using System;
using System.Collections;
using System.Collections.Generic;
using Chess.Game;
using UnityEngine;

namespace Chess
{
    public class GameStartButtonManager : MonoBehaviour
    {
        public GameManager gameManager;
        
        public void NewGame ()
        {
            gameManager.NewGame (PlayerType.Human, PlayerType.AI);
        }

        public void NewAIvsAIGame()
        {
            gameManager.NewGame(PlayerType.AI, PlayerType.AI);
        }

        public void NewHybridVsAIGame()
        {
            gameManager.NewGame(PlayerType.Hybrid, PlayerType.AI);
        }
    }
}