using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Chess.Game
{
    public class PlayerInputManager
    {
        public enum InputState {
			None,
			PieceSelected,
			DraggingPiece
		}

		InputState _currentState;

		BoardUI _boardUI;
		Camera _cam;
		Coord _selectedPieceSquare;
		Board _board;
		private GameManager _gameManager;
		private Move _intentionalMove;
		
		private Action<Move> _choseMove;
		public PlayerInputManager() {
			_boardUI = Object.FindObjectOfType<BoardUI> ();
			_cam = Camera.main;
			_gameManager = Object.FindObjectOfType<GameManager>();
			_board = GameBoardManager.Instance.Board;
			_choseMove = _gameManager.OnMoveChosen;
		}
		public void HandleInput () {
			Vector2 mousePos = _cam.ScreenToWorldPoint (Input.mousePosition);

			if (_currentState == InputState.None) {
				HandlePieceSelection (mousePos);
			} else if (_currentState == InputState.DraggingPiece) {
				HandleDragMovement (mousePos);
			} else if (_currentState == InputState.PieceSelected) {
				HandlePointAndClickMovement (mousePos);
			}

			if (Input.GetMouseButtonDown (1)) {
				CancelPieceSelection ();
			}
		}

		void HandlePointAndClickMovement (Vector2 mousePos) {
			if (Input.GetMouseButton (0)) {
				HandlePiecePlacement (mousePos);
			}
		}

		void HandleDragMovement (Vector2 mousePos) {
			_boardUI.DragPiece (_selectedPieceSquare, mousePos);
			// If mouse is released, then try place the piece
			if (Input.GetMouseButtonUp (0)) {
				HandlePiecePlacement (mousePos);
			}
		}

		void HandlePiecePlacement (Vector2 mousePos) {
			Coord targetSquare;
			if (_boardUI.TryGetSquareUnderMouse (mousePos, out targetSquare)) {
				if (targetSquare.Equals (_selectedPieceSquare)) {
					_boardUI.ResetPiecePosition (_selectedPieceSquare);
					if (_currentState == InputState.DraggingPiece) {
						_currentState = InputState.PieceSelected;
					} else {
						_currentState = InputState.None;
						_boardUI.DeselectSquare (_selectedPieceSquare);
					}
				} else {
					int targetIndex = BoardRepresentation.IndexFromCoord (targetSquare.fileIndex, targetSquare.rankIndex);
					if (Piece.IsColour (_board.Square[targetIndex], _board.ColourToMove) && _board.Square[targetIndex] != 0) {
						CancelPieceSelection ();
						HandlePieceSelection (mousePos);
					} else {
						TryMakeMove (_selectedPieceSquare, targetSquare);
					}
				}
			} else {
				CancelPieceSelection ();
			}

		}

		void CancelPieceSelection () {
			if (_currentState != InputState.None) {
				_currentState = InputState.None;
				_boardUI.DeselectSquare (_selectedPieceSquare);
				_boardUI.ResetPiecePosition (_selectedPieceSquare);
			}
		}

		void TryMakeMove (Coord startSquare, Coord targetSquare) {
			int startIndex = BoardRepresentation.IndexFromCoord (startSquare);
			int targetIndex = BoardRepresentation.IndexFromCoord (targetSquare);
			bool moveIsLegal = false;
			Move chosenMove = new Move ();

			MoveGenerator moveGenerator = new MoveGenerator ();
			bool wantsKnightPromotion = Input.GetKey (KeyCode.LeftAlt);

			var legalMoves = moveGenerator.GenerateMoves (_board);
			for (int i = 0; i < legalMoves.Count; i++) {
				var legalMove = legalMoves[i];

				if (legalMove.StartSquare == startIndex && legalMove.TargetSquare == targetIndex) {
					if (legalMove.IsPromotion) {
						if (legalMove.MoveFlag == Move.Flag.PromoteToQueen && wantsKnightPromotion) {
							continue;
						}
						if (legalMove.MoveFlag != Move.Flag.PromoteToQueen && !wantsKnightPromotion) {
							continue;
						}
					}
					moveIsLegal = true;
					chosenMove = legalMove;
					//	Debug.Log (legalMove.PromotionPieceType);
					break;
				}
			}

			if (moveIsLegal) {
				_choseMove (chosenMove);
				_intentionalMove = chosenMove;
				_currentState = InputState.None;
			} else {
				CancelPieceSelection ();
			}
		}

		void HandlePieceSelection (Vector2 mousePos) {
			if (Input.GetMouseButtonDown (0)) {
				if (_boardUI.TryGetSquareUnderMouse (mousePos, out _selectedPieceSquare)) {
					int index = BoardRepresentation.IndexFromCoord (_selectedPieceSquare);
					// If square contains a piece, select that piece for dragging
					if (Piece.IsColour (_board.Square[index], _board.ColourToMove)) {
						_boardUI.HighlightLegalMoves (_board, _selectedPieceSquare);
						_boardUI.SelectSquare (_selectedPieceSquare);
						_currentState = InputState.DraggingPiece;
					}
				}
			}
		}
    }
}