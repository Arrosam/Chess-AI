using UnityEngine;

namespace Chess
{
    public class MoveExecutor
    {
        private Move _move;
        private bool _isMoveReady;
        private System.Action<Move> _choseMove;

        public void PrepareMove(Move move)
        {
            if (!_isMoveReady)
            {
                _move = move;
                _isMoveReady = true;
            }
            else
            {
                Debug.Log("Duplicated Move prepared.");
            }
            
        }

        public void ChoseMove()
        {
            if (_isMoveReady)
            {
                _isMoveReady = false;
                _choseMove(_move);
            }
        }
    }
}