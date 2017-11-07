using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBouns : MonoBehaviour {

	public float speedX;
	public float speedY;
	public float rangeX;
	public float ang;

	[System.Serializable]
	public struct TexutereBonus
	{
		public Texture2D bonusLife;
		public Texture2D bonusTime;
		public Texture2D bonusAmmo;
		public Texture2D bonusSpeedShot;
	}

	public TexutereBonus textures;
	Material matFront, matBack;
	Dictionary<string, Texture2D> dictTxBonus;

	void Start () {

		matFront = utils.getMaterial (gameObject, "prfFront");
		matBack = utils.getMaterial (gameObject, "prfBack");
		Destroy (gameObject, 10f);
		Rigidbody rb = GetComponent<Rigidbody> ();

		dictTxBonus = new Dictionary<string, Texture2D>
		{
			{ "bonusLife", textures.bonusLife},
			{ "bonusTime", textures.bonusTime},
			{ "bonusAmmo", textures.bonusAmmo},
			{ "bonusSpeedShot", textures.bonusSpeedShot}
		};

		matFront.mainTexture = dictTxBonus[gameObject.tag];
		matBack.mainTexture = dictTxBonus[gameObject.tag];

	}

	public void onHit(){
		manageEvents.AudioPlayer.publish ("hitBonus", false, 1f);
		manageEvents.GameState.publish(this.tag);
		Destroy (gameObject);
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "ground"){
			Destroy (gameObject);
		}
	}

	void Update () {
		transform.position += new Vector3 ( Mathf.Sin(Mathf.PI * speedX * Time.time) * rangeX * Time.deltaTime , -speedY * Time.deltaTime, 0f);
		transform.Rotate(new Vector3(0f, Time.deltaTime * ang, 0f));
	}
}
