using UnityEngine;
using System.Collections;

public class bomb : MonoBehaviour {

	public Vector3 dir;
	public float speed;

	void Start() {

		Destroy (gameObject, 10f);

		Rigidbody rb = GetComponent<Rigidbody> ();
		rb.AddForce (new Vector3 (0f,0f, -0.0003f));
		rb.AddTorque(new Vector3(0f, -1.01f, 0f));
		rb.AddTorque(new Vector3(0f, 0f, 0.5f));
	}

	public void onHit(){
		manageEvents.AudioPlayer.publish ("ahaha", false, 1f);
		destroy ();
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "ground") destroy ();
	}

	void destroy(){
		manageEvents.AudioPlayer.publish ("explosionBomb", false, 1f);
		Destroy (gameObject);
	}

	void Update () {
	}
}
