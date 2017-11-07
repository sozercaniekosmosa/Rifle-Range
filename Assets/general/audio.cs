using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audio : MonoBehaviour {

	public AudioClip[] audioClips;
	Dictionary<string, AudioClip> dictAudioClips = new Dictionary<string, AudioClip>();

	AudioSource audioSourceOneShoters;
	AudioSource audioSourceLoops;

	void Start () {

		AudioSource[] tmp = GetComponents<AudioSource> ();
		audioSourceOneShoters = tmp[0];
		audioSourceLoops = tmp[1];

		foreach (AudioClip it in audioClips) {
			try {
				dictAudioClips.Add(it.name, it);
			} catch (System.Exception ex) {
				print (ex);
			}
		}

		manageEvents.AudioPlayer.subscribe((object obj)=>{
			var pkg = (events.TPackage2)obj;

			if((bool)pkg.value){
				audioSourceLoops.clip = dictAudioClips[pkg.key.ToString()];
				audioSourceLoops.Play();
			}else{
				audioSourceOneShoters.PlayOneShot (dictAudioClips[pkg.key.ToString()], (pkg.value2==null?1f:(float)pkg.value2));	
			}
		});

	}

	void Update () {
		
	}
}
