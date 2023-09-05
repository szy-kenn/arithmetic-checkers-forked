using UnityEngine;

namespace Damath
{
    public class Cheats : MonoBehaviour
    {
        public Ruleset Rules { get; private set; }
        public Window PieceMenu = null;
        public Window ToolsMenu = null;
        public Cell SelectedCell;
        public bool EnableDebug = true;
        public Player WhoClicked = null;

        void Awake()
        {
            Game.Events.OnRulesetCreate += ReceiveRuleset;
            Game.Events.OnMatchBegin += Init;
            Game.Events.OnPlayerRightClick += CreateMenu;
            Game.Events.OnCellSelect += SelectCell;
        }
        
        void OnDisable()
        {
            Game.Events.OnRulesetCreate -= ReceiveRuleset;
            Game.Events.OnMatchBegin -= Init;
            Game.Events.OnPlayerRightClick -= CreateMenu;
            Game.Events.OnCellSelect -= SelectCell;
        }

        void ReceiveRuleset(Ruleset ruleset)
        {
            if (Settings.EnableDebugMode)
            {
                Game.Console.Log($"[CHEATS] Received ruleset");
            }
            Rules = ruleset;
        }
        
        public void Init()
        {
            if (Rules.EnableCheats)
            {
                Game.Console.Log($"[CHEATS]: Cheats enabled for this match.");
            }
        }

        public void Init(MatchController match)
        {
            Init();
        }

        #region Listener Functions

        public void AddPiece()
        {
            if (SelectedCell.Piece == null)
            {
                
            }
        }
        public void RemovePiece()
        {
            if (SelectedCell.Piece != null)
            {
                Debug.Log($"[CHEATS]: Removed {SelectedCell.Piece}");
                SelectedCell.Piece.Remove();
            }
        }
        public void Promote()
        {
            if (SelectedCell.Piece != null)
            {
                Debug.Log($"[CHEATS]: Promoted {SelectedCell.Piece}");
                SelectedCell.Piece.Promote();
            }
        }
        public void Demote()
        {
            if (SelectedCell.Piece != null)
            {
                Debug.Log($"[CHEATS]: Demoted {SelectedCell.Piece}");
                SelectedCell.Piece.Demote();
            }
        }
        public void RemoveAll()
        {
            // foreach (var c in Game.Main.Match.Board.cellMap)
            // {
            //     if (c.Value.piece != null) c.Value.piece.Remove();
            // }
        }
        public void PromoteAll()
        {
            // foreach (var c in Game.Main.Match.Board.cellMap)
            // {
            //     if (c.Value.piece != null) c.Value.piece.Promote();
            // }
        }
        public void DemoteAll()
        {
            // foreach (var c in Game.Main.Match.Board.cellMap)
            // {
            //     if (c.Value.piece != null) c.Value.piece.Demote();
            // }
        }
        #endregion

        public void SelectCell(Cell cell)
        {
            SelectedCell = cell;
        }

        public void CreateMenu(Player player)
        {
            if (!player.IsModerator) return;
            if (ToolsMenu != null) ToolsMenu.Close();
            if (PieceMenu != null)
            {
                Destroy(PieceMenu.gameObject);
                PieceMenu = UIHandler.Main.CreateWindow();
            } else
            {
                PieceMenu = UIHandler.Main.CreateWindow();
            }

            if (SelectedCell.Piece == null)
            {
                PieceMenu.AddChoice(AddPiece, "Add Piece", UIHandler.Main.icons[0]);
            } else
            {
                PieceMenu.AddChoice(RemovePiece, "Remove Piece", UIHandler.Main.icons[1]);

                if (!SelectedCell.Piece.IsKing)
                {
                    PieceMenu.AddChoice(Promote, "Promote", UIHandler.Main.icons[2]);
                } else
                {
                    PieceMenu.AddChoice(Demote, "Demote", UIHandler.Main.icons[3]);
                }
            }
            PieceMenu.Open(Input.mousePosition);
        }

        public void CreateToolsMenu()
        {
            if (PieceMenu != null) PieceMenu.Close();
            if (ToolsMenu != null)
            {
                Destroy(ToolsMenu.gameObject);
                ToolsMenu = UIHandler.Main.CreateWindow();
            } else
            {
                ToolsMenu = UIHandler.Main.CreateWindow();
            }

            if (EnableDebug)
            {
                ToolsMenu.AddChoice(RemoveAll, "Remove All");
                ToolsMenu.AddChoice(PromoteAll, "Promote All");
                ToolsMenu.AddChoice(DemoteAll, "Demote All");
            }
            ToolsMenu.Open(Input.mousePosition);
        }
    }
}