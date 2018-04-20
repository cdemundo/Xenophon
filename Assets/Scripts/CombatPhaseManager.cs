using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatPhaseManager : MonoBehaviour {

    public static CombatPhaseManager Current; 

    //need all of the different Phase classes
    private BattlefieldSetupPhase _battlefieldSetupPhase;
    private PlayerSetupPhase _playerSetupPhase;
    private AISetupPhase _aiSetupPhase;
    private AttackPhase _attackPhase;

    //Master reference to currentTarget (the unit being attacked)
    private GameObject currentTarget;
    public GameObject CurrentTarget
    {
        get
        {
            return this.currentTarget;
        }
        set
        {
            //turn off the highlight on the existing target before setting a new one
            if(currentTarget != null)
            {
                currentTarget.GetComponent<EnemyHighlight>().DisplayTarget.SetActive(false);
            }
            currentTarget = value; 
        }
        
    }
    public GameObject currentUnitSelected; //master reference to the currently selected unit (the one that is attacking)

    public Button _changeCombatPhaseButton;
    public Text roundCounter;

    private enum CurrentPhase { None, BattlefieldSetup, PlayerSetup, AISetup, Attack };
    private CurrentPhase currentPhase;

    /*
     * This function is called when the changeCombatPhase button is clicked.  It handles moving between combat phases
     */
    public void SetPhase()
    {
        //check what current phase is and set to the next phase in order:
        /* 1. Battlefield Setup
         * 2. Player Setup
         * 3. AI Setup
         * 4. Attack Phase
         */
        switch(currentPhase)
        {
            case CurrentPhase.None:
                currentPhase = CurrentPhase.BattlefieldSetup;
                _battlefieldSetupPhase = new BattlefieldSetupPhase();
                _battlefieldSetupPhase.SetupField();
                ClickCombatPhaseChangeButton();
                break;
            case CurrentPhase.BattlefieldSetup:
                currentPhase = CurrentPhase.PlayerSetup;
                SetButtonText("End Player Setup");
                //eventually player will be able to move units around and stuff prior to attacking
                //_playerSetupPhase = new PlayerSetupPhase();
                break;
            case CurrentPhase.PlayerSetup:
                currentPhase = CurrentPhase.AISetup;
                //AI setup would happen here, positioning of AI units\
                SetButtonText("Next Unit's Attack");
                ClickCombatPhaseChangeButton();
                break;
            //ATTACK PHASE
            case CurrentPhase.AISetup:
                currentPhase = CurrentPhase.Attack; //should there be a separate AI class? not sure how to handle this yet since units will switch back and forth given initiative                
                if (_attackPhase.unitTurnsRemaining)
                {
                    //AttackPhase will check all units with a turn remaining, figure out which one has the highest initiative
                    //and select it as currentUnitSelected
                    _attackPhase.StartRound();
                }
                else
                {
                    //reset all units initiative and move speed
                    _attackPhase.ResetRound();
                    roundCounter.text = _attackPhase.roundCount.ToString();
                    _attackPhase.StartRound();
                }                
                break;
            //This next part handles the transition from one unit to another and then restarts the attack phase for the next unit
            case CurrentPhase.Attack:
                //reset to AISetup so the next click will start the attack phase for a new unit
                currentPhase = CurrentPhase.AISetup;

                //reset everything for the current unit selected
                foreach (var interaction in currentUnitSelected.GetComponents<Interaction>())
                {
                    interaction.Deselect();
                }

                //remove the attack script since the currently selected unit is done for this round
                Destroy(currentUnitSelected.GetComponent<UnitAttack>());
                
                currentUnitSelected = null;

                //********************************************************************

                //Reset everything for the current target
                if (CurrentTarget != null)
                {
                    foreach (var interaction in CurrentTarget.GetComponents<Interaction>())
                    {
                        interaction.Deselect();
                    }
                    CurrentTarget = null;
                }
                //*********************************************************************

                //check how many units have left to go - if there is only one unit left to go, the next click will end the round
                if (_attackPhase.unitsRemaining == 1) 
                {
                    SetButtonText("End Round");
                }
                else
                {
                    SetButtonText("Next Unit's Attack");
                }
                //invoke the click to start the next unit's turn
                ClickCombatPhaseChangeButton(); 
                break;            
        }      
    }

    /*
     *When a unit is destroyed on the battlefield during combat, remove all references to it
     *If it was the last enemy unit destroyed, handle the end of the round
     */
    public void CleanUpDestroyedUnit(string unitID)
    {
        //remove from the library of existing units on the battlefield
        _attackPhase.RemoveUnitAfterDestroy(unitID);

        currentTarget = null;

        var aiUnitsLeft = GameObject.FindGameObjectsWithTag("AIUnit");

        if (aiUnitsLeft.Length <= 1) // this was the last unit to be destroyed!
            EndCombat();
        else
            foreach(var ai in aiUnitsLeft)
            {
                Debug.Log(ai.name);
            }
    }

    /*
     *When all AI units have been destroyed, AttackPhase will call this function
     *At some point, probably do things like calculate experience and any lasting effects - right now, just win!
     */
    public void EndCombat()
    {
        _changeCombatPhaseButton.interactable = false;

        var gameOverTextBox = GameObject.Find("GameOver");
        gameOverTextBox.GetComponent<Text>().text = "YOU WIN!!!!";
    }

    private void SetButtonText(string buttonText)
    {
        if (buttonText != null)
        {
            _changeCombatPhaseButton.GetComponentInChildren<Text>().text = buttonText;
        }
    }

    private void ClickCombatPhaseChangeButton()
    {
        _changeCombatPhaseButton.onClick.Invoke();
    }

	// Use this for initialization
	void Start () {
        currentPhase = CurrentPhase.None;
        _attackPhase = new AttackPhase();
        currentUnitSelected = null;

        Current = this;
    }
	
	// Update is called once per frame
	void Update () {
		
        
	}
}
