using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetect : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        detectObjectWithRaycast();
    }

    void detectObjectWithRaycast()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            Debug.Log(hit.collider.gameObject.name);
            if (hit.collider != null)
            {
                if (hit.collider.tag == "Cell")
                {
                    Cell selectedCell = hit.collider.gameObject.GetComponent<Cell>();
                    selectedCell.SelectMe();
                }
                //add more else-if statements to add more components that can be detected by the Raycast
                //Make sure to include every game objects with tags to avoid getting not detected
            }
        }
    }
}