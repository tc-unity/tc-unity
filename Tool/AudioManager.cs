using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AudioManager : MonoBehaviour
{
	static private AudioManager instance;
	static public AudioManager Instanct{
		get{
			if(instance == null){
				instance = new GameObject("AudioManager").AddComponent<AudioManager>();
			}
			return instance;
		}
	}
	const string partoclePath = "audio/";
	private Dictionary<string,AudioClip> audioResource = new Dictionary<string, AudioClip> ();

	public AudioSource Play(string ClipName, Vector3 pos, bool loop){
		if (audioResource.ContainsKey (ClipName)) {
			return Play (audioResource [ClipName], pos, loop);
		} else {
			AudioClip temp = Resources.Load(partoclePath + ClipName) as AudioClip;
			try{
				if(temp != null){
					audioResource.Add(ClipName,temp);
				}else{
					throw new Exception("load audio fail : " + ClipName);
				}
			}catch(Exception e){
				Debug.Log(e.ToString());
			}
			if(audioResource.ContainsKey(ClipName)){
				return Play (audioResource [ClipName], pos, loop);
			}else{
				return null;
			}
		}
	}

	public AudioSource Play(string ClipName, Vector3 pos, bool loop, float volume){
		if (audioResource.ContainsKey (ClipName)) {
			return Play (audioResource [ClipName], pos, loop, volume);
		} else {
			AudioClip temp = Resources.Load(partoclePath + ClipName) as AudioClip;
			try{
				if(temp != null){
					audioResource.Add(ClipName,temp);
				}else{
					throw new Exception("load audio fail : " + ClipName);
				}
			}catch(Exception e){
				Debug.Log(e.ToString());
			}
			if(audioResource.ContainsKey(ClipName)){
				return Play (audioResource [ClipName], pos, loop, volume);
			}else{
				return null;
			}
		}
	}

	public AudioSource Play(AudioClip clip, Transform emitter)
	{
		return Play(clip, emitter, 1f, 1f);
	}
	
	public AudioSource Play(AudioClip clip, Transform emitter, float volume)
	{
		return Play(clip, emitter, volume, 1f);
	}
	
	///
	/// Plays a sound by creating an empty game object with an AudioSource
	/// and attaching it to the given transform (so it moves with the transform). Destroys it after it finished playing.
	///
	///
	///
	///
	///
	///
	public AudioSource Play(AudioClip clip, Transform emitter, float volume, float pitch)
	{
		//Create an empty game object
		GameObject go = new GameObject ("Audio: " + clip.name);
		go.transform.position = emitter.position;
		go.transform.parent = emitter;
		
		//Create the source
		AudioSource source = go.AddComponent<AudioSource>();
		source.clip = clip;
		source.volume = volume;
		source.pitch = pitch;
		source.Play ();
		Destroy (go, clip.length);
		return source;
	}
	
	public AudioSource Play(AudioClip clip, Vector3 point)
	{
		return Play(clip, point, false, 1f, 1f);
	}
	
	public AudioSource Play(AudioClip clip, Vector3 point, bool loop)
	{
		return Play(clip, point, loop, 1f, 1f);
	}
	
	public AudioSource Play(AudioClip clip, Vector3 point, bool loop, float volume)
	{
		return Play(clip, point, loop, volume, 1f);
	}
	
	///
	/// Plays a sound at the given point in space by creating an empty game object with an AudioSource
	/// in that place and destroys it after it finished playing.
	///
	///
	///
	///
	///
	///
	public AudioSource Play(AudioClip clip, Vector3 point, bool loop, float volume, float pitch)
	{
		//Create an empty game object
		GameObject go = new GameObject("Audio: " + clip.name);
		go.transform.position = point;
		
		//Create the source
		AudioSource source = go.AddComponent<AudioSource>();
		source.clip = clip;
		source.volume = volume;
		source.pitch = pitch;
		source.Play();
		if (loop) {
			source.loop = true;
		} else {
			Destroy(go, clip.length);
		}
		return source;
	}

    public void Stop(string clip)
    {
        GameObject go = GameObject.Find("Audio: " + clip);
        if (go!=null)
        {
            AudioSource source = go.GetComponent<AudioSource>();
            StartCoroutine(VolumeMin(source));
            Destroy(go, 3);
        }
    }
    IEnumerator VolumeMin(AudioSource source)
    {
        while (source.volume>0.1f)
        {
            source.volume = Mathf.Lerp(source.volume, 0, 0.1f);
            yield return new WaitForSeconds(0.1f);
        }
    }

	void OnDisable(){
		foreach(AudioClip obj in audioResource.Values){
			Resources.UnloadAsset (obj);
		}
		audioResource.Clear ();
		Resources.UnloadUnusedAssets ();
	}
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                