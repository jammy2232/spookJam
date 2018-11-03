using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderComponent : MonoBehaviour
{

    public Sprite spriteToRender = null;
    public SpriteRenderer spriteRenderer;

    // Use this for initialization
    void OnEnable ()
    {

        if(!spriteToRender)
        {
            Debug.Log(gameObject.name + " has no sprite assigned on the rendercomponent.");
            return;
        }

        // Find reference the the Billboard
        Transform[] objects = GetComponentsInChildren<Transform>();

        foreach(Transform trans in objects)
        {
            if(trans.gameObject.name == "BillBoard")
            {
                // apply the msprite
                spriteRenderer = trans.gameObject.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = spriteToRender;
            }
        }

        if(spriteRenderer == null)
        {
            Debug.Log(gameObject.name + " has no Billboard Prefab attached to render it.");
        }
		
	}

}
