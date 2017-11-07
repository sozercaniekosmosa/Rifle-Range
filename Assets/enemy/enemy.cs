using UnityEngine;
using System;
using System.Collections;
//using System.Threading;

public class enemy : MonoBehaviour {

	struct TBehavior{
		public float life;
		public float speedX;
		public float speedY;
		public float forceY;
		public float speedRot;
		public float forceRot;
		public Texture2D texture;
		public float timeBeforeCreate;
	}

	public enum enumRank
	{
		soldier=0,
		jumper=1,
		swinger=2,
		rotater=3,
		bomber=4,
		boss=5,
		good=6,
		any=7
	}

	[System.Serializable]
	public struct TexutereRank
	{
		public Texture2D soldier;
		public Texture2D jumper;
		public Texture2D swinger;
		public Texture2D rotater;
		public Texture2D bomber;
		public Texture2D boss;
		public Texture2D good;
	}

	public bool sleep = true;
	public float randSpeedX = 0.1f;
	public float timeRelaxAtferShot;
	public float maxAngleShot;
	public TexutereRank textures;
	public GameObject bomb;
	public enumRank rank;

	TBehavior[] listBehavior;

	Vector3 startPos;
	Vector3 pos;
	Vector3 ang;
	float timeAcc = 0f;
	float timer;
	float startLife;
	float startSpeedX;
	float _posX = 0f;
	float timeBeforeCreate;
	float timeBomb = 0f;

	Material matPlace;
	GameObject crown;

	float life;
	float speedX;
	float speedY;
	float forceY;
	float speedRot;
	float forceRot;

	TBehavior currBehavior;

	void Start () {

		listBehavior = new TBehavior[]{
			new TBehavior() {life = 1f, speedX = 2f,	speedY = 0f,	forceY = 0f, speedRot = 0f,	forceRot = 0f,	timeBeforeCreate = 2f, texture = textures.soldier},
			new TBehavior() {life = 1f, speedX = 1f,	speedY = 4f,	forceY = 7f, speedRot = 0f,	forceRot = 0f,	timeBeforeCreate = 2f, texture = textures.jumper},
			new TBehavior() {life = 1f, speedX = 1.5f,	speedY = 0f,	forceY = 0f, speedRot = 5f,	forceRot = 4f,	timeBeforeCreate = 2f, texture = textures.swinger},
			new TBehavior() {life = 1f, speedX = 2f,	speedY = 3f,	forceY = 6f, speedRot = 5f,	forceRot = 4f,	timeBeforeCreate = 2f, texture = textures.rotater},
			new TBehavior() {life = 1f, speedX = 2f,	speedY = 6f,	forceY = 5f, speedRot = 0f,	forceRot = 0f,	timeBeforeCreate = 5f, texture = textures.bomber},
			new TBehavior() {life = 3f, speedX = 2f,	speedY = 8f,	forceY = 1f, speedRot = 6f,	forceRot = 1f,	timeBeforeCreate = 0f, texture = textures.boss},
			new TBehavior() {life = 1f, speedX = 2f,	speedY = 3f,	forceY = 5f, speedRot = 5f,	forceRot = 4f,	timeBeforeCreate = 0f, texture = textures.good},
			new TBehavior() {life = 0f, speedX = 0f,	speedY = 0f,	forceY = 0f, speedRot = 0f,	forceRot = 0f,	timeBeforeCreate = 0f, texture = null},
		};

		matPlace = utils.getMaterial (gameObject, "place");
		crown = transform.Find ("crown").gameObject;

		startPos = transform.position;

		createEnemyPerson ();

		Invoke("eventsInit", 0.1f);
	}

	void eventsInit(){
		Action<object> callback = (object obj) =>{ sleep = (bool)obj;};

		manageEvents.AllSleep.subscribe (callback);
		switch (rank) {
		case enumRank.bomber: manageEvents.BomberSleep.subscribe (callback); break;
		case enumRank.boss: manageEvents.BossSleep.subscribe (callback); break;
		case enumRank.good: manageEvents.GoodSleep.subscribe (callback); break;
		case enumRank.jumper: manageEvents.JumperSleep.subscribe (callback); break;
		case enumRank.rotater: manageEvents.RotaterSleep.subscribe (callback); break;
		case enumRank.soldier: manageEvents.SoldierSleep.subscribe (callback); break;
		case enumRank.swinger: manageEvents.SwingerSleep.subscribe (callback); break;
		}

		manageEvents.GameState.subscribe ((object obj)=>{
			if(((string)obj) == "stop") Destroy(gameObject);
		});

	}

