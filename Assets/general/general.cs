using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class general : MonoBehaviour {

	public uint level;
	public int enemyKillCountForWin;
	public float totalTimeLeft;
	public GameObject bonus;
	public float delayBeforeGameOver;


	public static Dictionary<string, GameObject> gameObjectList = new Dictionary<string, GameObject>();
	public static Dictionary<string, Texture2D> texture2DList = new Dictionary<string, Texture2D>();

	string[] bonusTags = { "bonusLife", "bonusTime", "bonusAmmo", "bonusSpeedShot" };

	float enemyKillCountMax;
	bool oneSay = true;

	void Start () {
		enemyKillCountMax = enemyKillCountForWin;
		Invoke("eventsInit", 0.1f);
	}

	void eventsInit(){
		manageEvents.AudioPlayer.publish ("fight", false, 1f);
		manageEvents.AudioPlayer.publish ("main", true, 1f);

		manageEvents.UpdateGUI.publish("textDestroyCount", enemyKillCountForWin);
		manageEvents.AllSleep.publish(true);

		manageEvents.EnemyDestroy.subscribe((object obj)=>{
			enemy enm = (enemy)obj;

			switch (enm.rank) {
			case enemy.enumRank.soldier:
			case enemy.enumRank.jumper:
			case enemy.enumRank.swinger:
			case enemy.enumRank.rotater:
			case enemy.enumRank.bomber:
				// убил врага
				enemyKillCountForWin--;
				if(enemyKillCountForWin<0) enemyKillCountForWin=0;
				manageEvents.UpdateGUI.publish("textDestroyCount", enemyKillCountForWin);
				break;
			case enemy.enumRank.boss:
				// убили босса переход на следующий уровень игры
				manageEvents.AudioPlayer.publish ("flawlessvictory", false, 1f);
				manageEvents.GameState.publish("stop");
//				SceneManager.LoadScene("level_" + (level+1));
				Invoke( "gameWin", delayBeforeGameOver);
				manageEvents.UpdateGUI.publish("textMessage", "Ура, победа!");
				break;
			case enemy.enumRank.good:
				// убил good-объект минус 1-жизнь
				manageEvents.PlayerHit.publish();
				break;
			default:
				break;
			}
		});

		manageEvents.UpdateGUI.subscribe ((object obj) => {
			var pkg = (events.TPackage)obj;
			if(pkg.key.ToString() == "textPlayerLife") if(((uint)pkg.value) <= 0){
				manageEvents.UpdateGUI.publish("textMessage", "кончились жизни");
				Invoke( "gameOver", delayBeforeGameOver);
			}
			if(pkg.key.ToString() == "textAmmo") if(((uint)pkg.value) <= 0){
				Invoke( "gameOver", delayBeforeGameOver);
				manageEvents.UpdateGUI.publish("textMessage", "кончились патроны");
			}
		});

		manageEvents.GameState.subscribe ((object obj) => {

			if (obj.ToString () == "bonusTime") {
				totalTimeLeft += 10;
				manageEvents.UpdateGUI.publish ("textTimeLeft", totalTimeLeft);
			}
		});

	}

	void gameWin(){
		SceneManager.LoadScene("win");
	}
	void gameOver(){
		SceneManager.LoadScene("gameOver");
	}

	void Update () {
		totalTimeLeft -= Time.deltaTime;

		if (totalTimeLeft <= 0) {
			totalTimeLeft = 0f;
			Invoke( "gameOver", delayBeforeGameOver);
			manageEvents.UpdateGUI.publish("textMessage", "кончилось время");
		}
			
		manageEvents.UpdateGUI.publish("textTimeLeft", totalTimeLeft);

		if(utils.isTimeout (1, 10f + UnityEngine.Random.Range(1.0f, 10.0f) )){
			CBouns b = Instantiate (bonus, new Vector4 (UnityEngine.Random.Range(-1.0f, 1.0f), 10f, 0f), Quaternion.identity).GetComponent<CBouns>();
			int x = UnityEngine.Random.Range (0, bonusTags.Length);
			b.tag = bonusTags[x];
		}

		logicLevel();
	}

	void logicLevel(){
		switch (level) {
		case 0:
			if (enemyKillCountForWin <= utils.getPrc(enemyKillCountMax, 100f)) {

				manageEvents.SoldierSleep.publish(false);
				manageEvents.BomberSleep.publish(false);

			}
			if (enemyKillCountForWin <= utils.getPrc(enemyKillCountMax, 80f)) {
				manageEvents.GoodSleep.publish(false);
			}
			if (enemyKillCountForWin <= utils.getPrc(enemyKillCountMax, 60)) {
				manageEvents.SoldierSleep.publish(true);
				manageEvents.JumperSleep.publish(false);
				if (oneSay) {
					oneSay = false;
					manageEvents.AudioPlayer.publish ("youNever", false, 1f);
				}
			}
			if (enemyKillCountForWin <= utils.getPrc(enemyKillCountMax, 40f)) {
				manageEvents.JumperSleep.publish(true);
				manageEvents.SwingerSleep.publish(false);
			}
			if (enemyKillCountForWin <= utils.getPrc(enemyKillCountMax, 20f)) {
				manageEvents.SwingerSleep.publish(true);
				manageEvents.RotaterSleep.publish(false);
			}
			if (enemyKillCountForWin == 0) {
				manageEvents.RotaterSleep.publish(true);
				manageEvents.BossSleep.publish(false);
			}
			break;
		}
	}

	Dictionary<string, GameObject> getObjectsListAll(){

		Dictionary<string, GameObject> list = new Dictionary<string, GameObject>();
		GameObject []objects = Resources.FindObjectsOfTypeAll<GameObject>();

		foreach (GameObject it in objects) {
			try {
				list.Add(it.name, it);
			} catch (System.Exception ex) {
				print (ex);
			}
		}
		return list;
	}

	Dictionary<string, Texture2D> getTexture2DListAll(){

		Dictionary<string, Texture2D> list = new Dictionary<string, Texture2D>();
		Texture2D []objects = Resources.FindObjectsOfTypeAll<Texture2D>();

		foreach (Texture2D it in objects) {
			try {
				list.Add(it.name, it);
			} catch (System.Exception ex) {
				print (ex);
			}
		}
		return list;
	}
}
