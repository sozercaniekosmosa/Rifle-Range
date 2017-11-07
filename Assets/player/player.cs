using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class player : MonoBehaviour {
	
	public uint life = 10;
	public uint ammoCount = 100;
	public float speedShot = 0.5f;
	public float crossSize;
	public Texture2D crosshair;
	public Camera cam;
	public GameObject weapon;
	public float volume;

	AudioSource audioSource;
	float speedShotStrart;
	Transform gun;
	Transform support;
	Quaternion QX90 = new Quaternion(1f, 0f, 0f, 1f);
	float speedRotMG = 0f;
	Vector3 pos;
	bool isBackForce = false;

	void Start() {

		audioSource = GetComponent<AudioSource> ();

		speedShotStrart = speedShot;
		gun = transform.Find ("machinegun").transform;
		pos = gun.transform.position;
		support = gun.Find ("support").transform;
		Invoke("eventsInit", 0.1f);
	}

	void eventsInit(){
		manageEvents.UpdateGUI.publish("textSpeedShot", speedShotStrart/speedShot);
		manageEvents.UpdateGUI.publish("textAmmo", ammoCount);
		manageEvents.UpdateGUI.publish("textPlayerLife", life);
		manageEvents.PlayerHit.subscribe((object obj)=>{
			manageEvents.AudioPlayer.publish ("hitPain", false, volume);
			life--;
			manageEvents.UpdateGUI.publish("textPlayerLife", life);
		});

		manageEvents.GameState.subscribe((object obj)=>{

			if(obj.ToString() == "bonusLife"){
				life++;
				if(life > 10) life = 10;
				manageEvents.UpdateGUI.publish("textPlayerLife", life);
			}

			if(obj.ToString() == "bonusAmmo"){
				ammoCount+=50;
				if(ammoCount > 999) ammoCount = 999;
				manageEvents.UpdateGUI.publish("textAmmo", ammoCount);
			}

			if(obj.ToString() == "bonusSpeedShot"){
				speedShot /= 2;
				manageEvents.UpdateGUI.publish("textSpeedShot", speedShotStrart/speedShot);
				Invoke("resetSpeedShot", 5f);
			}
		});
	}

	void resetSpeedShot(){
		speedShot *= 2;
		manageEvents.UpdateGUI.publish("textSpeedShot", speedShotStrart/speedShot);
	}
		
	void LateUpdate () {
		
//		RaycastHit[] hits;
		RaycastHit hit;
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);

		gun.rotation = Quaternion.FromToRotation (new Vector3 (0f, 0f, 1f), ray.direction);

		gun.rotation *= QX90;

		Debug.DrawRay(ray.origin, ray.direction*50, Color.yellow);

		if (Input.GetMouseButton (0)) {
			if (utils.isTimeout (0, speedShot)) {
				
				if (speedRotMG <= 90f) speedRotMG += 50f;
			
				if ((ammoCount <= 0)) manageEvents.AudioPlayer.publish ("emptyAmmo", false, volume);

				if (ammoCount > 0) {

					if (Physics.Raycast (ray, out hit, 1000f)) {
						if (hit.transform.gameObject.tag == "enemy") hit.transform.gameObject.GetComponent<enemy> ().onHit ();
						if (hit.transform.gameObject.tag.IndexOf ("bonus") != -1) hit.transform.gameObject.GetComponent<CBouns> ().onHit ();
						if (hit.transform.gameObject.tag == "bomb") hit.transform.gameObject.GetComponent<bomb> ().onHit ();
					}

					GameObject go = (GameObject)Instantiate (weapon, cam.transform.position, Quaternion.FromToRotation (new Vector3 (0f, 0f, 1f), ray.direction));
					go.GetComponent<weapon> ().dir = ray.direction;

					manageEvents.AudioPlayer.publish ("machinegun", false, volume);

					gun.position = new Vector3(pos.x, pos.y, pos.z+ (isBackForce?0.02f:0f) );
					isBackForce = !isBackForce;

					ammoCount--;
					manageEvents.UpdateGUI.publish ("textAmmo", ammoCount);
				}
			}
		}else {
			if (speedRotMG > 0f)
				speedRotMG -= 1f;
			if (speedRotMG < 0f)
				speedRotMG = 0f;
			
			gun.position = pos;
		}

		utils.log(speedRotMG);
		support.Rotate(new Vector3(0f,speedRotMG,0f));
	}

	void OnGUI(){
		Cursor.visible = false;

		float crossX = Input.mousePosition.x - crosshair.width / 2 * crossSize;
		float crossY = -Input.mousePosition.y + Screen.height - crosshair.width / 2 * crossSize;
		float crossW = crosshair.width * crossSize;
		float crossH = crosshair.height * crossSize;

		GUI.DrawTexture (new Rect (crossX, crossY, crossW, crossH), crosshair);
	}

	void OnDestroy(){
		Cursor.visible = true;
	}
}
