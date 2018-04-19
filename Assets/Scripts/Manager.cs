using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour {

    public static Manager Current = null;

    public List<PlayerSetupDefinition> Players = new List<PlayerSetupDefinition>();

    public TerrainCollider MapCollider;

    public Button changePhaseButton;

    /*
     * Identify a point on the map from a 2d point on the screen
     */
    public RaycastHit? ScreenPointToMapPosition(Vector2 point)
    {
        var ray = Camera.main.ScreenPointToRay(point);
        RaycastHit hit;

        //if we have not hit anything, then just return null
        if (!MapCollider.Raycast(ray, out hit, Mathf.Infinity))
            return null;

        return hit;
    }

    /*
     * Check if a unit was clicked and return true if so
     */
    public bool ScreenPointToUnit(Vector2 point)
    {
        var ray = Camera.main.ScreenPointToRay(point);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit))
            return false; //we didnt hit anything
        else
        {
            //If the tag does not equal Untagged, we clicked a unit, so return the unit
            if (!(hit.transform.tag == "Untagged"))
                return true;
            else
                return false;
        }
    }

    // Use this for initialization
    void Start () {
        Current = this;
        changePhaseButton.GetComponentInChildren<Text>().text = "Start Combat!";
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
