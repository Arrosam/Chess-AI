namespace Chess.Game
{
    public abstract class PlayerMoveGenerator
    {
        protected Move moveReady;
        protected bool isMoveFound;

        public bool IfMoveFound()
        {
            return isMoveFound;
        }

        public Move GetMoveFound()
        {
            return moveReady;
        }
        
        public void ResetMoveFound()
        {
            isMoveFound = false;
        } 
    }
}