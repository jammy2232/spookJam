using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoardRendering : MonoBehaviour 
{

    // This is the static target look
    public new static Transform camera;

    private void Start()
    {
        // Work out the vector direction to face for all the billboards
        Vector3 direction = -camera.forward;
        Vector3 up = camera.up;

        // Set billboard rotation to match this 
        transform.rotation = Quaternion.LookRotation(direction, up);
    }

}
