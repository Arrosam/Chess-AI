using UnityEngine;

namespace Chess.Game
{
	[CreateAssetMenu(menuName = "Theme/Pieces3D")]
	public class Piece3DTheme : ScriptableObject
	{
		public Piece3DObeject whitePieces;
		public Piece3DObeject blackPieces;
		public GameObject GetPiecePrefab(int piece)
		{
			Piece3DObeject pieceObject = Piece.IsColour(piece, Piece.White) ? whitePieces : blackPieces;

			switch (Piece.PieceType(piece))
			{
				case Piece.Pawn:
					return pieceObject.pawn;
				case Piece.Rook:
					return pieceObject.rook;
				case Piece.Knight:
					return pieceObject.knight;
				case Piece.Bishop:
					return pieceObject.bishop;
				case Piece.Queen:
					return pieceObject.queen;
				case Piece.King:
					return pieceObject.king;
				default:
					if (piece != 0)
					{
						Debug.Log(piece);
					}
					return new GameObject("Piece3D");
			}
		}

		[System.Serializable]
		public class Piece3DObeject
		{
			public GameObject pawn, rook, knight, bishop, queen, king;
			public GameObject this[int i]
			{
				get
				{
					return new GameObject[] { pawn, rook, knight, bishop, queen, king }[i];
				}
			}
		}
	}
}