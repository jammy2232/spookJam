using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class billBoardRendering : MonoBehaviour {

    // 
    Camera cameraRef;

	// Use this for initialization
	void Start ()
    {

        // Store the reference to the camera 
        cameraRef = Camera.main;
		
	}
	
	// Update is called once per frame
	void Update ()
    {

        // Make the bill board face the camera
        transform.LookAt(cameraRef.transform);
        transform.Rotate(transform.up, Mathf.Deg2Rad * 180.0f);

	}
}
