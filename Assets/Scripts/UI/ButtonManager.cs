using System;
using System.Collections;
using System.Collections.Generic;
using Chess.Game;
using UnityEngine;

namespace Chess
{
    public class ButtonManager : MonoBehaviour
    {
        public GameManager gameManager;
        
        public void NewGame ()
        {
            var humanPlayer = GameManager.PlayerType.Human;
            var aiPlayer = GameManager.PlayerType.AI;
            gameManager.NewGame (humanPlayer, aiPlayer);
        }

        public void NewAIvsAIGame()
        {
            var aiPlayer = GameManager.PlayerType.AI;
            gameManager.NewGame(aiPlayer, aiPlayer);
        }
    }
}