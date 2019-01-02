using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CookieGenerator))]
public class CookieGeneratorEditor : Editor
{

    public override void OnInspectorGUI()
    {

        // Default Params
        DrawDefaultInspector();

        // Reference to the script this applies to
        CookieGenerator gen = (CookieGenerator)target;

        GUILayout.Space(10);

        // Button to generator final Cookie
        if (GUILayout.RepeatButton("Show Preview"))
        {
            gen.ShowPreview();
        }
        else
        {
            gen.HidePreview();
        }

        GUILayout.Space(3);

        // Button to generator final Cookie
        if (GUILayout.Button("Generate Cookie"))
        {
            gen.GeneratorCookie();
        }

        GUILayout.Space(3);

    }

}
