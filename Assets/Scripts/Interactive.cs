using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This class is designed to manage all interactions with a unit. 
 * Scripts of class "Interaction" should be attached to units along with this script
 * 
 */
public class Interactive : MonoBehaviour
{
    private bool _Selected = false;
    public bool Selected { get { return _Selected; } }

    public void Select()
    {
        _Selected = true;
        foreach (var selection in GetComponents<Interaction>())
        {
            selection.Select();
        }
    }

    public void Deselect()
    {
        _Selected = false;
        foreach (var selection in GetComponents<Interaction>())
        {
            selection.Deselect();
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
