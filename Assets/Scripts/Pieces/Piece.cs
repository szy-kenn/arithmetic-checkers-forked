using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    // Start is called before the first frame update
    int id;
    int value;
    bool isBlue = false;

    Piece(int id, int value, bool isBlue)
    {
        this.id = id;
        this.value = value;
        this.isBlue = isBlue;
    }

    //Function to move the piece
    public void movePiece(int id)
    {
        if(this.id == id)
        {
            //insert code here
        }
    }

    //Captures the pieces
    public void capturePiece(int id, Piece piece) 
    {
        if (this.id == id)
        {
            //insert code here
        }
    }

    public void promotePiece(int id)
    {
        if (this.id == id)
        {
            //insert code here
        }
    }

    public void removeMe(int id)
    {
        if (this.id == id)
        {
            //insert code here
        }
    }


}
