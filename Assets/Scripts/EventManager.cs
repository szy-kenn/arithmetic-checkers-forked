using System;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using System.Text.RegularExpressions;

namespace Damath
{
    public class EventManager : MonoBehaviour
    {
        public bool EnableDebug = false;

        #region Global events

        public event Action OnMatchCreate;
        public event Action<MatchController> OnMatchBegin;
        public event Action OnMatchEnd;
        public event Action OnBoardCreate;
        public event Action<Ruleset> OnRulesetCreate;

        #endregion

        #region Player events
        
        public event Action<Player> OnPlayerCreate;
        public event Action<Player> OnPlayerClick;
        public event Action<Player> OnPlayerHold;
        public event Action<Player> OnPlayerRelease;
        public event Action<Player> OnPlayerSelect;
        public event Action<Player> OnPlayerDeselect;
        public event Action<Player> OnPlayerCommand;

        #endregion
        
        #region Match events
        public event Action<Cell> OnCellSelect;
        public event Action<Cell> OnCellDeselect;
        public event Action<int, int> OnCellRequest;
        public event Action<Cell> OnCellReturn;
        public event Action<Cell> OnMoveSelect;
        public event Action<Piece> OnPieceSelect;
        public event Action<Piece> OnPieceWait;
        public event Action<Move> OnPieceMove;
        public event Action<Piece> OnPieceDone;
        public event Action<Move> OnPieceCapture;
        public event Action<List<Move>> OnBoardUpdateMoves;
        public event Action<Dictionary<(int, int), Cell>> OnBoardUpdateCellmap;
        public event Action<MoveType> OnMoveTypeRequest;
        public event Action<bool> OnRequireCapture;
        public event Action OnRefresh;
        public event Action<Side> OnChangeTurn;
        

        #endregion

        public event Action initialize;
        public event Action refresh;
        public event Action createBoard;
        public event Action onCellClick;
        public event Action onPieceClick;
        public event Action onPieceMove;
        public event Action onPieceCapture;

        // Selection
        public event Action<Cell> moveSelectionIndicator;
        public event Action<List<Move>> createMoveIndicatorHandler;

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
        /// <param name="rules"></param>
        public void RulesetCreate(Ruleset rules)
        {
            OnRulesetCreate?.Invoke(rules);
        }

        #endregion

        #region Player event methods

        /// <summary>
        /// Called when a player selects something.
        /// </summary>
        public void PlayerSelect(Player player)
        {
            if (OnPlayerSelect != null)
            {
                OnPlayerSelect(player);
            }
        }

        /// <summary>
        /// Called when player deselects.
        /// </summary>
        public void PlayerDeselect(Player who)
        {
            if (OnPlayerDeselect != null)
            {
                OnPlayerDeselect(who);
            }
        }

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
        /// Called when player clicks.
        /// </summary>
        public void PlayerClick(Player player)
        {
            if (OnPlayerClick != null)
            {
                OnPlayerClick(player);
            }
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
            if (OnMoveSelect != null)
            {
                OnMoveSelect(cell);
            }
        }

        /// <summary>
        /// Called when a piece is selected.
        /// </summary>
        public void PieceSelect(Piece piece)
        {
            if (OnPieceSelect != null)
            {
                OnPieceSelect(piece);
            }
        }

        /// <summary>
        /// Called when piece is waiting for an action.
        /// </summary>
        public void PieceWait(Piece piece)
        {
            if (OnPieceWait != null)
            {
                OnPieceWait(piece);
            }
        }

        /// <summary>
        /// Called when a piece is moved.
        /// </summary>
        public void PieceMove(Move move)
        {
            if (OnPieceMove != null)
            {
                OnPieceMove(move);
            }
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
        public void BoardUpdateMoves(List<Move> moves)
        {
            if (OnBoardUpdateMoves != null)
            {
                OnBoardUpdateMoves(moves);
            }
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
            if (OnPieceCapture != null)
            {
                OnPieceCapture(move);
            }
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
