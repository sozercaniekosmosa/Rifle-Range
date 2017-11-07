using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class infoGUI : MonoBehaviour {

	Text textPlayerLife;
	Text textDestroyCount;
	Text textTimeLeft;
	Text textAmmo;
	Text textSpeedShot;
	Text textDbg;
	Text textMessage;
	GameObject backPanel;

	void Start () {
		init ();
	}

	void init() {
		textPlayerLife = transform.Find ("textPlayerLife").gameObject.GetComponent<Text>() as Text;
		textDestroyCount = transform.Find ("textDestroyCount").gameObject.GetComponent<Text>() as Text;
		textTimeLeft = transform.Find ("textTimeLeft").gameObject.GetComponent<Text>() as Text;
		textAmmo = transform.Find ("textAmmo").gameObject.GetComponent<Text>() as Text;
		textSpeedShot = transform.Find ("textSpeedShot").gameObject.GetComponent<Text>() as Text;
		textDbg = transform.Find ("textDbg").gameObject.GetComponent<Text>() as Text;
		backPanel = transform.Find ("backPanel").gameObject;
		textMessage = backPanel.transform.Find ("textMessage").gameObject.GetComponent<Text>() as Text;

		manageEvents.UpdateGUI.subscribe((object obj)=>{
			var pkg = (events.TPackage)obj;

			switch (pkg.key.ToString()) {
			case "textPlayerLife":
				textPlayerLife.text = "жизни: " + pkg.value;
				break;
			case "textDestroyCount":
				if(transform != null){
					textDestroyCount = transform.Find ("textDestroyCount").gameObject.GetComponent<Text>() as Text;
					textDestroyCount.text = "враги: " + pkg.value;
				}
				break;
			case "textTimeLeft":
				textTimeLeft.text = "время: " + Mathf.RoundToInt(float.Parse(pkg.value.ToString()));
				break;
			case "textAmmo":
				textAmmo.text = "патроны: " + pkg.value;
				break;
			case "textSpeedShot":
				textSpeedShot.text = pkg.value + "x";
				break;
			case "textMessage":
				backPanel.SetActive(true);
				textMessage.text = pkg.value + "";
				break;
			case "textDbg":
				textDbg.text = pkg.value + "";
				break;
			}
		});
	}

	void Update () {
		
	}
}
