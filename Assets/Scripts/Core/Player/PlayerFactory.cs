﻿using System;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Chess.Game
{
    public static class PlayerFactory
    {

        public static Player CreatePlayer(PlayerType playerType, AISettings aiSettings, GameSettings gameSettings,
            GameBoardManager gameBoardManager, Action<Move> onMoveChosen)
        {
            Player player;
            if (playerType == PlayerType.Human)
            {
                player = new HumanPlayer(gameBoardManager.Board);
            }else if (playerType == PlayerType.Hybrid)
            {
                player = new HybridPlayer(gameBoardManager.Board, gameBoardManager.SearchBoard, aiSettings,
                    gameSettings);
            }else if (playerType == PlayerType.AI)
            {
                player = new AIPlayer(gameBoardManager.SearchBoard, aiSettings);
            }
            else
            {
                throw new Exception("Player type not recognized");
            }

            player.onMoveChosen += onMoveChosen;
            return player;
        }
    }
}