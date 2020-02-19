using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrumpyController : MonoBehaviour {

	Animator trumpyAnimator;
	public GameObject Trumpy;
	public GameObject[] TrumpyArray = new GameObject[16];
	SpriteRenderer trumpySpriteRenderer;
	public Canvas trumpyCanvas;

	public GameObject emptyGameObject;          // parent that trumpy is spawned under
    public Material solidWhiteMaterial;         // solid white color for evolution

	Vector3 mouseWorldPos3D;
	Vector3 foodLocation;
	GameObject foodToEat;
	bool isMovingToFood;
	bool clickedFood;
	public float moveSpeed;
	public AudioClip munchSound;
	AudioSource audioSource;
	public GameObject manager;
	Vector3 center = new Vector3 (0,0,0);
	Vector3 lastLocation;

    float previousTrumpyX, previousTrumpyY, currentTrumpyX, currentTrumpyY;
    float previousDeltaX, previousDeltaY, currentDeltaX, currentDeltaY;
    float deltaToMoveTrumpyX, deltaToMoveTrumpyY;
    public float timeToSpawnTrumpy;
    bool Xdirection;
    public bool isEvolving;

    // Use this for initialization
    void Start () {
		Debug.Log ("TRUMPYCONTROLLER trumpy level is " + manager.GetComponent<ExperienceController>().currentLevel);
		if (PlayerPrefsController.hasPlayedBefore == true) {
			lastLocation.x = PlayerPrefs.GetFloat ("LastLocationX");
			lastLocation.y = PlayerPrefs.GetFloat ("LastLocationY");
			lastLocation.z = PlayerPrefs.GetFloat ("LastLocationZ");
			Debug.Log ("lastlocation loaded is = " + lastLocation);
		}

		Trumpy = Instantiate (TrumpyArray [manager.GetComponent<ExperienceController>().currentLevel],lastLocation, gameObject.transform.rotation, emptyGameObject.transform);
		Trumpy.transform.localPosition = lastLocation;

		audioSource = gameObject.GetComponent<AudioSource> ();
		trumpyAnimator = Trumpy.GetComponent<Animator> ();
		trumpySpriteRenderer = Trumpy.GetComponent<SpriteRenderer> ();
	}



    public void LeveledUp(){
		lastLocation = Trumpy.transform.localPosition;
        Debug.Log("current location before level up is " + lastLocation);
		Xdirection = Trumpy.GetComponent<SpriteRenderer> ().flipX;
        GetPreviousDeltaPositions();

        trumpySpriteRenderer.material = solidWhiteMaterial;
        isEvolving = true;

        trumpyAnimator.SetBool("isEvolving", true);
        gameObject.GetComponent<ExperienceController>().ActivateEvolvingTextBox();

        Trumpy.transform.localPosition = new Vector3(0, 0, 0);

        GameObject oldTrumpy = Trumpy;

        Invoke("InstantiateNewTrumpy", timeToSpawnTrumpy);
        Destroy(oldTrumpy, 2f);
	}

    void InstantiateNewTrumpy()
    {
        Debug.Log("currently spawning");
        Trumpy = Instantiate(TrumpyArray[manager.GetComponent<ExperienceController>().currentLevel], new Vector3 (0,0,0), gameObject.transform.rotation, emptyGameObject.transform);
        trumpyAnimator = Trumpy.GetComponent<Animator>();
        trumpySpriteRenderer = Trumpy.GetComponent<SpriteRenderer>();
        trumpySpriteRenderer.flipX = Xdirection;
        GetCurrentDeltaPositions();
        Invoke("RepositionNewTrumpy", 1f);
    }

    void RepositionNewTrumpy()
    {
        Trumpy.transform.localPosition = lastLocation;

        // deltaToMoveTrumpyY will stay positive as long as previousDeltaY < or = currentDeltaY
        deltaToMoveTrumpyY = Mathf.Abs(previousDeltaY - currentDeltaY);         

        if (previousDeltaY > currentDeltaY)
        {
            // move bottom edge of current/new trumpy DOWN to be at same position of previous trumpy
            // need to make deltaToMoveTrumpyY a negative number
            deltaToMoveTrumpyY = -1 * deltaToMoveTrumpyY;
        }


        //  code to change current/new trumpy location based on delta positions
        Trumpy.transform.localPosition = new Vector3(lastLocation.x, lastLocation.y + deltaToMoveTrumpyY, 0);
    }

    void GetCurrentDeltaPositions()
    {
        currentTrumpyX = Trumpy.GetComponent<RectTransform>().rect.width;
        currentDeltaX = currentTrumpyX * Trumpy.GetComponent<RectTransform>().pivot.x * Trumpy.GetComponent<RectTransform>().localScale.x;
        currentTrumpyY = Trumpy.GetComponent<RectTransform>().rect.height;
        currentDeltaY = currentTrumpyY * Trumpy.GetComponent<RectTransform>().pivot.y * Trumpy.GetComponent<RectTransform>().localScale.y;
    }


    void GetPreviousDeltaPositions()
    {
        previousTrumpyX = Trumpy.GetComponent<RectTransform>().rect.width;
        previousDeltaX = previousTrumpyX * Trumpy.GetComponent<RectTransform>().pivot.x * Trumpy.GetComponent<RectTransform>().localScale.x;
        previousTrumpyY = Trumpy.GetComponent<RectTransform>().rect.height;
        previousDeltaY = previousTrumpyY * Trumpy.GetComponent<RectTransform>().pivot.y * Trumpy.GetComponent<RectTransform>().localScale.y;
    }




	public void SaveLastLocation(){
		lastLocation = Trumpy.transform.localPosition;
		PlayerPrefs.SetFloat ("LastLocationX", lastLocation.x);
		PlayerPrefs.SetFloat ("LastLocationY", lastLocation.y);
		PlayerPrefs.SetFloat ("LastLocationZ", lastLocation.z);
		Debug.Log ("lastlocation saved options button = " + lastLocation);
	}

	// Update is called once per frame
	void Update () {
        if (isEvolving != true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                mouseWorldPos3D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mouseWorldPos3D.x, mouseWorldPos3D.y);
                Vector2 dir = Vector2.zero;

                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, dir);

                if (hit.collider != null)
                {
                    if (hit.collider.tag == "food" || hit.collider.tag == "poop")
                    {
                        //					Debug.Log ("clicked poop");
                        clickedFood = true;
                        foodToEat = hit.collider.gameObject;
                    }
                }
            }
            if (Input.GetMouseButtonUp(0) && clickedFood == true)
            {
                //			MoveTrumpyToFood (food);
                foodLocation = foodToEat.transform.position;
                isMovingToFood = true;
                clickedFood = false;
            }

            // CODE TO MOVE TRUMPY TO FOOD POSITION
            if (isMovingToFood == true)
            {
                trumpyAnimator.SetBool("isWalking", true);
                if (Trumpy.transform.position.x > foodToEat.transform.position.x)
                {
                    trumpySpriteRenderer.flipX = true;
                }
                else
                {
                    trumpySpriteRenderer.flipX = false;
                }

                float trumpyAcceleration = moveSpeed * Time.deltaTime;
                Trumpy.transform.position = Vector3.MoveTowards(Trumpy.transform.position, foodLocation, trumpyAcceleration);
            }

            // CODE ONCE TRUMPY HAS REACHED FOOD POSITION
            if (foodToEat != null && Trumpy.transform.position == foodToEat.transform.position)
            {
                if (PlayerPrefsController.soundFXState == 1)
                {
                    audioSource.PlayOneShot(munchSound, 0.25f);
                }
                emptyGameObject.GetComponent<Animator>().SetBool("isEating", true);
                manager.GetComponent<ExperienceController>().UpdateExp(foodToEat.GetComponent<FoodExpController>().foodExpWorthAmount);
                Destroy(foodToEat);
                manager.GetComponent<FoodSpawnController>().foodCountOnScreen--;
                isMovingToFood = false;
                trumpyAnimator.SetBool("isWalking", false);
                StartCoroutine(StopEatingAnim());
            }
        }
	}

	IEnumerator StopEatingAnim(){
		yield return new WaitForSeconds (0.133f);
		emptyGameObject.GetComponent<Animator>().SetBool("isEating", false);


	}
    
	void OnApplicationQuit(){
		Debug.Log ("game has quit");
		SaveLastLocation ();
	}


}
