using System;
using System.Collections.Generic;
using UnityEngine;

namespace Damath
{
    public class EventManager : MonoBehaviour
    {
        public bool EnableDebugMode = false;

        #region Global events

        public event Action OnMatchCreate;
        public event Action<MatchController> OnMatchBegin;
        public event Action OnMatchEnd;
        public event Action OnBoardCreate;
        public event Action<Ruleset> OnRulesetCreate;

        #endregion

        #region Player events
        
        public event Action<Player> OnPlayerCreate;
        public event Action<Player> OnPlayerLeftClick;
        public event Action<Player> OnPlayerRightClick;
        public event Action<Player> OnPlayerHold;
        public event Action<Player> OnPlayerRelease;
        public event Action<Player> OnPlayerCommand;

        #endregion
        
        #region Match events
        public event Action<Cell> OnCellSelect;
        public event Action<Cell> OnCellDeselect;
        public event Action<int, int> OnCellRequest;
        public event Action<Cell> OnCellReturn;
        public event Action<Cell> OnMoveSelect;
        public event Action<Piece> OnPieceSelect;
        public event Action<Piece> OnPieceDeselect;
        public event Action<Piece> OnPieceWait;
        public event Action<Move> OnPieceMove;
        public event Action<Piece> OnPieceDone;
        public event Action<Move> OnPieceCapture;
        public event Action<List<Move>> OnBoardUpdateValidMoves;
        public event Action<List<Move>> OnBoardUpdateCaptureables;
        public event Action<Dictionary<(int, int), Cell>> OnBoardUpdateCellmap;
        public event Action<MoveType> OnMoveTypeRequest;
        public event Action<bool> OnRequireCapture;
        public event Action OnRefresh;
        public event Action<Side> OnChangeTurn;
        

        #endregion

        // Methods
        #region Global event methods
        
        /// <summary>
        /// Called when a match begins.
        /// </summary>
        public void MatchBegin(MatchController match)
        {
            OnMatchBegin?.Invoke(match);
        }

        /// <summary>
        /// Called when a ruleset is created. 
        /// </summary>
        /// <param name="value"></param>
        public void RulesetCreate(Ruleset value)
        {
            OnRulesetCreate?.Invoke(value);
        }

        #endregion

        #region Player event methods

        /// <summary>
        /// Called when a new player is created.
        /// </summary>
        public void PlayerCreate(Player player)
        {
            if (OnPlayerCreate != null)
            {
                OnPlayerCreate(player);
            }
        }

        /// <summary>
        /// Called when player left clicks.
        /// </summary>
        public void PlayerLeftClick(Player player)
        {
            OnPlayerLeftClick?.Invoke(player);
        }

        public void PlayerRightClick(Player player)
        {
            OnPlayerRightClick?.Invoke(player);
        }

        /// <summary>
        /// Called when player holds.
        /// </summary>
        public void PlayerHold(Player player)
        {
            if (OnPlayerHold != null)
            {
                OnPlayerHold(player);
            }
        }

        /// <summary>
        /// Called when player releases.
        /// </summary>
        public void PlayerRelease(Player player)
        {
            if (OnPlayerRelease != null)
            {
                OnPlayerRelease(player);
            }
        }

        public void PlayerCommand(Player player)
        {
            if (OnPlayerCommand != null)
            {
                OnPlayerCommand(player);
            }
        }

        #endregion

        #region Match event methods

        /// <summary>
        /// Called when a cell is selected.
        /// </summary>
        public void CellSelect(Cell cell)
        {
            if (OnCellSelect != null)
            {
                OnCellSelect(cell);
            }
        }

        /// <summary>
        /// Called when a cell is deselected.
        /// </summary>
        public void CellDeselect(Cell cell)
        {
            OnCellDeselect?.Invoke(cell);
        }
        
        public void CellRequest(int col, int row)
        {
            OnCellRequest?.Invoke(col, row);
        }
        
        public void CellReturn(Cell cell)
        {
            OnCellReturn?.Invoke(cell);
        }

        /// <summary>
        /// Called when a cell with valid move is selected.
        /// </summary>
        public void MoveSelect(Cell cell)
        {
            OnMoveSelect?.Invoke(cell);
        }

        /// <summary>
        /// Called when a piece is selected.
        /// </summary>
        public void PieceSelect(Piece piece)
        {
            OnPieceSelect?.Invoke(piece);
        }

        /// <summary>
        /// Called when a piece is deselected.
        /// </summary>
        public void PieceDeselect(Piece piece)
        {
            OnPieceDeselect?.Invoke(piece);
        }

        /// <summary>
        /// Called when piece is waiting for an action.
        /// </summary>
        public void PieceWait(Piece piece)
        {
            OnPieceWait?.Invoke(piece);
        }

        /// <summary>
        /// Called when a piece is moved.
        /// </summary>
        public void PieceMove(Move move)
        {
            OnPieceMove?.Invoke(move);
        }

        /// <summary>
        /// Called when a piece has no more actions to take.
        /// </summary>
        public void PieceDone(Piece piece)
        {
            if (OnPieceDone != null)
            {
                OnPieceDone(piece);
            }
        }
        
        /// <summary>
        /// Called when the Board updates all its valid moves.
        /// </summary>
        public void BoardUpdateValidMoves(List<Move> moves)
        {
            OnBoardUpdateValidMoves?.Invoke(moves);
        }

        public void BoardUpdateCaptureables(List<Move> moves)
        {
            OnBoardUpdateCaptureables?.Invoke(moves);
        }
        /// <summary>
        /// Called when the Board updates all its valid moves.
        /// </summary>
        public void BoardUpdateCellmap(Dictionary<(int, int), Cell> cellmap)
        {
            OnBoardUpdateCellmap?.Invoke(cellmap);
        }
        
        public void MoveTypeRequest(MoveType moveType)
        {
            if (OnMoveTypeRequest != null)
            {
                OnMoveTypeRequest(moveType);
            }
        }

        public void PieceCapture(Move move)
        {
            OnPieceCapture?.Invoke(move);
        }

        /// <summary>
        /// Called when the Board requests for a turn that has a mandatory capture.
        /// </summary>
        public void RequireCapture(bool value)
        {
            if (OnRequireCapture != null)
            {
                OnRequireCapture(value);
            }
        }

        public void Refresh()
        {
            if (OnRefresh != null)
            {
                OnRefresh();
            }
        }

        public void ChangeTurn(Side side)
        {
            if (OnChangeTurn != null)
            {
                OnChangeTurn(side);
            }
        }

        #endregion
    }

}
