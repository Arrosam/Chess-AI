using System.Collections.Generic;
using Core.Skill;
using UnityEngine;

namespace Chess
{
    public class StatusBoard : Board
    {
		public Status[] statusSquare;
		private PieceHPSettingManager _pieceHpSetting;
		private Status _noneStatus;

		// Make a move on the board
		// The inSearch parameter controls whether this move should be recorded in the game history (for detecting three-fold repetition)
		public void MakeMove (Move move, bool inSearch = false) {
			uint originalCastleState = currentGameState & 15;
			uint newCastleState = originalCastleState;
			currentGameState = 0;

			int opponentColourIndex = 1 - ColourToMoveIndex;
			int moveFrom = move.StartSquare;
			int moveTo = move.TargetSquare;

			int capturedPieceType = Piece.PieceType (square[moveTo]);
			int movePiece = square[moveFrom];
			int movePieceType = Piece.PieceType (movePiece);

			int moveFlag = move.MoveFlag;
			bool isPromotion = move.IsPromotion;
			bool isEnPassant = moveFlag == Move.Flag.EnPassantCapture;
			Status statusOnTargetSquare = statusSquare[moveFrom];

			// Handle non-En Passant captures
			currentGameState |= (ushort) (capturedPieceType << 8);
			if (capturedPieceType != 0 && !isEnPassant) {
				RemovePieceAt(moveTo);
			}

			// Move pieces in piece lists
			MovePiece(moveFrom, moveTo, ref newCastleState);

			int pieceOnTargetSquare = movePiece;
			// Handle promotion
			if (isPromotion)
			{
				pieceOnTargetSquare = Promotion(moveFlag, moveTo);
			} else {
				// Handle other special moves (en-passant, and castling)
				switch (moveFlag) {
					case Move.Flag.EnPassantCapture:
						int epPawnSquare = moveTo + ((ColourToMove == Piece.White) ? -8 : 8);
						currentGameState |= (ushort) (square[epPawnSquare] << 8); // add pawn as capture type
						RemovePieceAt(epPawnSquare);
						break;
					case Move.Flag.Castling:
						Castling(moveTo);
						break;
				}
			}

			// Update the board representation:
			square[moveTo] = pieceOnTargetSquare;
			square[moveFrom] = Piece.None;
			statusSquare[moveTo] = statusOnTargetSquare;
			statusSquare[moveFrom] =  new Status(_pieceHpSetting.GetPieceInitialHP(Piece.None));

			// Pawn has moved two forwards, mark file with en-passant flag
			if (moveFlag == Move.Flag.PawnTwoForward) {
				int file = BoardRepresentation.FileIndex (moveFrom) + 1;
				currentGameState |= (ushort) (file << 4);
				ZobristKey ^= Zobrist.enPassantFile[file];
			}

			// Piece moving to/from rook square removes castling right for that side
			if (originalCastleState != 0) {
				if (moveTo == BoardRepresentation.h1 || moveFrom == BoardRepresentation.h1) {
					newCastleState &= whiteCastleKingsideMask;
				} else if (moveTo == BoardRepresentation.a1 || moveFrom == BoardRepresentation.a1) {
					newCastleState &= whiteCastleQueensideMask;
				}
				if (moveTo == BoardRepresentation.h8 || moveFrom == BoardRepresentation.h8) {
					newCastleState &= blackCastleKingsideMask;
				} else if (moveTo == BoardRepresentation.a8 || moveFrom == BoardRepresentation.a8) {
					newCastleState &= blackCastleQueensideMask;
				}
			}

			// Update zobrist key with new piece position and side to move
			ZobristKey ^= Zobrist.sideToMove;
			ZobristKey ^= Zobrist.piecesArray[movePieceType, ColourToMoveIndex, moveFrom];
			ZobristKey ^= Zobrist.piecesArray[Piece.PieceType (pieceOnTargetSquare), ColourToMoveIndex, moveTo];

			uint oldEnPassantFile = (currentGameState >> 4) & 15;
			if (oldEnPassantFile != 0)
				ZobristKey ^= Zobrist.enPassantFile[oldEnPassantFile];

			if (newCastleState != originalCastleState) {
				ZobristKey ^= Zobrist.castlingRights[originalCastleState]; // remove old castling rights state
				ZobristKey ^= Zobrist.castlingRights[newCastleState]; // add new castling rights state
			}
			currentGameState |= newCastleState;
			gameStateHistory.Push (currentGameState);

			// Change side to move
			WhiteToMove = !WhiteToMove;
			ColourToMove = (WhiteToMove) ? Piece.White : Piece.Black;
			OpponentColour = (WhiteToMove) ? Piece.Black : Piece.White;
			ColourToMoveIndex = 1 - ColourToMoveIndex;
			plyCount++;

			if (!inSearch) {
				if (movePieceType == Piece.Pawn || capturedPieceType != Piece.None) {
					RepetitionPositionHistory.Clear ();
				} else {
					RepetitionPositionHistory.Push (ZobristKey);
				}
			}
		}

