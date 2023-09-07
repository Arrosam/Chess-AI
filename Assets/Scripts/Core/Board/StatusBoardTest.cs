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

        private void Start()
        {
            _board = gameBoardManager.Board;
            _statusArray = _board.StatusSquare;
        }

        public void CheckStatus()
        {
            if (_board is null)
            {
                Debug.Log("Board is void");
            }

            if (_statusArray is null)
            {
                Debug.Log("Status Square is void");
                return;
            }
            for (int i = 0; i < 8; i++)
            {
                string stringBuilder = "";
                for (int j = 0; j < 8; j++)
                {
                    stringBuilder += _statusArray[i*8+j].GetHP();
                }
                Debug.Log(stringBuilder);
            }
        }
    }
}