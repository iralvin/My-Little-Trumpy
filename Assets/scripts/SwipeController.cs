using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeController : MonoBehaviour {


	bool isSwiping = false;
	public GameObject swipeTrail;
	public float minSwipingVelocity = 0.001f;

	Vector2 previousPosition;
	Rigidbody2D rb;
	GameObject currentSwipeTrail;
	CircleCollider2D circleCollider;
	public GameObject managerObject;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		circleCollider = GetComponent<CircleCollider2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			StartSwiping ();
		} else if (Input.GetMouseButtonUp (0)) {
			StopSwiping ();
		}


		if (isSwiping) {
			UpdateSwipePath ();
		}
	}


	void UpdateSwipePath(){
		Vector2 newPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		rb.position = newPosition;

		float velocity = (newPosition - previousPosition).magnitude * Time.deltaTime;
		if (velocity > minSwipingVelocity) {
            currentSwipeTrail.SetActive(true);
			circleCollider.enabled = true;

		} else { 
			circleCollider.enabled = false;
		}
		previousPosition = newPosition;

	}

	void StartSwiping(){
		isSwiping = true;
		currentSwipeTrail = Instantiate (swipeTrail, transform);
        currentSwipeTrail.SetActive(false);
		previousPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		circleCollider.enabled = false;
	}


	void StopSwiping(){
		isSwiping = false;
		currentSwipeTrail.transform.SetParent (null);
		Destroy (currentSwipeTrail, 1f);
		circleCollider.enabled = false;

	}



	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "poop") {
			Debug.Log ("SWIPED AT POOP*****");
			int poopCount = other.gameObject.GetComponent<FoodExpController> ().poopCounter;
			Vector3 lastPoopLocation = other.gameObject.transform.position;
			if (poopCount == 3) {
				Destroy (other.gameObject);
				GameObject poop2 = Instantiate (
					                   managerObject.GetComponent<FoodSpawnController> ().poop2,
					                   lastPoopLocation,
					                   gameObject.transform.rotation,
					managerObject.GetComponent<FoodSpawnController> ().foodSpawnArea.transform);
				poop2.GetComponent<FoodExpController> ().poopCounter = 2;
				Debug.Log (poop2 + " )))) " + poop2.GetComponent<FoodExpController> ().poopCounter);
			}
			else if (poopCount == 2) {
				Destroy (other.gameObject);
				GameObject poop1 = Instantiate (
					managerObject.GetComponent<FoodSpawnController> ().poop1,
					lastPoopLocation,
					gameObject.transform.rotation,
					managerObject.GetComponent<FoodSpawnController> ().foodSpawnArea.transform);
				poop1.GetComponent<FoodExpController> ().poopCounter = 1;
				Debug.Log (poop1 + " )))) " + poop1.GetComponent<FoodExpController> ().poopCounter);

			}
			else if (poopCount == 1) {
				Destroy(other.gameObject);
				managerObject.GetComponent<FoodSpawnController> ().foodCountOnScreen--;
			}
		}
	}
}
