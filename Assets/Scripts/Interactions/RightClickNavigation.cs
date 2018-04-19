using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RightClickNavigation : Interaction {
    //other classes this class needs access to
    private NavMeshAgent agent;
    private Unit unitScript;

    private float relaxDistance = 1;
    private Vector3 target = Vector3.zero; //where the unit is moving to
    private bool selected = false; //is the unit selected - is it it's turn to attack?    
    
    private bool isActive = false;
    public bool IsActive { get; private set; }

    private Vector3 startPosition = Vector3.zero;
    private float distanceTraveledInRound = 0;

    /**********************METHODS**********************************/

    public override void Deselect()
    {
        selected = false;
        //reset movement calculators
        distanceTraveledInRound = 0;
        startPosition = Vector3.zero;
    }

    public override void Select() //method is called when the unit is selected in each round
    {
        selected = true;
    }

    public void SendToTarget()
    {
        agent.SetDestination(target);
        agent.isStopped = false;
        isActive = true;      
    }

    // Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 5; //!!!!!!!!!!!!!!!!! need a real way to determine speed

        unitScript = GetComponentInParent<Unit>(); //get the unit info script for parent i'm attached to        
    }

    // Update is called once per frame
    void Update()
    {
        if (unitScript.MaxDistance - distanceTraveledInRound <= 0)
            return;

        //First, see if there is a valid right click and the unit is selected
        //then move the unit
        if (selected && Input.GetMouseButtonDown(1))
        {
            Debug.Log("registered click!");
            //if user right clicked a unit, return
            if (Manager.Current.ScreenPointToUnit(Input.mousePosition))
                return;

            var tempTarget = Manager.Current.ScreenPointToMapPosition(Input.mousePosition);

            if (tempTarget.HasValue)
            {
                RaycastHit hit = (RaycastHit)tempTarget;

                target = hit.point;

                //lets move the unit
                SendToTarget();
            }
        }
        
        //if the unit reaches it's destination from the click, then stop it from moving
        if (isActive && Vector3.Distance(target, transform.position) <= relaxDistance)
        {
            agent.isStopped = true;
            isActive = false;
        }

        //check how far the unit has moved since last frame - if it's exceeding MaxDistance for unit, stop movement
        if (isActive)
        {
            if (startPosition == Vector3.zero) //if its the first movement of the round
            {
                startPosition = transform.position;
            }
            else
            {
                //calculate distance moved in the last frame and add it to the total movement in the round
                distanceTraveledInRound = distanceTraveledInRound + Vector3.Distance(startPosition, transform.position);
                //update startPosition for next frame
                startPosition = transform.position;
                //if we've moved more than the units max distance, stop the unit
                if ((unitScript.MaxDistance - distanceTraveledInRound) <= 0)
                {
                    agent.isStopped = true;
                    isActive = false;
                }
            }

            


            //movementLeft = MaxDistance - distance traveled from original position
            unitScript.MovementLeft = unitScript.MaxDistance-distanceTraveledInRound;
        }

        //need some sort of feature - if user presses and holds right click, show the potential distance that can be moved
        /*if (selected && Input.GetMouseButton(1))
        {

        }*/

    }
}
