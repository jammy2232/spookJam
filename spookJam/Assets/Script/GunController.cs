using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private float fireRate;

    [SerializeField]
    private float maxRandomVariation;

	[SerializeField]
	private int damage;

	[SerializeField]
	private int burstSize;
	
	[SerializeField]
	private string targetEnemyType;
	
    private float timeSinceFire;

    private bool readyToFire;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        TrackFireCooldown();
	}

    public bool FireGun(float angleFired, float shootingFreezeTime)
    {
        if (!readyToFire) return false;
        readyToFire = false;
        timeSinceFire = 0;

	    var waitTime = shootingFreezeTime / burstSize;
	    
	    
		StartCoroutine(SpawnBullet(angleFired, waitTime));

	    return true;
    }

	private IEnumerator SpawnBullet(float angleFired, float waitTime)
	{
		for (int i = 0; i < burstSize; i++)
		{
			yield return new WaitForSeconds(waitTime);
			GameObject bullet;
			bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
			bullet.GetComponent<Bullet>().FireBullet(angleFired, damage, targetEnemyType, maxRandomVariation);
		}
	}

	private void TrackFireCooldown()
    {
        if (readyToFire) return;
        timeSinceFire += Time.deltaTime;
        readyToFire = timeSinceFire >= fireRate;
    }
}
