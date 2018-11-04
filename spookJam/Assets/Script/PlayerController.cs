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
    public enum PlayerNumber { PLAYER1, PLAYER2, KEYBOARDTEST };

    // Handling the input device
    public PlayerNumber playerNumber;
    private string Horizontal;
    private string Vertical;
    private string HorizontalAim;
    private string VerticalAim;
    private string Fire;

    // Setting the speed for movement
    [SerializeField]
    private float moveSpeed = 1.0f;
 
    // private variables

    [SerializeField]
    private float shootingFreezeTime = 0.05f;

    [SerializeField]
    private int health = 150;

    [SerializeField]
    private float knockback = 10;
    
    private Rigidbody rigidbody;
    private GunController gunController;
    private RenderComponent playerRenderer;

	// Use this for initialization
	void Start ()
    {
        rigidbody = GetComponent<Rigidbody>();
        gunController = GetComponentInChildren<GunController>();
        playerRenderer = GetComponent<RenderComponent>();

        // Setup the player input
        if(playerNumber == PlayerNumber.PLAYER1)
        {
            Horizontal = "Horizontal1";
            Vertical = "Vertical1";
            HorizontalAim = "HorizontalAim1";
            VerticalAim = "VerticalAim1";
            Fire = "Fire1";
        }
        else if (playerNumber == PlayerNumber.PLAYER2)
        {
            Horizontal = "Horizontal2";
            Vertical = "Vertical2";
            HorizontalAim = "HorizontalAim2";
            VerticalAim = "VerticalAim2";
            Fire = "Fire2";
        }
        else
        {
            Horizontal = "Horizontal";
            Vertical = "Vertical";
            HorizontalAim = "KeyboardHorizontalAim";
            VerticalAim = "KeyboardVerticalAim";
            Fire = "KeyboardFire";
        }

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

       if (Input.GetAxis(Fire) < 0.2f) return;
       
        var angleFired = Input.GetAxis(HorizontalAim);
        FireGun(angleFired);
           
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

        float xDelt = 0.0f;
        float zDelt = 0.0f;

        xDelt = Input.GetAxis(Horizontal);
        zDelt = Input.GetAxis(Vertical);

        // Update the render of the character
        bool goingUpwards = false;
        bool goingLeft = false;

        if (xDelt < 0.0f)
        {
            goingLeft = true;
        }

        if (zDelt <= 0.0f)
        {
            goingUpwards = true;
        }

        // Apply the new graphics
        playerRenderer.ChangeSpriteDirection(goingUpwards, goingLeft);

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
