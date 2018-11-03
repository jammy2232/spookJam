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

    public void FireGun(Vector3 angleFired)
    {
        if (!readyToFire) return;
        readyToFire = false;
        timeSinceFire = 0;

        GameObject bullet;
        bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);

        if (angleFired == Vector3.zero) angleFired = Vector3.right;

        float variation = Random.Range(-maxRandomVariation, maxRandomVariation);
        var angle = new Vector3(angleFired.x + variation, 0,  angleFired.z + variation);
        bullet.GetComponent<Bullet>().FireBullet(angle, damage, targetEnemyType);
    }

    private void TrackFireCooldown()
    {
        if (readyToFire) return;
        timeSinceFire += Time.deltaTime;
        readyToFire = timeSinceFire >= fireRate;
    }
}
