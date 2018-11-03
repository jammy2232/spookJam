using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

    private enum PLAYERSTATE
    {
        moving,
        dead
    }

    private PLAYERSTATE playerState;

    [SerializeField]
    private string fireButton = "Fire1_P1";

    [SerializeField]
    private string horizontalAxisMovement = "Horizontal_P1";

    [SerializeField]
    private string verticalAxisMovement = "Vertical_P1";

    [SerializeField]
    private string horizontalAxisLook = "Joystick X_P1";

    [SerializeField]
    private string verticalAxisLook = "Joystick Y_P1";
    

    [SerializeField]
    private float moveSpeed = 1.0f;

    [SerializeField]
    private float rotSpeed = 5.0f;

    [SerializeField]
    private bool keyboardTestMode = false;

    private Rigidbody rigidbody;

    private GunController gunController;


	// Use this for initialization
	void Start ()
    {
        rigidbody = GetComponent<Rigidbody>();
        gunController = GetComponentInChildren<GunController>();
	}
	
	// Update is called once per frame
	void Update () {
        switch (playerState)
        {
            case PLAYERSTATE.moving:
                ControlGun();
                GetMovement();
                break;
            case PLAYERSTATE.dead:
                break;
        }
	}

    private void ControlGun()
    {
        if (keyboardTestMode && !Input.GetButton("FirePC")) return;
        else if (!keyboardTestMode && !Input.GetButton(fireButton)) return;
        
        if (keyboardTestMode)
             gunController.FireGun(new Vector3(Input.GetAxisRaw("MouseX"), 0 , Input.GetAxisRaw("MouseY")));
        else
             gunController.FireGun(new Vector3(Input.GetAxis(horizontalAxisLook), 0 , Input.GetAxis(verticalAxisLook)));
    }

    //Get movement velocity
    private void GetMovement()
    {
        float xDelt;
        float zDelt;
        if (keyboardTestMode)
        {
            xDelt = Input.GetAxisRaw("HorizontalPC");
            zDelt = Input.GetAxisRaw("VerticalPC");
        }
        else
        {
            xDelt = Input.GetAxisRaw(horizontalAxisMovement);
            zDelt = Input.GetAxisRaw(verticalAxisMovement);
        }

        //Adjust for rotation
        var horizMove = transform.right * xDelt;
        var vertMove = transform.forward * zDelt;

        var velocity = (horizMove + vertMove).normalized * moveSpeed;

        if (velocity != Vector3.zero)
        {
            rigidbody.MovePosition((rigidbody.position + velocity * Time.deltaTime));
        }
    }

}
