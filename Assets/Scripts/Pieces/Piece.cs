using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piecex : MonoBehaviour
{
    // Start is called before the first frame update
    int id;
    int value;
    bool isBlue = false;

    Piecex(int id, int value, bool isBlue)
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
    public void CapturePiece(int id, Piece piece) 
    {
        if (this.id == id)
        {
            //insert code here
        }
    }

    public void PromotePiece(int id)
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
