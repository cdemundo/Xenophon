using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDeath : MonoBehaviour {

	public void DestroyUnit()
    {
        string unitID = gameObject.GetComponent<Unit>().ID;
        Destroy(gameObject, 0f);

        //clean up references to this unit before destroying it
        CombatPhaseManager.Current.CleanUpDestroyedUnit(unitID);
    }
}
