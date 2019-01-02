using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    [SerializeField]
    private float bulletSpeed = 500.0f;

    private string targetTag;
	private int damage;
	private float hitAngle;
	

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void FireBullet(float angle, int dmg, string targetEnemy, float maxRandomVariation)
    {
	    hitAngle = angle;
	    damage = dmg;
	    targetTag = targetEnemy;
        StartCoroutine(WaitTillFire(angle, maxRandomVariation));
    }

    private IEnumerator WaitTillFire(float angle, float maxRandomVariation)
    {
        yield return new WaitForEndOfFrame();
	    var random = Random.Range(-maxRandomVariation, maxRandomVariation);
	    var force = transform.right * angle * bulletSpeed;
        GetComponent<Rigidbody>().AddForce(new Vector3 (force.x, force.y, force.z + random));
    }

    private void OnTriggerEnter(Collider other)
    {

	    if (other.gameObject.CompareTag(targetTag))
	    {
            IHealth obj = other.gameObject.GetComponent<IHealth>();

            if(obj != null)
            {
                obj.TakeDamage(damage, hitAngle);
            }
            else
            {
                Debug.Log(gameObject.name + " Attempted to hit with " + other.gameObject.name + " but not IHealth");
            }

		    StartCoroutine(DestroyBullet());

	    }
	    else if (other.gameObject.CompareTag(Tags.WallTag))
	    {
		   StartCoroutine(DestroyBullet());

        }
    }

	private IEnumerator DestroyBullet()
	{
		yield return null;
		Destroy(this.gameObject);
	}

}
