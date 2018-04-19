using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackPhase {

    private Dictionary<float, GameObject> unitsOnField;

    //!!!!!!!!!!! Should this be public?? accessed from CombatPhaseManager
    public bool unitTurnsRemaining = true;
    public int unitsRemaining = 0;
    public bool roundStarted = false;
    public int roundCount = 1;    
    
    /*
     * When the attack phase first starts, find all the units on the battlefield and add them to a dictionary.  
     * Reset the movement points for all the units on the field for the new attack
     */
    public void StartRound()
    {
        if (!roundStarted)
        {
            unitsOnField = new Dictionary<float, GameObject>();

            //find all units on the battlefield
            List<GameObject> units = new List<GameObject>(GameObject.FindGameObjectsWithTag("Unit")); //player units
            var aiUnits = GameObject.FindGameObjectsWithTag("AIUnit");

            foreach(var aiUnit in aiUnits)
            {
                units.Add(aiUnit);
            }            

            if (units != null)
            {
                foreach (var unit in units)
                {
                    Unit unitScript = unit.GetComponent<Unit>();
                    unitsOnField.Add(unitScript.Initiative, unit);                    

                    RightClickNavigation navScript = unit.GetComponent<RightClickNavigation>();
                    if (navScript != null)
                    {
                        unitScript.MovementLeft = unitScript.MaxDistance;                        
                    }
                }
            }
            else
            {
                Debug.Log("No characters with tag 'Unit' were found in scene!");
            }

            roundStarted = true;            
        }
        GetHighestInitiative();
    }

    /*
     * Look through unitsOnField and find unit with highest initiative
     * Store it in currentUnitSelected, remove it from unitsOnField and start the attack!
     */
    public void GetHighestInitiative()
    {
        if (unitsOnField.Count > 0)
        {
            Debug.Log(unitsOnField.Count);
            //find unit with highest initiative from those on the field
            CombatPhaseManager.CurrentCombatPhaseManager.currentUnitSelected = unitsOnField[unitsOnField.Max(x => x.Key)];

            //remove that unit from the dictionary so we don't find it the next time around
            unitsOnField.Remove(unitsOnField.Max(x => x.Key));

            if(unitsOnField.Count == 0)
            {
                unitTurnsRemaining = false;
            }

            unitsRemaining = unitsOnField.Count;
            StartAttack();
        }
        else
        {
            Debug.Log("Trying to find highest initiative unit, but all units have been removed from unitsOnField dictionary!");
        }        
    }

    /*
     * Determine if the unit is AI or Player and run the appropriate function to start the attack
     */
    public void StartAttack()
    {
        //if the highest initiative is an AI unit - need to do AI things
        if (CombatPhaseManager.CurrentCombatPhaseManager.currentUnitSelected.GetComponent<Unit>().AIControlled)
        {
            //run AI function to let AI have it's turn
        }
        else
        {
            //start the player attack turn
            SelectUnit();
        }
    }

    public void SelectUnit()
    {
        //actually select the starting unit for the player
        CombatPhaseManager.CurrentCombatPhaseManager.currentUnitSelected.GetComponent<Interactive>().Select();

        //add AttackScript so the unit can attack
        AttackInRange attackScript = CombatPhaseManager.CurrentCombatPhaseManager.currentUnitSelected.AddComponent<AttackInRange>();
        //pass off to AttackInRange to wait and listen for an attack
        attackScript.FindTarget();
    }

    private void Update()
    {

    }

    private void Start()
    {

    }

    /*
     * Triggered when no units are left in unitsOnField
     * All units have attacked - need to run any "end of turn" things and start over
     */
    public void ResetRound()
    {
        roundCount = roundCount + 1;
        unitTurnsRemaining = true;
        roundStarted = false;
    }
}
