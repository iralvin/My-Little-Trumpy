using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicController : MonoBehaviour {

	AudioSource audioSource;
	private static BackgroundMusicController instance;

	private void Awake()
	{
		Debug.Log("audio level is 1");
		// First we check if there are any other instances conflicting
		if (instance != null)
		{
			// If that is the case, we destroy other instances
			Destroy(gameObject);
            //Debug.Log("destroyed instance of background music" + gameObject);
        }
		else if (instance == null)
		{
			// Here we save our singleton instance
			instance = this;
			//Debug.Log("instance saved as " + this);
			// Furthermore we make sure that we don't destroy between scenes (this is optional)
			DontDestroyOnLoad(gameObject);
		}
	}


	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update () {
		if (PlayerPrefsController.musicState == 1)
		{
			audioSource.volume = 0.5f;
			//Debug.Log("audio level is " + audioSource.volume);
		}
		else if (PlayerPrefsController.musicState == 0)
		{
			audioSource.volume = 0;
			//Debug.Log("audio level is " + audioSource.volume);
		}
	}




}
