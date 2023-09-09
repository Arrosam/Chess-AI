using System;
using Chess.Game;
using Core.Skill;
using UnityEngine;

namespace Chess
{
    public class StatusBoardTest : MonoBehaviour
    {
        public GameBoardManager gameBoardManager;
        private Board _board;
        private Status[] _statusArray;
        
        public void CheckStatus()
        {
            _board = gameBoardManager.Board;
            _statusArray = _board.statusSquare;
            if (_board is null)
            {
                Debug.Log("Board is void");
            }

            if (_statusArray is null)
            {
                Debug.Log("Status Square is void");
                return;
            }
            string stringBuilder = "";
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    stringBuilder += _statusArray[i*8+j].GetHP() + " ";
                }
                stringBuilder += "\n";
            }
            Debug.Log(stringBuilder);
        }
    }
}