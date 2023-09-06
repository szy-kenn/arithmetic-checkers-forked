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
            Game.Events.OnPlayerRightClick += CheckPlayer;
            Game.Events.OnCellDeselect += HideMenus;
        }
        
        void OnDisable()
        {
            Game.Events.OnRulesetCreate -= ReceiveRuleset;
            Game.Events.OnMatchBegin -= Init;
            Game.Events.OnPlayerRightClick -= CheckPlayer;
            Game.Events.OnCellDeselect -= HideMenus;
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

        public void CheckPlayer(Player player)
        {
            if (!player.IsModerator) return;

            SelectedCell = player.SelectedCell;
            CreateMenu();
        }

        public void HideMenus(Cell cell)
        {
            PieceMenu?.Close();
            ToolsMenu?.Close();;
        }

        public void CreateMenu()
        {
            if (ToolsMenu != null) ToolsMenu.Close();
            if (PieceMenu != null)
            {
                Destroy(PieceMenu.gameObject);
                PieceMenu = Game.UI.CreateWindow();
            } else
            {
                PieceMenu = Game.UI.CreateWindow();
            }

            if (SelectedCell.Piece == null)
            {
                PieceMenu.AddChoice(AddPiece, "Add Piece", Game.UI.icons[0]);
            } else
            {
                PieceMenu.AddChoice(RemovePiece, "Remove Piece", Game.UI.icons[1]);
                PieceMenu.AddChoice(RemovePiece, "Capture Piece", Game.UI.icons[1]);

                if (!SelectedCell.Piece.IsKing)
                {
                    PieceMenu.AddChoice(Promote, "Promote", Game.UI.icons[2]);
                } else
                {
                    PieceMenu.AddChoice(Demote, "Demote", Game.UI.icons[3]);
                }
            }
            PieceMenu.Open(Input.mousePosition);
            PieceMenu.gameObject.transform.position.z.Equals(-1f);
        }

        public void CreateToolsMenu()
        {
            if (PieceMenu != null) PieceMenu.Close();
            if (ToolsMenu != null)
            {
                Destroy(ToolsMenu.gameObject);
                ToolsMenu = Game.UI.CreateWindow();
            } else
            {
                ToolsMenu = Game.UI.CreateWindow();
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