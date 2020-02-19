using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsController : MonoBehaviour {

	// int value 1 means soundFX is ON
	// int value 0 means soundFX is OFF
	public static int soundFXState;
	public static int musicState;
	public static bool hasPlayedBefore;
	//	public GameObject tutorialPage;

	private void Awake()
	{
        //DeletePlayerPrefSettings();
        hasPlayedBefore = PlayerPrefsX.GetBool("HasPlayedBefore");
		Debug.Log ("PLAYERPREFSCONTROLLER hasplayed = " + hasPlayedBefore);

		//		Debug.Log(soundd);
		//		soundd = PlayerPrefsX.GetBool("SoundfxState");
		//		Debug.Log(soundd);
	}


	// Use this for initialization
	void Start () {
		if (hasPlayedBefore == true)
		{
			soundFXState = PlayerPrefs.GetInt("SoundFX State");
			musicState = PlayerPrefs.GetInt("Music State");

		} else if (hasPlayedBefore == false)
		{
            soundFXState = 1;
            musicState = 1;
            PlayerPrefs.SetInt("Music State", musicState);
			PlayerPrefs.SetInt("SoundFX State", soundFXState);
		}

		Debug.Log("music state is " + musicState);
		Debug.Log("soundfx state is " + soundFXState);

	}

	public void resetHasPlayed(){
		hasPlayedBefore = false;
		PlayerPrefsX.SetBool("HasPlayedBefore", hasPlayedBefore);
		Debug.Log ("hasplayedbefore = " + hasPlayedBefore);
	}

    public void DeletePlayerPrefSettings()
    {
        PlayerPrefs.DeleteAll();
        
    }

    private void OnDestroy()
    {
        hasPlayedBefore = true;
        PlayerPrefsX.SetBool("HasPlayedBefore", hasPlayedBefore);

    }



    public void ToggleSoundFX()
	{
		if (soundFXState == 0)
		{
			// turn soundFX ON
			soundFXState = 1;
			Debug.Log("soundFx is ON" );
		} else if (soundFXState == 1)
		{
			// turn soundFX OFF
			soundFXState = 0;
			Debug.Log("soundFx is OFF");
		}

		PlayerPrefs.SetInt("SoundFX State", soundFXState);
	}


	public void ToggleMusic()
	{
		if (musicState == 0)
		{
			// turn soundFX ON
			musicState = 1;
			Debug.Log("music State is ON");
		}
		else if (musicState == 1)
		{
			// turn soundFX OFF
			musicState = 0;
			Debug.Log("musicState is OFF");
		}

		PlayerPrefs.SetInt("Music State", musicState);
	}




}






