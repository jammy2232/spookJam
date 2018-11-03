using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    [SerializeField]
    private float bulletSpeed = 500.0f;

    [SerializeField]
    private string TargetTag;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void FireBullet(Vector3 angle)
    {
        StartCoroutine(WaitTillFire(angle));
    }

    private IEnumerator WaitTillFire(Vector3 angle)
    {
        yield return new WaitForEndOfFrame();
        
        GetComponent<Rigidbody>().AddForce(angle * bulletSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
