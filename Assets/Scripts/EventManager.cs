using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Damath
{
    public class EventManager : MonoBehaviour
    {
        #region Global events

        public event Action OnMatchCreate;
        public event Action<Ruleset> OnMatchBegin;
        public event Action OnMatchEnd;

        #endregion

        #region Player events
        
        public event Action<Player> OnPlayerCreate;
        public event Action<Player> OnPlayerClick;
        public event Action<Player> OnPlayerSelect;
        public event Action<Player> OnPlayerDeselect;

        #endregion
        
        #region Match events
        public event Action<Cell> OnCellSelect;
        public event Action<Piece> OnPieceSelect;
        public event Action<Move> OnMoveSelect;
        public event Action<Move> OnPieceMove;
        public event Action<Move> OnPieceCapture;
        public event Action<MoveType> OnMoveTypeRequest;
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
        /// Player click event.
        /// </summary>
        public void MatchBegin(Ruleset ruleset)
        {
            if (OnMatchBegin != null)
            {
                OnMatchBegin(ruleset);
            }
        }

        #endregion

        #region Player event methods
        
        /// <summary>
        /// Player select event.
        /// </summary>
        public void PlayerSelect(Player player)
        {
            if (OnPlayerSelect != null)
            {
                OnPlayerSelect(player);
            }
        }

        /// <summary>
        /// Player deselect event.
        /// </summary>
        public void PlayerDeselect(Player player)
        {
            if (OnPlayerDeselect != null)
            {
                OnPlayerDeselect(player);
            }
        }

        /// <summary>
        /// Player create event.
        /// </summary>
        public void PlayerCreate(Player player)
        {
            if (OnPlayerCreate != null)
            {
                OnPlayerCreate(player);
            }
        }

        /// <summary>
        /// Player click event.
        /// </summary>
        public void PlayerClick(Player player)
        {
            if (OnPlayerClick != null)
            {
                OnPlayerClick(player);
            }
        }

        #endregion

        #region Match event methods

        /// <summary>
        /// Cell select event.
        /// </summary>
        public void CellSelect(Cell selectedCell)
        {
            if (OnCellSelect != null)
            {
                OnCellSelect(selectedCell);
            }
        }

        public void PieceSelect(Piece piece)
        {
            if (OnPieceSelect != null)
            {
                OnPieceSelect(piece);
            }
        }
        
        public void MoveSelect(Move move)
        {
            if (OnMoveSelect != null)
            {
                OnMoveSelect(move);
            }
        }
        
        public void MoveTypeRequest(MoveType moveType)
        {
            if (OnMoveTypeRequest != null)
            {
                OnMoveTypeRequest(moveType);
            }
        }

        public void PieceMove(Move move)
        {
            if (OnPieceMove != null)
            {
                OnPieceMove(move);
            }
        }

        public void PieceCapture(Move move)
        {
            if (OnPieceCapture != null)
            {
                OnPieceCapture(move);
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
