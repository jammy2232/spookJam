using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(RenderComponent))]
public class EnemyAI : MonoBehaviour
{
    public enum Type { Ghost, Buster }

    public enum State { Alive, Dieing, Dead }

    public Type type = Type.Ghost;
    public State state = State.Alive;
    public float forceMovement = 1.0f;

    public DeathAnimation death;
    public Sprite deadSprite;

    [HideInInspector]
    public Transform Target;

    // Private
    private Rigidbody rb;
    private SpriteRenderer sr;

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
        sr = GetComponent<RenderComponent>().spriteRenderer;

        // Get the rigid body
        rb = GetComponent<Rigidbody>();
            
	}

    // Update is called once per frame
    void Update()
    {

        switch (state)
        {
            case State.Alive:
                Move();
                break;
            case State.Dieing:
                sr.sprite = deadSprite;
                Destroy(rb);
                Destroy(GetComponent<CapsuleCollider>());
                death.Execute(transform.position, gameObject);
                Destroy(this);
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

}
