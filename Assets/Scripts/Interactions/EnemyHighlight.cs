using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHighlight : MonoBehaviour {

    public GameObject DisplayTarget;

    // Use this for initialization
    void Start ()
    {
        DisplayTarget = gameObject.transform.Find("Targeted_Quad").gameObject;
        DisplayTarget.SetActive(false);
	}

    private void OnMouseOver()
    {
        //if the user is mousing over a target in between rounds, avoid returning a nullreferenceexception
        if (CombatPhaseManager.Current.currentUnitSelected == null)
            return; 

        if (!CombatPhaseManager.Current.currentUnitSelected.GetComponent<UnitAttack>().OutOfAttacks)
        {
            DisplayTarget.SetActive(true);

            if (Input.GetMouseButtonDown(1))
            {
                //if there is a target already, turn the display icon off
                if (CombatPhaseManager.Current.CurrentTarget != null)
                    CombatPhaseManager.Current.CurrentTarget.GetComponent<EnemyHighlight>().DisplayTarget.SetActive(false);
                //set this unit as the new target
                CombatPhaseManager.Current.CurrentTarget = transform.gameObject;

                //display information about the target
                InfoManager.Current.TargetUnitName.text = transform.gameObject.name;
                InfoManager.Current.TargetUnitHP.text = transform.gameObject.GetComponent<Unit>().Hitpoints.ToString();
                InfoManager.Current.TargetUnitPortrait.sprite = transform.gameObject.GetComponent<Unit>().UnitPortrait;
                InfoManager.Current.TargetUnitPortrait.enabled = true;
            }
        }
    }

    private void OnMouseExit()
    {
        if (CombatPhaseManager.Current.CurrentTarget == null)
            DisplayTarget.SetActive(false);
        else
        {
            if (!GameObject.ReferenceEquals(CombatPhaseManager.Current.CurrentTarget, transform.gameObject))
                DisplayTarget.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update ()
    {
	}
}
