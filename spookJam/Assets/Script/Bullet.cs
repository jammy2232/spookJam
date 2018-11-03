using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    [SerializeField]
    private float bulletSpeed = 500.0f;

    private string targetTag;

	private int damage;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void FireBullet(Vector3 angle, int dmg, string targetEnemy)
    {
	    damage = dmg;
	    targetTag = targetEnemy;
        StartCoroutine(WaitTillFire(angle));
    }

    private IEnumerator WaitTillFire(Vector3 angle)
    {
        yield return new WaitForEndOfFrame();
        
        GetComponent<Rigidbody>().AddForce(angle * bulletSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
	    if (other.gameObject.CompareTag(targetTag))
	    {
		    gameObject.GetComponent<IHealth>().TakeDamage(damage);
		    DestroyBullet();
	    }
	    else if (other.gameObject.CompareTag(Tags.WallTag))
	    {
		    DestroyBullet();
	    }
    }

	private void DestroyBullet()
	{
		Destroy(this);
	}
}
