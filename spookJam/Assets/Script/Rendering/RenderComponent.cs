using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Render Component controls the 2D quad direction vector relative to the camera to create the illusion of 2D characters in the 3D environment.
public class RenderComponent : MonoBehaviour
{

    // Inspector Interface *********************

    [Header("Sprites")]
    public Sprite backSprite = null;
    public Sprite frontSprite = null;
    public Material renderMaterial = null;

    [Header("BillBoard Setup")]
    public Vector3 localPlacement;

    [Header("Bob")]
    public float bobAmplitude = 0.0f;
    public float bobFrequency = 0.0f;

    //******************************************


    // Private data ****************************

    private SpriteRenderer spriteRenderer;

    //******************************************


    // Use this for initialization
    void OnEnable()
    {

        // Check the sprites are valid 
        if (!frontSprite || !backSprite)
        {
            Debug.LogError(gameObject.name + " has no sprites assigned to either of the front or back on the rendercomponent.");
        }

        // Validate the render material 
        if(!renderMaterial)
        {
            Debug.LogError(gameObject.name + " has no renderer material on the rendercomponent.");
        }

        // Add the billboard component required
        CreateBillboard();

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


    void Update()
    {

        // apply the bobing As required
        spriteRenderer.gameObject.transform.position += new Vector3(0.0f, bobAmplitude * Mathf.Sin(Time.fixedTime * bobFrequency), 0.0f);

    }


    // This function creates the billboard component required for the RenderComponent and returns it's sprite renderer
    void CreateBillboard()
    {

        // Create a new game object and apply all the components required with the correct settings
        GameObject billboard = new GameObject();
        billboard.name = "billboard";
        billboard.transform.SetParent(transform);
        billboard.transform.localPosition = localPlacement;

        // Add the BillBoarding component and Sprite Rendering 
        billboard.AddComponent<BillBoardRendering>();
        spriteRenderer = billboard.AddComponent<SpriteRenderer>();

        // Add the sprites assuming the sprite is facing right and front
        spriteRenderer.sprite = frontSprite;
        spriteRenderer.material = renderMaterial;
        spriteRenderer.flipX = true;

    }

}

