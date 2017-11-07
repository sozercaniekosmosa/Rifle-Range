using UnityEngine;
using System.Collections;

public class weapon : MonoBehaviour {

	public Vector3 dir;
	public float speed;

	void Start() {
		Destroy (gameObject, 1f);
	}
		
	void Update () {
		transform.position += dir*speed;
	}
}
