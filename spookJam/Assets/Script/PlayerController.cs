using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour, IHealth {

    public enum PLAYERSTATE
    {
        moving,
        firing,
        dead
    }

    public PLAYERSTATE playerState = PLAYERSTATE.moving;
    public enum PlayerNumber { PLAYER1, PLAYER2, KEYBOARDTEST };

    public enum Platform
    {
        WINDOWS,
        MAC
    };
    
    // Handling the input device
    public Platform platform;
    public PlayerNumber playerNumber;
    private string Horizontal;
    private string Vertical;
    private string HorizontalAim;
    private string VerticalAim;
    private string Fire;

    private bool hittable = true;


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

    [SerializeField]
    private float knockbackOnHit = 1000;

    private float redValue;
    private Rigidbody rigidbody;
    private GunController gunController;
    private RenderComponent playerRenderer;
    private bool goingLeft;
    private float hitInvlunerabilityTimer = 0.5f;
    public bool die;

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
            if (platform == Platform.MAC)
                Fire = "Fire1";
            else
                Fire = "Fire1Windows";
        }
        else if (playerNumber == PlayerNumber.PLAYER2)
        {
            Horizontal = "Horizontal2";
            Vertical = "Vertical2";
            HorizontalAim = "HorizontalAim2";
            VerticalAim = "VerticalAim2";
            if (platform == Platform.MAC)
                Fire = "Fire2";
            else
                Fire = "Fire2Windows";
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

	    if (die) Die();
	}

    private void ControlGun()
    {
       if (!Input.GetButton(Fire)) return;
       
        var angleFired = goingLeft ? -1:1;
        FireGun(angleFired);
           
    }

    private void FireGun(float angleFired)
    {
        if (angleFired == 0) angleFired = 1.0f;

        if (gunController.FireGun(angleFired, shootingFreezeTime))
        {
            //StartCoroutine(StartFiring());
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

        Debug.Log(zDelt);

        // Update the render of the character
        if (xDelt != 0)
            goingLeft = xDelt < 0.0f;
        

        
        bool goingUpwards = zDelt <= 0.0f;

        // Apply the new graphics
        playerRenderer.ChangeSpriteDirection(goingUpwards, goingLeft);
        
        gunController.MatchSpriteFlip(goingLeft);

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
        if (!hittable || playerState == PLAYERSTATE.dead) return;

        StartCoroutine(StartInvincibility());

        if(health >0)
            health -= damage;

        redValue = (float)health / 150.0f;

        rigidbody.AddForce(hitAngle * knockback, 0, 0);

        transform.GetComponentInChildren<SpriteRenderer>().color = new Color(1.0f, redValue, redValue);

        if (health <= 0) Die();

    }
    
    private IEnumerator StartInvincibility()
    {
        hittable = false;
        yield return new WaitForSeconds(hitInvlunerabilityTimer);
        hittable = true;
    }

    private void Die()
    {
        playerState = PLAYERSTATE.dead;
        transform.GetComponentInChildren<SpriteRenderer>().color = Color.grey;
        health = 0;
    }

    public void Revive()
    {
        playerState = PLAYERSTATE.moving;
        health = 30;
        redValue = (float)health / 150.0f;
        transform.GetComponentInChildren<SpriteRenderer>().color = new Color(1.0f, redValue, redValue);
    }

    private void OnTriggerEnter(Collider other)
    {
        var otherPlayer = other.GetComponent<PlayerController>();
        if (otherPlayer!= null && !otherPlayer.gameObject.CompareTag(this.tag))
        {
            if (otherPlayer.playerState == PLAYERSTATE.dead)
                StartCoroutine(StartRevive(otherPlayer));
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        var otherPlayer = other.GetComponent<PlayerController>();
        if (otherPlayer!= null && !otherPlayer.gameObject.CompareTag(this.tag))
        {
            if (otherPlayer.playerState == PLAYERSTATE.dead)
                StopCoroutine(StartRevive(otherPlayer));
        }
    }

    private IEnumerator StartRevive(PlayerController playerController)
    {
        yield return new WaitForSeconds(2);
        if (playerState != PLAYERSTATE.dead)
            playerController.Revive();
    }
}
