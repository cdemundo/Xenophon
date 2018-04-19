using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCradle : MonoBehaviour {

    public float Speed = 20;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(
            Input.GetAxis("Horizontal") * Speed * Time.deltaTime,
            Input.GetAxis("Vertical") * Speed * Time.deltaTime,
            0);
	}
}
