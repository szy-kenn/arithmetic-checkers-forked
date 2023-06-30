
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Cheats : MonoBehaviour
{
    public Window pieceMenu;
    public Window toolsMenu;
    public Cell selectedCell;
    public bool EnableDebug = true;

    #region Listener Functions

    public void AddPiece()
    {
        if (selectedCell.piece == null)
        {
            
        }
    }
    public void RemovePiece()
    {
        if (selectedCell.piece != null)
        {
            Debug.Log($"[CHEATS]: Removed {selectedCell.piece}");
            selectedCell.piece.Remove();
        }
    }
    public void Promote()
    {
        if (selectedCell.piece != null)
        {
            Debug.Log($"[CHEATS]: Promoted {selectedCell.piece}");
            selectedCell.piece.Promote();
        }
    }
    public void Demote()
    {
        if (selectedCell.piece != null)
        {
            Debug.Log($"[CHEATS]: Demoted {selectedCell.piece}");
            selectedCell.piece.Demote();
        }
    }
    public void RemoveAll()
    {
        foreach (var c in Game.Main.Board.cellMap)
        {
            if (c.Value.piece != null) c.Value.piece.Remove();
        }
    }
    public void PromoteAll()
    {
        foreach (var c in Game.Main.Board.cellMap)
        {
            if (c.Value.piece != null) c.Value.piece.Promote();
        }
    }
    public void DemoteAll()
    {
        foreach (var c in Game.Main.Board.cellMap)
        {
            if (c.Value.piece != null) c.Value.piece.Demote();
        }
    }
    #endregion

    public void Init()
    {        
        Debug.Log($"[CHEATS]: Enabled cheats");
    }

    public void Select(Cell cell)
    {
        selectedCell = cell;
    }

    public void CreatePieceMenu()
    {
        if (toolsMenu != null) toolsMenu.Close();
        if (pieceMenu != null) Destroy(pieceMenu.gameObject);
        pieceMenu = UIHandler.Main.CreateWindow();

        if (selectedCell.piece == null)
        {
            pieceMenu.AddChoice(AddPiece, "Add Piece", UIHandler.Main.icons[0]);
        } else
        {
            pieceMenu.AddChoice(RemovePiece, "Remove Piece", UIHandler.Main.icons[1]);

            if (!selectedCell.piece.IsKing)
            {
                pieceMenu.AddChoice(Promote, "Promote", UIHandler.Main.icons[2]);
            } else
            {
                pieceMenu.AddChoice(Demote, "Demote", UIHandler.Main.icons[3]);
            }
        }
        pieceMenu.Open(Input.mousePosition);
    }

    public void CreateToolsMenu()
    {
        if (pieceMenu != null) pieceMenu.Close();
        if (toolsMenu != null) Destroy(toolsMenu.gameObject);
        toolsMenu = UIHandler.Main.CreateWindow();

        if (EnableDebug)
        {
            toolsMenu.AddChoice(RemoveAll, "Remove All");
            toolsMenu.AddChoice(PromoteAll, "Promote All");
            toolsMenu.AddChoice(DemoteAll, "Demote All");
        }
        toolsMenu.Open(Input.mousePosition);
    }
}
