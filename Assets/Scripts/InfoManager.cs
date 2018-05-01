using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoManager : MonoBehaviour {

    public static InfoManager Current;

    //display information about the currently selected unit
    public Text SelectedUnitName;
    public Text SelectedUnitHP;
    public Text SelectedUnitMP;
    public Image SelectedUnitPortrait;

    //info about currently selected target
    public Text TargetUnitName;
    public Text TargetUnitHP;
    public Image TargetUnitPortrait;

    public Text GameOver;
    public Text RoundCount;

	// Use this for initialization
	void Start () {
        Current = this;

        SelectedUnitPortrait.enabled = false;
        TargetUnitPortrait.enabled = false;
	}
}
