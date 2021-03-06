﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Unit : MonoBehaviour {

    protected string player; //what player controls this unit
    protected bool aicontrolled;
    protected float maxDistance; //how far can the unit travel in one turn
    protected float movementLeft; //how many movement points are left for this turn
    protected float movementSpeed; //how fast does the unit move

    public float Hitpoints { get; set; }
    public float Initiative { get; set; }
    public string Player { get; set; }
    public bool AIControlled { get; set; }
    public float MaxDistance { get; set; }
    public float MovementLeft { get; set; }
    public float MovementSpeed { get; set; }

    public float AttackRange { get; set; }
    public float AttackDamage { get; set; }
    public float AttackPoints { get; set; } //how many attacks a turn can this unit do

    public float MaxHitpoints;
    public string ID;
    public Sprite UnitPortrait;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    
	}
}
