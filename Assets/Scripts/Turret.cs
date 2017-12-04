using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour 
{
	public Transform turretBody;
	public Transform turretGunPoint;
	public float rotationAngle = 135f;
    public float smoothTime = 30f;
	public float laserLength = 10f;
	[Space(10)]
	public Bullet bulletPrefab;
	public float projDmg = 5f;
	public float projSpeed = 135f;
	public float timeBetweenShots = 0.5f;
	public AudioClip gunShotClip;
	[Space(10)]
    public LayerMask raycastMask;
	

	private float msTimeBetweenShots;
	private bool playerGotDetected = false;

	void Start()
	{
		// Reading proj speed from a file.
		if(LoadDataFromFile.instance.IsReady())
		{
			int bSpeed = LoadDataFromFile.instance.GetGridElement("projectileSpeed");

			// Seting the proj speed to default value if the returned value read from a file is null or 0.
			projSpeed = (bSpeed > 0)? bSpeed : projSpeed;
		}

	}

	void Update()
	{
		
		if(!playerGotDetected)
		{
			// Coverage area arc.
        	float angle = Mathf.PingPong(Time.time * smoothTime, rotationAngle) - (rotationAngle / 2);
			
			Quaternion targetRot = Quaternion.Euler(new Vector3(turretBody.eulerAngles.x, angle + transform.eulerAngles.y, turretBody.eulerAngles.z));

			turretBody.rotation = Quaternion.RotateTowards(turretBody.rotation, targetRot, Time.deltaTime * smoothTime);
		}

        if(turretGunPoint)
		{
			RaycastHit hit;
			Ray ray = new Ray(turretGunPoint.position, turretGunPoint.forward);
			
			// Laser beam.
			LineRenderer lineRenderer = GetComponent<LineRenderer>();
			lineRenderer.positionCount = 2;
			lineRenderer.SetPosition(0, ray.origin);
			lineRenderer.SetPosition(1, ray.origin + (turretGunPoint.forward * laserLength));
			
			// Coverage area for player detection.
			playerGotDetected = false;
			if(Physics.Raycast(ray, out hit, laserLength, raycastMask))
			{
				playerGotDetected = true;
				
				// Rotating the turret towards the player.
				Vector3 dirToLookAt = hit.collider.transform.position - turretBody.position;
				dirToLookAt.y = 0;				
				turretBody.rotation = Quaternion.LookRotation(dirToLookAt);

				// Do the shooting.
                if(Time.time >= msTimeBetweenShots)
				{
					if(!hit.collider.GetComponent<Player>().dead)
					{
						SoundsManager.instance.PlaySound(gunShotClip, turretGunPoint.position);
						//SoundsManager.instance.PlaySFX(gunShotClip);
						Bullet bullet = (Bullet)Instantiate(bulletPrefab, turretGunPoint.position, turretGunPoint.rotation);
						bullet.SetProjSpeedAndDmg(projSpeed, projDmg);
						msTimeBetweenShots = Time.time + timeBetweenShots;
					}
				}
			}
		}
	}
}
