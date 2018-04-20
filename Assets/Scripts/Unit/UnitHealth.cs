using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealth : MonoBehaviour {

    private float maxHitpoints { get; set; }
    public float CurrentHitpoints {get; set;}
    public Animator animator;

    public void IncreaseHitpoints(float amountToIncrease)
    {
        if ((CurrentHitpoints + amountToIncrease) < maxHitpoints)
            CurrentHitpoints = CurrentHitpoints + amountToIncrease;
        else
            CurrentHitpoints = maxHitpoints;
    }

    public void DecreaseHitpoints(float amountToDecrease)
    {
        if (CurrentHitpoints - amountToDecrease > 0)
        {
            CurrentHitpoints = CurrentHitpoints - amountToDecrease;
            animator.SetTrigger("damage");
        }
        else
        {
            CurrentHitpoints = 0;
            gameObject.GetComponent<UnitDeath>().DestroyUnit();
        }
    }

	// Use this for initialization
	void Start () {

        CurrentHitpoints = GetComponentInParent<Unit>().MaxHitpoints;

        // any object that is attacking should have animator attached
        animator = gameObject.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
