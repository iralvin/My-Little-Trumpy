using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ExperienceController : MonoBehaviour {

	int exp;
	public int currentLevel;
	public  Text expText, levelText;
	public  int[] levelExpArray = new int[16];
	public Slider expBar;
	public GameObject manager;
	Scene currentScene;
	string sceneName;
	public GameObject resetPopUp;
    public GameObject evolvingTextBox;
    public Text evolvingText;

    public Text currentTrumpyName;
    public string[] trumpyNamesArray = new string[16];

    /* TRUMPY NAMES
    0. n/a
    1. Cheddar Trumpy
    2. Gremlin Trumpy
    3. Lamprey Trumpy
    4. Baby Trumpy
    5. Bully Trumpy
    6. Bizness Trumpy
    7. Emperor Trumpy
    8. Horny Trumpy
    9. Scorpion Trumpy
    10. Sluggy Trumpy
    11. Sexy Trumpy
    12. NSFW Trumpy
    13. Jabba Trumpy
    14. Wispy Trumpy
    15. Grampy Trumpy
    */

    void Awake(){
//		Debug.Log ("EXPERIENCECONTROLLER hasplayed = " + PlayerPrefsController.hasPlayedBefore);

		exp = PlayerPrefs.GetInt ("TrumpyEXP");
		currentLevel = PlayerPrefs.GetInt ("TrumpyLevel");
		Debug.Log ("EXPERIENCECONTROLLER trumpy level is " + currentLevel);


        // TESTING EVOLUTION SETTING CONSISTENT LEVEL
        //currentLevel = 8;
        //exp = 100000;

    }

	// Use this for initialization
	void Start () {
		if (PlayerPrefsController.hasPlayedBefore == false) {
			currentLevel = 1;
			PlayerPrefs.SetInt ("TrumpyLevel", currentLevel);
		}
//		Debug.Log ("EXPERIENCECONTROLLER trumpy level is " + currentLevel);

		currentScene = SceneManager.GetActiveScene();
		sceneName = currentScene.name;
		if (sceneName == "game") {
			expText.text = (exp.ToString () + " / " + levelExpArray [currentLevel].ToString ());
			expBar.maxValue = levelExpArray [currentLevel];
			levelText.text = ("EGO Level " + currentLevel);
            currentTrumpyName.text = (trumpyNamesArray[currentLevel]);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (sceneName == "game") {
			expBar.value = exp;
		}

    }
    public void ActivateEvolvingTextBox()
    {
        evolvingTextBox.SetActive(true);
		evolvingText.text = (trumpyNamesArray[currentLevel -1] + " is evolving into...");
		gameObject.GetComponent<FoodSpawnController> ().foodCanvas.SetActive (false);
		gameObject.GetComponent<FoodSpawnController> ().enabled = false;
        Invoke("DisplayTrumpyNameWhenEvolving", 2f);
    }
    
    void DisplayTrumpyNameWhenEvolving()
    {
		evolvingText.text = (trumpyNamesArray[currentLevel -1] + " is evolving into..." + trumpyNamesArray[currentLevel] + "!");
        Invoke("DisableEvolvingTextBox", 1f);
    }
    void DisableEvolvingTextBox()
    {
        evolvingTextBox.SetActive(false);
        gameObject.GetComponent<TrumpyController>().isEvolving = false;

		gameObject.GetComponent<FoodSpawnController> ().foodCanvas.SetActive (true);
		gameObject.GetComponent<FoodSpawnController> ().enabled = true;
    }

    public  void UpdateExp(int foodExpReceived){
		exp += foodExpReceived;
		expBar.value = exp;
		expText.text = (exp.ToString() + " / " + levelExpArray[currentLevel].ToString());

		if (exp >= levelExpArray [currentLevel] && currentLevel < 15) {
			currentLevel++;
			exp = 0;
			expBar.value = exp;
			expText.text = (exp.ToString () + " / " + levelExpArray [currentLevel].ToString ());
			levelText.text = ("EGO Level " + currentLevel);
            currentTrumpyName.text = (trumpyNamesArray[currentLevel]);
			expBar.maxValue = levelExpArray [currentLevel];
            manager.GetComponent<TrumpyController>().LeveledUp();
        }
        else if (exp >= levelExpArray [currentLevel] && currentLevel >= 15) {
			exp = levelExpArray [currentLevel];
			expBar.value = exp;
			expText.text = (exp.ToString () + " / " + levelExpArray [currentLevel].ToString ());
		}
		PlayerPrefs.SetInt ("TrumpyEXP", exp);
		PlayerPrefs.SetInt ("TrumpyLevel", currentLevel);
	}

	public void YesResetTrumpy(){
		exp = 0;
		currentLevel = 1;
		PlayerPrefs.SetInt ("TrumpyEXP", exp);
		PlayerPrefs.SetInt ("TrumpyLevel", currentLevel);
		resetPopUp.SetActive (false);
	}

	public void NoResetTrumpy(){
		resetPopUp.SetActive (false);
	}

	public void ActivateResetPopUp(){
		resetPopUp.SetActive (true);
	}

	void OnApplicationQuit(){
		PlayerPrefs.SetInt ("TrumpyEXP", exp);
		PlayerPrefs.SetInt ("TrumpyLevel", currentLevel);
	}

	void OnDestroy(){
		PlayerPrefs.SetInt ("TrumpyEXP", exp);
		PlayerPrefs.SetInt ("TrumpyLevel", currentLevel);
	}
}
