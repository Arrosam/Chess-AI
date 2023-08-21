﻿using System;

namespace Chess.Game
{
    public class GameBoardManager
    {
        public Board Board { get; private set; } = new();
        public Board SearchBoard { get; private set; } = new(); // Duplicate version of board used for ai search

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