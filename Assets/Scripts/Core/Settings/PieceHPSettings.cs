using UnityEngine;

namespace Chess
{
    [CreateAssetMenu (menuName = "Settings/PieceHP")]
    public class PieceHPSettings : ScriptableObject
    {
        public float KingHP;
        public float PawnHP;
        public float KnightHP;
        public float BishopHP;
        public float RookHP;
        public float QueenHP;
    }
}