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
            var humanPlayer = PlayerFactory.PlayerType.Human;
            var aiPlayer = PlayerFactory.PlayerType.AI;
            gameManager.NewGame (humanPlayer, aiPlayer);
        }

        public void NewAIvsAIGame()
        {
            var aiPlayer = PlayerFactory.PlayerType.AI;
            gameManager.NewGame(aiPlayer, aiPlayer);
        }
    }
}