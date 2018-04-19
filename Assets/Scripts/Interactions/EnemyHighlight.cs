using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHighlight : MonoBehaviour {

    public GameObject DisplayTarget;
    [SerializeField] private bool isAI;

    // Use this for initialization
    void Start ()
    {
        DisplayTarget.SetActive(false);

        //check if the parent object is AI upon start
        if(GetComponentInParent<Unit>().AIControlled)
        {
            isAI = true;
        }
	}

    private void OnMouseOver()
    {
        if(isAI)
        {
            if (!CombatPhaseManager.CurrentCombatPhaseManager.currentUnitSelected.GetComponent<AttackInRange>().OutOfAttacks)
            {
                DisplayTarget.SetActive(true);

                if (Input.GetMouseButtonDown(1))
                {
                    CombatPhaseManager.CurrentCombatPhaseManager.CurrentTarget = transform.gameObject;
                }
            }
        }
    }

    private void OnMouseExit()
    {
        if (CombatPhaseManager.CurrentCombatPhaseManager.CurrentTarget == null)
            DisplayTarget.SetActive(false);
        else
        {
            if (!GameObject.ReferenceEquals(CombatPhaseManager.CurrentCombatPhaseManager.CurrentTarget, transform.gameObject))
                DisplayTarget.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update () {

        if(!(CombatPhaseManager.CurrentCombatPhaseManager.CurrentTarget == null))
        {
            if (!GameObject.ReferenceEquals(CombatPhaseManager.CurrentCombatPhaseManager.CurrentTarget, transform.gameObject))
                DisplayTarget.SetActive(false);
        }
        else
        {
            return;
        }


	}
}
