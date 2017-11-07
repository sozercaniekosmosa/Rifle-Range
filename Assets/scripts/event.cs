using System;
using System.Collections;
using System.Collections.Generic;

public class events{
	
	public struct TPackage{

		public Object set(Object key, Object value){
			this.key = key;
			this.value = value;

			return this;
		}

		public object key;
		public object value;
	}

	public struct TPackage2{

		public Object set(Object key, Object value, Object value2){
			this.key = key;
			this.value = value;
			this.value2 = value2;

			return this;
		}

		public object key;
		public object value;
		public object value2;
	}

	TPackage pakage = new TPackage();
	TPackage2 pakage2 = new TPackage2();

	private readonly List<Action<Object>> callList = new List<Action<Object>>(); 

	public void subscribe(Action<Object> subscriber){
		callList.Add(subscriber);
	}

	public void publish(Object e = null){
		foreach (Action<Object> it in callList)
			it(e);
	}

	public void publish(Object key, Object value){
		foreach (Action<Object> it in callList)
			it(pakage.set(key, value));
	}

	public void publish(Object key, Object value, Object value2){
		foreach (Action<Object> it in callList)
			it(pakage2.set(key, value, value2));
	}
}