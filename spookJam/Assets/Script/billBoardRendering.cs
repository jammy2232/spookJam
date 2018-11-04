using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class billBoardRendering : MonoBehaviour 
{

    // This is the static target look
    public static Transform targetLook;
	
	// Update is called once per frame
	void Update ()
    {
        // Make the bill board face the camera
        transform.LookAt(targetLook);
        transform.Rotate(transform.up, Mathf.Deg2Rad * 180.0f);
	}

}
