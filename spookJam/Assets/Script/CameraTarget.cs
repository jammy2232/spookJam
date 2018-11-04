using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
	// Use this for initialization
	void OnEnable ()
    {
        billBoardRendering.targetLook = this.transform;
	}
}
