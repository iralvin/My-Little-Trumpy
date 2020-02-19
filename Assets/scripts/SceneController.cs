using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;


public class SceneController : MonoBehaviour {


	public void sceneselect (string sceneName)
	{
        SceneManager.LoadScene (sceneName);
	}



}
