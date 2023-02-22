using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChipRenderer : MonoBehaviour
{
    public string value;
    TextMeshPro textMeshPro;

    // Start is called before the first frame update
    void Start()
    {
        textMeshPro = GetComponentInChildren<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGUI()
    {
        if (GUI.changed)
        {
            textMeshPro.text = value;
        }
    }
}
