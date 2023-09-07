using System;
using UnityEngine;

namespace Chess
{
    public class PieceHPSettingManager : MonoBehaviour
    {
        public static PieceHPSettingManager instance;
        public PieceHPSettings pieceHpSettings;
        private float[] _pieceHpArray;

        private void Awake()
        {
            instance = this;
            _pieceHpArray = new float[8];
            _pieceHpArray[Piece.None] = -1;
            _pieceHpArray[Piece.King] = pieceHpSettings.KingHP;
            _pieceHpArray[Piece.Pawn] = pieceHpSettings.PawnHP;
            _pieceHpArray[Piece.Knight] = pieceHpSettings.KnightHP;
            _pieceHpArray[Piece.Bishop] = pieceHpSettings.BishopHP;
            _pieceHpArray[Piece.Rook] = pieceHpSettings.RookHP;
            _pieceHpArray[Piece.Queen] = pieceHpSettings.QueenHP;
        }

        public float GetPieceInitialHP(int pieceIndex)
        {
            return _pieceHpArray[pieceIndex];
        }
    }
}