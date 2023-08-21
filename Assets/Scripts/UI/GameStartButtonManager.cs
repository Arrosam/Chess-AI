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
            var humanPlayer = PlayerType.Human;
            var aiPlayer = PlayerType.AI;
            gameManager.NewGame (humanPlayer, aiPlayer);
        }

        public void NewAIvsAIGame()
        {
            var aiPlayer = PlayerType.AI;
            gameManager.NewGame(aiPlayer, aiPlayer);
        }
    }
}