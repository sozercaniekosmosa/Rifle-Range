using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gears : MonoBehaviour {

	public bool CCW;
	public float deltaAng;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (new Vector3(0f,0f,deltaAng*(CCW?1f:-1f)));
	}
}
