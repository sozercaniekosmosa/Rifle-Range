using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class manageEvents{
	public static events EnemyDestroy = new events();
	public static events PlayerHit = new events();
	public static events BonusHit = new events();

	public static events UpdateGUI = new events();
	public static events GameState = new events();
	public static events AudioPlayer = new events();

	public static events SoldierSleep = new events();
	public static events JumperSleep = new events();
	public static events SwingerSleep = new events();
	public static events RotaterSleep = new events();
	public static events BomberSleep = new events();
	public static events BossSleep = new events();
	public static events GoodSleep = new events();
	public static events AllSleep = new events();
}
