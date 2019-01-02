using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

[RequireComponent(typeof(Light))]
public class CookieGenerator : MonoBehaviour
{

    public enum Quality { LOW, MEDIUM, HIGH };

    // Customisations of the Tool
    [Header("Cookie Setting")]
    public bool useSpot = false;
    public float depth = 100.0f;
    [Tooltip("How white to ignore as an object")]
    [Range(0.0f, 1.0f)]
    public float threshholdLevel = 0.9f;
    [Header("Output Setting")]
    public string cookieName = "default";
    public Quality resolutionSetting = Quality.LOW;

    // Private Variables to control the state of the GUI
    private bool showPreview_ = false;

    // Private Variables for internal operation
    private new Camera camera;
    private new Light light;

    // Get a reference to the light Object
    private void Reset()
    {
        // Get the light component 
        light = GetComponent<Light>();
    }

    // Function shows a Preview by adding a camera object
    public void ShowPreview()
    {
        if(!showPreview_)
        {
            AddCamera();
            showPreview_ = true;
        }
    }

    public void HidePreview()
    {
        if(showPreview_)
        {
            RemoveCamera();
            showPreview_ = false;
        }
    }


    // Function which generates a cookie based on the current settings by adding a camera to the object
    public void GeneratorCookie()
    {

        // Check the light is the correct tpye to generate the cookie
        if(light.type != LightType.Spot)
        {
            Debug.Log("Light is not a spot light");
            return;
        }

        // update the name
        if(cookieName == "default")
            cookieName = gameObject.name;

        Vector2Int resolution = GetResolution();

        // Create a render texture to store the result
        RenderTexture texture = new RenderTexture(resolution.x, resolution.y, 32);
        texture.Create();

        // Add a Camera Component and setup the new component
        AddCamera();
        camera.targetTexture = texture;
       
        // Render the texture
        camera.Render();

        // Set the current render target
        RenderTexture.active = texture;

        // Extract the render texture and save the result
        Texture2D tex = new Texture2D(texture.width, texture.height);
        tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
        tex.Apply();

        // Format the texture 
        FormatTexture(tex);

        // reset the render target
        RenderTexture.active = null;

        // Check if a folder already exists and if not create it
        string folderGUID = CheckAndCreateFolder();

        // Encode the texture for use outwith Unity
        byte[] data = tex.EncodeToPNG();

        // create the file name
        string name = Application.dataPath + "/GeneratedCookies/" + cookieName + ".png";

        // Save this asset to the folder
        System.IO.File.WriteAllBytes(name, data);

        // Remove the camera and other temporary assets
        texture.Release();
        RemoveCamera();
        DestroyImmediate(texture);

        // Setup and get the cookie image
        AssetDatabase.ImportAsset("Assets/GeneratedCookies/" + cookieName + ".png");
        Texture2D cookie = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/GeneratedCookies/" + cookieName + ".png", typeof(Texture2D));

        if (!cookie)
        {
            Debug.LogError("Cookie failed to create");
            return;
        }

        // Apply the cookie to the light
        light.cookie = cookie;

        // inform the user
        Debug.Log("Texture Created: " + name);

        // Delete the componenet
        DestroyImmediate(this);

    }


    // Lookup the resolution for the setting
    Vector2Int GetResolution()
    {

        switch(resolutionSetting)
        {
            case Quality.LOW:
                return new Vector2Int(256, 256);
                break;
            case Quality.MEDIUM:
                return new Vector2Int(512, 512);
                break;
            case Quality.HIGH:
                return new Vector2Int(1024, 1024);
                break;
        }

        // Default
        return new Vector2Int(256, 256);

    }


    string CheckAndCreateFolder()
    {

        // Attempted to get the folder and if there is nothing create a new one
        if(AssetDatabase.IsValidFolder("Assets/GeneratedCookies"))
        {
            return AssetDatabase.AssetPathToGUID("Assets/GeneratedCookies");
        }

        return AssetDatabase.CreateFolder("Assets", "GeneratedCookies");

    }


    Camera AddCamera()
    {

        // Check if a camera still exists
        if(camera)
        {
            DestroyImmediate(camera);
        }

        // Add a new camera object
        camera = gameObject.AddComponent<Camera>() as Camera;
        camera.clearFlags = CameraClearFlags.Color;
        camera.hideFlags = HideFlags.HideInHierarchy;
        camera.backgroundColor = Color.white;

        if (useSpot)
            camera.farClipPlane = light.range;
        else
            camera.farClipPlane = depth;

        camera.fieldOfView = light.spotAngle;
        return camera;

    }


    void RemoveCamera()
    {
        if(camera)
            DestroyImmediate(camera);
    }


    // Go through and process each pixel
    void FormatTexture(Texture2D texture)
    {
        for (int y = 0; y < texture.width; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {

                // Get the colour 
                Color c = texture.GetPixel(x, y);
                
                // Process the pixel colour to determine what to store
                if(c.r < threshholdLevel && c.b < threshholdLevel && c.g < threshholdLevel)
                {
                    c = Color.white;
                    c.a = 0.0f;
                }
                else
                {
                    c = Color.black;
                    c.a = 1.0f;
                }

                // Set the final colour
                texture.SetPixel(x, y, c);

            }
        }

    }


}

#endif