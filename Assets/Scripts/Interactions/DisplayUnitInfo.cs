using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayUnitInfo : Interaction {

    private bool _selected = false;
    private Unit unitScript;
    private RightClickNavigation navScript;

    [SerializeField] private Text unitNameTextBox;
    [SerializeField] private Text hitpointsTextBox;
    [SerializeField] private Text movementPointsTextBox;

    public override void Select()
    {
        _selected = true;

        unitNameTextBox.text = unitScript.Player + "-" + unitScript.name;
        hitpointsTextBox.text = unitScript.Hitpoints.ToString();
        movementPointsTextBox.text = "Movement Left: " + Mathf.Round(unitScript.MovementLeft).ToString();
    }

    public override void Deselect()
    {
        _selected = false;
    }

	// Use this for initialization
	void Start () {
        //get info about the unit i'm attached to
        unitScript = GetComponentInParent<Unit>();
        navScript = GetComponentInParent<RightClickNavigation>();

        //find the textboxes that need to be updated
        unitNameTextBox = GameObject.Find("UnitName").GetComponent<Text>();
        hitpointsTextBox = GameObject.Find("Hitpoints").GetComponent<Text>();
        movementPointsTextBox = GameObject.Find("MovementPoints").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {

        if (!_selected)
            return;

        movementPointsTextBox.text = "Movement Left: " + Mathf.Round(unitScript.MovementLeft).ToString();

        

	}
}