		void RemovePieceAt(int square, bool externalCall = false)
		{
			int capturedPieceType = Piece.PieceType (this.square[square]);
			int opponentColourIndex = 1 - ColourToMoveIndex;
            ZobristKey ^= Zobrist.piecesArray[capturedPieceType, opponentColourIndex, square];
            GetPieceList (capturedPieceType, opponentColourIndex).RemovePieceAtSquare (square);
            if (externalCall)
            {
	            this.square[square] = Piece.None;
	            statusSquare[square] = new Status(_pieceHpSetting.GetPieceInitialHP(Piece.None));
            }
		}

		void MovePiece(int moveFrom, int moveTo, ref uint newCastleState)
		{
			int movePiece = square[moveFrom];
			int movePieceType = Piece.PieceType (movePiece);
			if (movePieceType == Piece.King) {
				KingSquare[ColourToMoveIndex] = moveTo;
				newCastleState &= (WhiteToMove) ? whiteCastleMask : blackCastleMask;
			} else {
				GetPieceList (movePieceType, ColourToMoveIndex).MovePiece (moveFrom, moveTo);
			}
		}
		

		int Promotion(int moveFlag, int moveTo)
		{
			int promoteType = 0;
			switch (moveFlag) {
				case Move.Flag.PromoteToQueen:
					promoteType = Piece.Queen;
					queens[ColourToMoveIndex].AddPieceAtSquare (moveTo);
					break;
				case Move.Flag.PromoteToKnight:
					promoteType = Piece.Knight;
					knights[ColourToMoveIndex].AddPieceAtSquare (moveTo);
					break;

			}
			int pieceOnTargetSquare = promoteType | ColourToMove;
			pawns[ColourToMoveIndex].RemovePieceAtSquare (moveTo);
			return pieceOnTargetSquare;
		}

		void Castling(int moveTo)
		{
			bool kingside = moveTo == BoardRepresentation.g1 || moveTo == BoardRepresentation.g8;
			int castlingRookFromIndex = (kingside) ? moveTo + 1 : moveTo - 2;
			int castlingRookToIndex = (kingside) ? moveTo - 1 : moveTo + 1;

			square[castlingRookFromIndex] = Piece.None;
			square[castlingRookToIndex] = Piece.Rook | ColourToMove;

			rooks[ColourToMoveIndex].MovePiece (castlingRookFromIndex, castlingRookToIndex);
			ZobristKey ^= Zobrist.piecesArray[Piece.Rook, ColourToMoveIndex, castlingRookFromIndex];
			ZobristKey ^= Zobrist.piecesArray[Piece.Rook, ColourToMoveIndex, castlingRookToIndex];
		}

		// Load custom position from fen string
		public void LoadPosition (string fen) {
			Initialize ();
			base.LoadPosition(fen);
			var loadedPosition = FenUtility.PositionFromFen (fen);

			// Load pieces into board array and piece lists
			for (int squareIndex = 0; squareIndex < 64; squareIndex++) {
				int piece = loadedPosition.squares[squareIndex];
				int pieceType = Piece.PieceType (piece);
				if (statusSquare is null)
				{
					Debug.Log("status null");
				}
				statusSquare[squareIndex] = new Status(_pieceHpSetting.GetPieceInitialHP(pieceType));
			}
		}

        void Initialize () {
			base.Initialize();
			statusSquare = new Status[64];
			_pieceHpSetting = PieceHPSettingManager.instance;
        }
	}
}