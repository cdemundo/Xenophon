using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttack : MonoBehaviour {

    private GameObject target;
    private bool _inRange = false;    
    private int _numberOfAttacks = 0;

    private UnitHealth unitHealthScript;
    private Unit attackingUnitScript;

    public bool OutOfAttacks{ get; private set; }
    public Animator animator;

    // Use this for initialization
    void Awake () {
        OutOfAttacks = false;

        //any object that is attacking should have an attack animation with an animator attached
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
	void Update () {
		
        if(target == null)
        {
            FindTarget();
        }        

        if (Input.GetMouseButton(1))
        {
            if(Manager.Current.ScreenPointToUnit(Input.mousePosition))
            { 
                if(target != null)
                {
                    if(CheckTargetRange() && !OutOfAttacks)
                    {
                        Attack();
                    }
                }
            }
        }
    }

    public void FindTarget()
    {
        if (target != null)
            return;

        if(CombatPhaseManager.Current.CurrentTarget != null)
        {
            target = CombatPhaseManager.Current.CurrentTarget;
            unitHealthScript = CombatPhaseManager.Current.CurrentTarget.GetComponent<UnitHealth>();
            attackingUnitScript = CombatPhaseManager.Current.currentUnitSelected.GetComponent<Unit>();
            _inRange = false; //we haven't checked if it is in range yet 
        }
    }

    private bool CheckTargetRange()
    {
        if(Vector3.Distance(target.transform.position, CombatPhaseManager.Current.currentUnitSelected.transform.position) 
            < CombatPhaseManager.Current.currentUnitSelected.GetComponent<Unit>().AttackRange)
        {
            _inRange = true;
        }

        return (_inRange);
    }

    private void Attack()
    {
        if (_inRange)
        {
            animator.SetTrigger("attack");

            Debug.Log(unitHealthScript.CurrentHitpoints + " hitpoints before attack!");
            unitHealthScript.DecreaseHitpoints(attackingUnitScript.AttackDamage);
            Debug.Log(unitHealthScript.CurrentHitpoints + " hitpoints after attack!");

            _numberOfAttacks = _numberOfAttacks + 1;

            if(_numberOfAttacks >= attackingUnitScript.AttackPoints)
            {
                OutOfAttacks = true;
            }
        }
    }


}
