using System;
using Chess.Game;
using Core.Skill;
using UnityEngine;

namespace Chess
{
    public class StatusBoardTest : MonoBehaviour
    {
        public GameBoardManager gameBoardManager;
        private StatusBoard _board;
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
            for (int i = 7; i >= 0; i--)
            {
                for (int j = 0; j < 8; j++)
                {
                     stringBuilder += ToFixedStringLength(_statusArray[i * 8 + j].GetHP().ToString(), 4);
                }
                stringBuilder += "\n";
            }
            Debug.Log(stringBuilder);
        }

        private string ToFixedStringLength(string originalString, int length)
        {
            string newString = originalString;
            while (newString.Length < length)
            {
                newString += " ";
            }
            return newString;
        }
    }
}