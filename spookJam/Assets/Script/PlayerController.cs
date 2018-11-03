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

    private PLAYERSTATE playerState = PLAYERSTATE.moving;

    [SerializeField]
    private int playerNumber = 1;

    private string joystickName;

    [SerializeField]
    private string fireButton = "10";

    [SerializeField]
    private string horizontalAxisMovement = "8";

    [SerializeField]
    private string verticalAxisMovement = "8";

    [SerializeField]
    private string horizontalAxisLook = "9";

    [SerializeField]
    private string verticalAxisLook = "9";
    

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
        joystickName = "joystick " + playerNumber + " button ";
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
        if (keyboardTestMode && !Input.GetButton("Fire1")) return;
        else if (!keyboardTestMode && !Input.GetButton(joystickName + fireButton)) return;
        
        if (keyboardTestMode)
             gunController.FireGun(new Vector3(Input.GetAxisRaw("HorizontalAim"), 0 , Input.GetAxisRaw("VerticalAim")));
        else
             gunController.FireGun(new Vector3(Input.GetAxis(joystickName + horizontalAxisLook), 0 , Input.GetAxis(joystickName + verticalAxisLook)));
    }

    //Get movement velocity
    private void GetMovement()
    {
        float xDelt;
        float zDelt;
        if (keyboardTestMode)
        {
            xDelt = Input.GetAxisRaw("Horizontal");
            zDelt = Input.GetAxisRaw("Vertical");
        }
        else
        {
            xDelt = Input.GetAxisRaw(joystickName + horizontalAxisMovement);
            zDelt = Input.GetAxisRaw(joystickName + verticalAxisMovement);
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
