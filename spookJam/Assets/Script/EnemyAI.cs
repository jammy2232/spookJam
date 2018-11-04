using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(RenderComponent))]
public class EnemyAI : MonoBehaviour, IHealth
{

    // Events to inform different systems of thier status
    public delegate void EnemyDied();
    public static event EnemyDied OnEnemyDied;

    public enum Type { Ghost, Buster }
    public enum State { Alive, Dieing, Dead }

    private int health = 100;
    public Type type = Type.Ghost;
    public State state = State.Alive;
    public float forceMovement = 1.0f;
    public Sprite deadSprite;

    // [HideInInspector]
    public Transform Target;

    // Private
    private Rigidbody rb;
    private SpriteRenderer sr;

    // Effect to Spawn
    public GameObject effect;

    // The time the whole object should exist
    public float timeToDie;

    [SerializeField]
    private float knockback = 500;

    private bool hittable = true;

    private float hitInvlunerabilityTimer = 0.5f;

    // Use this for initialization
    void Start ()
    {

        // Check there is a valid target
        if (!Target)
        {
            Debug.Log(gameObject.name + " has no target.");
            Destroy(this.gameObject);
            return;
        }

        // Get reference to the sprite renderer
      //  sr = GetComponent<RenderComponent>().spriteRenderer;

        // Get the rigid body
        rb = GetComponent<Rigidbody>();
            
	}

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKey(KeyCode.E))
        {
            state = State.Dieing;
        }

        switch (state)
        {
            case State.Alive:

                if(health < 0)
                {
                    state = State.Dieing;
                }

                Move();

                break;
            case State.Dieing:
             //   sr.sprite = deadSprite;
                rb.isKinematic = true;
                GetComponent<CapsuleCollider>().enabled = false;;
                StartCoroutine(Die());
                state = State.Dead;
                break;
            case State.Dead:
                break;
        }

    }

    void Move()
    {

        // Simple Follow Script
        if (type == Type.Ghost)
        {
            Vector3 directionToTarget = Target.position - transform.position;
            rb.AddForce(directionToTarget.normalized * forceMovement);
        }
        else
        {
            // Ghost Busters don't float
            Vector3 directionToTarget = Target.position - transform.position;
            directionToTarget.y = 0.0f;
            rb.AddForce(directionToTarget.normalized * forceMovement);
        }

    }


    public IEnumerator Die()
    {

        // Instanciate the particle system
        GameObject particle = Instantiate(effect);
        // Place it in the correct position
        particle.transform.position = transform.position;
        // Send an event that the enemy has dies
        if (OnEnemyDied != null)
            OnEnemyDied();
        // Wait for the animation to play
        yield return new WaitForSeconds(timeToDie);
        // Delete the objects once thier dead
        Destroy(gameObject);
        Destroy(particle);
    }

    public void TakeDamage(int damage, float hitAngle)
    {
        if (!hittable) return;
        health -= damage;
        rb.AddForce(hitAngle*knockback, 0 , 0);
        
    }

    private IEnumerator StartInvincibility()
    {
        hittable = false;
        yield return new WaitForSeconds(hitInvlunerabilityTimer);
        hittable = true;
    }

}
