using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FoodSpawnController : MonoBehaviour {

	public GameObject[] foodArray = new GameObject[23];
	public int[] foodExpArray = new int[23];
	public GameObject poop2, poop1;
	public int foodCountOnScreen;

	/* FOOD EXPERIENCE GIVEN IS AS FOLLOWS BASED ON ITEM NUMBER 0,1,2,3,4.....
	 * 0 = 0
	 * 1 = 8
	 * 2 = 8
	 * 3 = 8
	 * 4 = 16
	 * 5 = 16
	 * 6 = 16
	 * 7 = 24
	 * 8 = 24
	 * 9 = 24
	 * 10 = 32
	 * 11 = 32
	 * 12 = 32
	 * 13 = 40
	 * 14 = 40
	 * 15 = 40
	 * 16 = 48
	 * 17 = 56
	 * 18 = 64
	 * 19 = 72
	 * 20 = 80
	 * 21 = 88
	 * 22 = 96
	*/

	// max allowed random roll number for food choices based on level (levels starting 0, 1, 2, 3, 4...)
	int[] foodMaxRoll = new int[] {0, 3, 6, 9, 12, 14, 16, 17, 18, 19, 20, 21, 22, 22, 22, 22};

    

	public GameObject foodSpawnArea;
	public GameObject foodCanvas;
	float timeBetweenSpawn;
	float timeBetweenPoopSpawn;
	float screenX, screenY;
	float sizeX,sizeY;

	Vector3 size;

	AudioSource audioSource;
	public AudioClip popSound;
    public DateTime exitTime;
    DateTime startTime;
    long lastCurrentTimeDifference;
    long numFoodToSpawn;
    long secondsPerSpawnWhenClosed = 15;        // how many seconds need to pass in order to spawn 1 food when game closed
    int randomNum;

    private void Awake()
    {
        if (PlayerPrefsController.hasPlayedBefore == true)
        {
            exitTime = DateTime.Parse(PlayerPrefs.GetString("ExitTime"));
            startTime = DateTime.Now;
            Debug.Log("game start previous exit time is " + exitTime);
            Debug.Log("start time is " + startTime);
        }
        
    }


    // Use this for initialization
    void Start () {

        foodCanvas = GameObject.Find("foodCanvas");
        foodSpawnArea = GameObject.Find("foodSpawnArea");
        foodCanvas.GetComponent<Canvas>().worldCamera = Camera.main;

        Debug.Log(foodCanvas + " " + foodSpawnArea);

        audioSource = gameObject.GetComponent<AudioSource> ();
		timeBetweenSpawn = 1f;
		timeBetweenPoopSpawn = 3f;

		sizeX = foodSpawnArea.GetComponent<RectTransform> ().rect.width;
		sizeY = foodSpawnArea.GetComponent<RectTransform> ().rect.height;
		Debug.Log("sizeX = " + sizeX + ".....sizeY  = " + sizeY);
        //		Debug.Log("screenx = " + screenX + " and screeny = " + screenY);

        TimeSpan difference = startTime.Subtract(exitTime);
        lastCurrentTimeDifference = (long)difference.TotalSeconds;
        numFoodToSpawn = lastCurrentTimeDifference / secondsPerSpawnWhenClosed;
        if (numFoodToSpawn > 15)
        {
            numFoodToSpawn = 15;
        }
        if (numFoodToSpawn > 0)
        {
            SpawnFoodAtStart();
        }
        
    }
	
	// Update is called once per frame
	void Update () {
		timeBetweenSpawn -= Time.deltaTime;
		timeBetweenPoopSpawn -= Time.deltaTime;
		if (timeBetweenSpawn <= 0 && foodCountOnScreen <15) {
            randomNum = UnityEngine.Random.Range(1, foodMaxRoll[gameObject.GetComponent<ExperienceController>().currentLevel]);
            //randomNum = UnityEngine.Random.Range(1, 1);
            SpawnFood (randomNum);
		}
		if (timeBetweenPoopSpawn <= 0 && foodCountOnScreen < 15) {
			int randomPoop = UnityEngine.Random.Range (0, 6);
			if (randomPoop == 0) {
				SpawnFood (randomPoop);
			} else {
				timeBetweenPoopSpawn = 3f;
			}
		}
	}

    void SpawnFoodAtStart()
    {
        for (long i = 0; i < numFoodToSpawn; i++)
        {
            randomNum = UnityEngine.Random.Range(0, foodMaxRoll[gameObject.GetComponent<ExperienceController>().currentLevel]);
            SpawnFood(randomNum);
        }
    }




    public void SpawnFood(int foodToSpawn){

		float foodSizeX = foodArray [foodToSpawn].GetComponent<RectTransform> ().rect.width;
		float foodSizeY = foodArray [foodToSpawn].GetComponent<RectTransform> ().rect.height;
		float halfFoodSizeX = (foodSizeX/2f)*foodArray[foodToSpawn].GetComponent<RectTransform>().localScale.x;
		float halfFoodSizeY = (foodSizeY/2f)*foodArray[foodToSpawn].GetComponent<RectTransform>().localScale.y;
        

		float randomFoodX = UnityEngine.Random.Range( (-1f * (sizeX / 2f)) + halfFoodSizeX, (sizeX / 2f) - halfFoodSizeX);
		float randomFoodY = UnityEngine.Random.Range( (-1f * (sizeY / 2f)) + halfFoodSizeY, (sizeY / 2f) - halfFoodSizeY);

		Vector3 foodLocation = new Vector3 (randomFoodX, randomFoodY,0);

		GameObject foodSpawned = Instantiate(foodArray[foodToSpawn],foodLocation,transform.rotation, foodSpawnArea.transform);
		foodSpawned.transform.localPosition =  new Vector3 (foodLocation.x, foodLocation.y, 0);
		foodSpawned.GetComponent<FoodExpController> ().foodExpWorthAmount = foodExpArray [foodToSpawn];
        foodCountOnScreen++;

		if (foodToSpawn == 0) {
			foodSpawned.GetComponent<FoodExpController> ().poopCounter = 3;
		}
		if (PlayerPrefsController.soundFXState == 1) {
			audioSource.PlayOneShot (popSound, 0.25f);
		}
		Debug.Log(foodSpawned);
		if (timeBetweenSpawn <= 0) {
			timeBetweenSpawn = 3f;
		}
		if (timeBetweenPoopSpawn <= 0) {
			timeBetweenPoopSpawn = 3;
		}
	}


    private void OnDestroy()
    {
        exitTime = DateTime.Now;
        PlayerPrefs.SetString("ExitTime", exitTime.ToString());
        Debug.Log("scene change exit time is " + exitTime);

    }


}
