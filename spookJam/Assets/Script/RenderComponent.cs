using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderComponent : MonoBehaviour
{

    [Header("Sprites")]
    public Sprite backSprite = null;
    public Sprite frontSprite = null;

    [Header("Bob")]
    public float bobAmplitude = 0.0f;
    public float bobFrequency = 0.0f;

    [HideInInspector]
    private SpriteRenderer spriteRenderer;

    // Use this for initialization
    void OnEnable()
    {

        // Check that sprites have been assigned
        if (!frontSprite || !backSprite)
        {
            Debug.Log(gameObject.name + " has no sprites assigned on the rendercomponent.");
            return;
        }

        // Find reference the the Billboard Component
        Transform[] objects = GetComponentsInChildren<Transform>();

        foreach (Transform trans in objects)
        {
            if (trans.gameObject.name == "BillBoard")
            {
                // apply a sprite assuming the player is facing forward 
                spriteRenderer = trans.gameObject.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = frontSprite;
            }
        }

        // If the component is attached but there is no corressponding billboard prefab
        if (spriteRenderer == null)
        {
            Debug.Log(gameObject.name + " has no Billboard Prefab attached to render it.");
        }

    }

    // This function allows requests to change the direction of the sprite based on movement 
    public void ChangeSpriteDirection(bool GoingUpwards, bool GoingLeft)
    {

        if (GoingUpwards)
        {
            // Assign the correct direction sprite
            spriteRenderer.sprite = frontSprite;
        }
        else
        {
            // Assign the correct direction sprite
            spriteRenderer.sprite = backSprite;
        }

        // if flip it
        if (!GoingLeft)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;

    }


    private void Update()
    {

        // apply the bobing 
        spriteRenderer.gameObject.transform.position += new Vector3(0.0f, bobAmplitude * Mathf.Sin(Time.fixedTime * bobFrequency), 0.0f);

    }

}