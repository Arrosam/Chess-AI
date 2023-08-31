using System;
using UnityEngine;

namespace Chess.Game
{
    public class GameBoardManager : MonoBehaviour
    {
        public static GameBoardManager Instance { get; private set; }
        public Board Board { get; private set; } = new();
        public Board SearchBoard { get; private set; } = new(); // Duplicate version of board used for ai search
        
        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            EventManager.OnMoveMade += MakeMove;
        }

        private void OnDisable()
        {
            EventManager.OnMoveMade -= MakeMove;
        }

        public void MakeMove(Move move)
        {
            Board.MakeMove(move);
            SearchBoard.MakeMove(move);
        }
    
        public void LoadPosition(String position = FenUtility.startFen)
        {
            Board.LoadPosition(position);
            SearchBoard.LoadPosition(position);
        }

        public bool IfWhitePlayerToMove()
        {
            return Board.WhiteToMove;
        }

        public Result GetGameState () {
            MoveGenerator moveGenerator = new MoveGenerator ();
            var moves = moveGenerator.GenerateMoves (Board);

            // Look for mate/stalemate
            if (moves.Count == 0) {
                if (moveGenerator.InCheck ()) {
                    return (IfWhitePlayerToMove()) ? Result.WhiteIsMated : Result.BlackIsMated;
                }
                return Result.Stalemate;
            }

            return Result.Playing;
        }
        
    }
}