	public void onHit(){
		if (sleep) return;

		if (life <= 0) return;
		ang = transform.eulerAngles;
		life--;
		manageEvents.AudioPlayer.publish ("hitEnemy", false, 1.0f);
		if (life <= 0) {
			pos.y = startPos.y;
			ang = new Vector3(90f,0f,0f);
			life = 0f;
			LateUpdate();
			Invoke ("createEnemyPerson", timeBeforeCreate);
			manageEvents.EnemyDestroy.publish(this);
		}else{

			if(life == 1) manageEvents.AudioPlayer.publish ("finishhim", false, 1f);

			ang.x += maxAngleShot;
			if (ang.x > 90f) ang.x = 90f;
			timer = Time.time + timeRelaxAtferShot;
		}
	}

	void createEnemyPerson(){
		pos = startPos;
		timer = Time.time + timeRelaxAtferShot;
		ang = new Vector3(90f, 0f, 0f);
		setRank (rank);
		if(speedX > 0f) speedX += UnityEngine.Random.Range (0.5f, randSpeedX);
		timeAcc = 0f;
	}



//	void OnTriggerEnter(Collider other){ }

	void LateUpdate () {
		if ((life > 0) && (sleep == false)) {

			if (Mathf.Clamp(pos.x, -startPos.x * 0.3f, startPos.x * 0.3f) == pos.x)
//			if ((rank == enumRank.bomber) && (utils.isTimeout (2, 0.7f )) ) {
			if ((rank == enumRank.bomber) && (utils.isTimeout (2, 5f + timeBomb )) ) {
				timeBomb = UnityEngine.Random.Range (1.0f, 5.0f);
				Instantiate (bomb, pos + new Vector3 (0f, 2.5f, 0f), Quaternion.identity).GetComponent<enemy> ();
			}

			timeAcc += Time.deltaTime;

//			pos.x = Mathf.Sin (Mathf.PI * (speedX * time * 0.1f)) * startPos.x;
			pos.x = Mathf.Sin (Mathf.PI * 0.5f + speedX* 0.5f * timeAcc) * startPos.x;
			pos.y = Mathf.Sin (timeAcc * Mathf.Deg2Rad * speedY * 100f) * forceY * 0.1f + startPos.y;
			ang.z = Mathf.Sin (timeAcc * Mathf.Deg2Rad * speedRot * 100f) * forceRot * 10f;

			float tm = timer - Time.time;
			if (tm > 0)
				ang = new Vector3 (Mathf.Lerp (0f, ang.x, (1f / timeRelaxAtferShot) * tm), 0f, ang.z);
			else
				ang = new Vector3 (0f, 0f, ang.z);

			matPlace.SetTextureScale ("_MainTex", new Vector2 (_posX - pos.x > 0 ? 1 : -1, 1f));
			_posX = pos.x;

			if (rank == enumRank.boss) {
				if (life == 2) {
					float lerp = Mathf.PingPong (Time.time * 10, 1f);
					matPlace.color = Color.Lerp (Color.white, Color.yellow, lerp);
				} else if (life == 1) {
					float lerp = Mathf.PingPong (Time.time * 10, 1f);
					matPlace.color = Color.Lerp (Color.white, Color.red, lerp);
					speedX = startSpeedX + utils.getPrc (startSpeedX, 100f);
				} else {
					matPlace.color = Color.white;
				}
			}
		} else {
			ang = new Vector3(90f, 0f, 0f);
		}

		transform.position = pos;
		transform.rotation = Quaternion.Euler(ang);
	}

	void setRank (enumRank rank){
		currBehavior = listBehavior [(int)rank];

		life = currBehavior.life;
		speedX = currBehavior.speedX;
		speedY = currBehavior.speedY;
		forceY = currBehavior.forceY;
		speedRot = currBehavior.speedRot;
		forceRot = currBehavior.forceRot;
		timeBeforeCreate = currBehavior.timeBeforeCreate;

		startLife = life;
		startSpeedX = speedX;
		try {
			matPlace.mainTexture = currBehavior.texture;
		} catch (Exception ex) {
			print (ex);
		}
		crown.SetActive (rank == enumRank.boss);
	}
}
