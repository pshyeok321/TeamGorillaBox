using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSource : MonoBehaviour {

    public static List<GameObject> laserList = new List<GameObject>();

    public bool isUpdate = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (isUpdate) {
            for (int i = 0; i < laserList.Count; i++)
                Debug.Log(laserList[i].name);

            isUpdate = false;
        }
	}
}
