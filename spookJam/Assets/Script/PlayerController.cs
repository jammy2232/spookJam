using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour, IHealth {

    private enum PLAYERSTATE
    {
        moving,
        firing,
        dead
    }

    private PLAYERSTATE playerState = PLAYERSTATE.moving;

    [SerializeField]
    private int playerNumber = 1;

    private string joystickName;


    private string fireButton = "10";


    private string horizontalAxisMovement = "8";


    private string verticalAxisMovement = "8";


    private string horizontalAxisLook = "9";


    private string verticalAxisLook = "9";
    

    [SerializeField]
    private float moveSpeed = 1.0f;

    [SerializeField]
    private float shootingFreezeTime = 0.05f;
    
    [SerializeField]
    private bool keyboardTestMode = false;

    [SerializeField]
    private int health = 150;

    [SerializeField]
    private float knockback = 10;
    
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
        {
            var angleFired =  Input.GetAxisRaw("HorizontalAim");
            FireGun(angleFired);
        }
        else
        {
            var angleFired = Input.GetAxis(joystickName + horizontalAxisLook);
            FireGun(angleFired);
            
        }
    }

    private void FireGun(float angleFired)
    {
        if (angleFired == 0) angleFired = 1.0f;

        if (gunController.FireGun(angleFired, shootingFreezeTime))
        {
            StartCoroutine(StartFiring());
            rigidbody.AddForce(-angleFired*knockback, 0, 0);
        }
    }

    private IEnumerator StartFiring()
    {
        playerState = PLAYERSTATE.firing;
        yield return new WaitForSeconds(shootingFreezeTime);

        if (health > 0)
            playerState = PLAYERSTATE.moving;
        else
            playerState = PLAYERSTATE.dead;

    }
    
    //Get movement velocity
    private void GetMovement()
    {
        float xDelt;
        float zDelt;
        if (keyboardTestMode)
        {
            xDelt = Input.GetAxis("Horizontal");
            zDelt = Input.GetAxis("Vertical");
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

    public void TakeDamage(int damage, float hitAngle)
    {
        health -= damage;
    }

}
