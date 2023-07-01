using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string playerName = "Player";
    public Side side;
    public int pieceCount = 0;
    public float score = 0f;
    public bool IsModerator = false;
    public bool IsPlaying = false;
    public Cell selectedCell = null;
    
    [SerializeField] GameObject _playerPrefab;

    void Start()
    {
        this.name = $"Player ({playerName})";
    }

    void Update()
    {
        DetectRaycast();
    }

    public Player Create(Side side, string playerName=null)
    {
        var newPlayer = Instantiate(_playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        newPlayer.transform.SetParent(transform);
        Player c_player = newPlayer.GetComponent<Player>();

        c_player.side = side;
        if (name != null) c_player.playerName = name;

        return c_player;
    }

    void DetectRaycast()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider == null) return;

            Click(hit);
        }

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider == null) return;
            if (!IsModerator)
            {
                Click(hit);
                return;
            }

            switch (hit.collider.tag)
            {
                case "Cell":
                    selectedCell = hit.collider.gameObject.GetComponent<Cell>();

                    if (Game.Main.CheatsEnabled)
                    {
                        Game.Main.Cheats.Select(selectedCell); 
                        Game.Main.Cheats.CreatePieceMenu();
                    }
                    break;

                case "Background":
                    Game.Main.Cheats.CreateToolsMenu(); 
                    break;

                default:
                    break;
            }
        }
    }

    public void Click(RaycastHit2D hit)
    {
        if (hit.collider.tag == "Cell")
        {
            selectedCell = hit.collider.gameObject.GetComponent<Cell>();

            Game.Main.Select(this, selectedCell);
        } else
        {
            Game.Main.Refresh();
        }
        //add more else-if statements to add more components that can be detected by the Raycast
        //Make sure to include every game objects with tags to avoid getting not detected
        return;
    }
}
