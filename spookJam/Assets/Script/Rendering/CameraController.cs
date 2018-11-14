using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

	// Use this for initialization
	void OnEnable ()
    {
        BillBoardRendering.camera = transform;
	}
	
}
