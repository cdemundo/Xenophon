using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattlefieldSetupPhase {

    public void SetupField()
    {
        //for each player in the game, find all the prefabs added to StartingUnits and instantiate
        foreach (var p in Manager.Current.Players)
        {
            //we want to set the units up in relation to each other so they're nicely arrayed on the field
            Vector3 lastUnitSpawnLocation = Vector3.zero;

            //at some point, need a better way to determine what the starting units are!
            foreach (var u in p.StartingUnits)
            {
                GameObject unitToSpawn;
                //if it equals null, just instantiate it at the player start point
                if (lastUnitSpawnLocation == Vector3.zero)
                {
                    unitToSpawn = GameObject.Instantiate(u, p.Location.position, p.Location.rotation);
                    lastUnitSpawnLocation = unitToSpawn.transform.position;
                }
                else //others figure out a spawn location relative to the most recently spawned unit
                {
                    unitToSpawn = GameObject.Instantiate(u, SpawnLocation(lastUnitSpawnLocation), p.Location.rotation);
                    lastUnitSpawnLocation = unitToSpawn.transform.position;
                }

                var player = unitToSpawn.AddComponent<Player>(); //add player script to every unit to keep track of what player owns it
                Unit unitScript = unitToSpawn.GetComponent<Unit>(); //be able to reference and set information about the unit
                player.Info = p; //set to current PlayerSetupDefinition class

                if (!p.IsAI)
                {
                    if (Player.Default == null)
                    {
                        Player.Default = p;
                    }
                    unitToSpawn.AddComponent<RightClickNavigation>(); //user needs to be able to move the unit!                   
                }
                else //if it is AI
                {
                    unitScript.AIControlled = true;
                    unitToSpawn.tag = "AIUnit";
                }

                //set the player name for the unit so we know who owns it
                unitToSpawn.GetComponent<Unit>().AIControlled = p.IsAI;
                unitToSpawn.GetComponent<Unit>().Player = p.Name;

                SetUnitInformation(unitScript, p.IsAI);
            }
        }
    }

    /*
     *Setup attributes of individual units as they are spawned
     * Randomly picking attributes right now
     */
    public void SetUnitInformation(Unit unitScript, bool isAI)
    {
        //there will actually be a way to determine initiative of a unit, but just to fuck around right now
        //as we spawn the units, set their hitpoints and initiative
        if (!isAI) //if it is a human player
        {
            //adding a bit of randomness to the initiative so no unit has the exact same initiative
            unitScript.Initiative = 10 + Random.Range(0.0f, 1.0f); //all units have a script with base class "Unit" attached - find the script and set the initiative property
            unitScript.Hitpoints = 50;
            unitScript.MaxDistance = 25;
            unitScript.AttackRange = 50;
            unitScript.AttackDamage = 10;
            unitScript.AttackPoints = 1;
        }
        else //ai gets bitch ass units!
        {
            unitScript.Initiative = 5 + Random.Range(0.0f, 1.0f);
            unitScript.Hitpoints = 50;
            unitScript.MaxDistance = 25;
        }
    }

    /*
     *Right now this just places units neatly next to each other (along the x axis) when they are spawned
     * Eventually we will include things like - do melee units go in front? 
     * Do we examine the terrain and put archers on recommended high ground?
     * 
     * referenceUnit is just the first unit spawned right now, but probably do something more in depth later
     */
    public Vector3 SpawnLocation(Vector3 referenceUnitVector3)
    {
        Vector3 spawnPosition = new Vector3(referenceUnitVector3.x + 5, referenceUnitVector3.y, referenceUnitVector3.z);

        //check if there is a unit there already
        if(!Manager.Current.ScreenPointToUnit(new Vector2(spawnPosition.x, spawnPosition.y)))
        {
            //!!!!!!!!!!!!!! need to also check suitability of terrain !!!!!!!!!!!!!!!!!
            //!!!!!!!!!!!!!! this will place units all the way to infinity
            return spawnPosition;
        }
        else
        {
            return SpawnLocation(new Vector3(referenceUnitVector3.x + 5, referenceUnitVector3.y, referenceUnitVector3.z));
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
