using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnOnLoadController : MonoBehaviour {

    


    // Use this for initialization
    void Start () {

    }


    // Update is called once per frame
    void Update () {
        CheckLastPlayTime();
	}



    void CheckLastPlayTime()
    {
        DateTime date1, date2;
        DateTime parsedDate1, parsedDate2;
        string stringParsedDate1, stringParsedDate2;

        date1 = new DateTime(2017, 1, 26, 8, 0, 0);
        date2 = new DateTime(2018, 1, 27, 8, 0, 0);
        stringParsedDate1 = date1.ToString();
        stringParsedDate2 = date2.ToString();

        parsedDate1 = DateTime.Parse(stringParsedDate1);
        parsedDate2 = DateTime.Parse(stringParsedDate2);

        TimeSpan difference1 = parsedDate1.Subtract(parsedDate2);
        Debug.Log("24hr difference date1 - date2 = " + difference1);
        Debug.Log(difference1.TotalSeconds + " seconds");
    }


}
