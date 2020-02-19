using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyController : MonoBehaviour {

    private static DontDestroyController instance;
    public GameObject manager;

	// Use this for initialization
	void Awake () {

        if (instance != null)
        {
            Destroy(gameObject);

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
    
}
