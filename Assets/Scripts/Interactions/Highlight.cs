using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : Interaction
{
    public GameObject DisplayItem;

    public override void Deselect()
    {
        DisplayItem.SetActive(false);
    }

    public override void Select()
    {
        DisplayItem.SetActive(true);
    }

    // Use this for initialization
    void Start()
    {
        DisplayItem.SetActive(false);
    }
}
