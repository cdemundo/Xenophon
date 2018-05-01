using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayUnitInfo : Interaction {

    private bool _selected = false;
    private Unit unitScript;
    private RightClickNavigation navScript;

    public override void Select()
    {
        _selected = true;

        InfoManager.Current.SelectedUnitName.text = unitScript.name;
        InfoManager.Current.SelectedUnitHP.text = "HP: " + unitScript.Hitpoints.ToString();
        InfoManager.Current.SelectedUnitMP.text = "Movement Left: " + Mathf.Round(unitScript.MovementLeft).ToString();
        InfoManager.Current.SelectedUnitPortrait.sprite = unitScript.UnitPortrait;
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
    }
	
	// Update is called once per frame
	void Update () {

        if (!_selected)
            return;

        //if the unit can move still, update the movement points left
        if(unitScript.MovementLeft > 0)
            InfoManager.Current.SelectedUnitMP.text = "Movement Left: " + Mathf.Round(unitScript.MovementLeft).ToString();       

	}
}
