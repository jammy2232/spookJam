using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {

    static GameObject instance;

	// Use this for initialization
	void Start () {

        DontDestroyOnLoad(gameObject);

        if(instance == null)
        {
            instance = this.gameObject;
        }
        else
        {
            DestroyImmediate(this.gameObject);
        }

    }
	
}
