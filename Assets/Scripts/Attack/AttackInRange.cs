using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackInRange : MonoBehaviour {

    private GameObject _target;
    private bool _inRange = false;    
    private int _numberOfAttacks = 0;

    private Unit unitScriptTarget;
    private Unit attackingUnitScript;

    public bool OutOfAttacks{ get; private set; }

    // Use this for initialization
    void Awake () {
        OutOfAttacks = false;
    }

    // Update is called once per frame
	void Update () {
		
        if(_target == null)
        {
            FindTarget();
        }        

        if (Input.GetMouseButton(1))
        {
            //if this is the currently selected unit
            if (GameObject.ReferenceEquals(CombatPhaseManager.CurrentCombatPhaseManager.currentUnitSelected, gameObject))
            {
                if(_target != null)
                {
                    if(CheckTargetRange())
                    {
                        Attack();
                    }
                }
            }
        }

    }

    public void FindTarget()
    {
        if (_target != null)
            return;

        if(CombatPhaseManager.CurrentCombatPhaseManager.CurrentTarget != null)
        {
            _target = CombatPhaseManager.CurrentCombatPhaseManager.CurrentTarget;
            unitScriptTarget = CombatPhaseManager.CurrentCombatPhaseManager.CurrentTarget.GetComponent<Unit>();
            attackingUnitScript = CombatPhaseManager.CurrentCombatPhaseManager.currentUnitSelected.GetComponent<Unit>();
            _inRange = false; //we haven't checked if it is in range yet 
        }
    }

    private bool CheckTargetRange()
    {
        if(Vector3.Distance(_target.transform.position, CombatPhaseManager.CurrentCombatPhaseManager.currentUnitSelected.transform.position) 
            < CombatPhaseManager.CurrentCombatPhaseManager.currentUnitSelected.GetComponent<Unit>().AttackRange)
        {
            _inRange = true;
        }

        return (_inRange);
    }

    private void Attack()
    {
        if (_inRange & !OutOfAttacks)
        {
            Debug.Log(unitScriptTarget.Hitpoints + " hitpoints before attack!");
            unitScriptTarget.Hitpoints = unitScriptTarget.Hitpoints - attackingUnitScript.AttackDamage;
            Debug.Log(unitScriptTarget.Hitpoints + " hitpoints after attack!");

            _numberOfAttacks = _numberOfAttacks + 1; 

            if(_numberOfAttacks >= attackingUnitScript.AttackPoints)
            {
                OutOfAttacks = true;
            }
        }
    }

    //reset the current combatPhaseManager.CurrentTarget after an attack is complete
}
