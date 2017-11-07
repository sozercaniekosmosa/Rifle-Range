using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public static class utils{

	private static float[]timeout = new float[64];

	public static bool isTimeout(uint id, float sec){
		if ((timeout[id] - Time.time) < 0) {
			timeout[id] = Time.time + sec;
			return true;
		}
		return false;
	}

	public static Material getMaterial(GameObject obj, string child=null){
		obj = (child==null)?obj:obj.transform.Find (child).gameObject;
		Renderer renderer = obj.GetComponent<Renderer>();
		return renderer.material;
	}

	public static float getPrc(float value, float prc){
		return (prc * value / 100f);
	}

	public static void setProp(object obj, string name, object val){
		PropertyInfo prop = obj.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
		if(null != prop && prop.CanWrite){
			prop.SetValue(obj, val, null);
		}
	}

	public static void callMethod(object obj, string name, object[] param){
		MethodInfo mi = obj.GetType ().GetMethod (name);
		mi.Invoke (obj, param);
	}

	public static void log(object mess){
		manageEvents.UpdateGUI.publish ("textDbg", mess);
	}
}
