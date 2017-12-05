using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour 
{
	public Transform turretBody;
	public Transform turretGunPoint;
	public float rotationAngle = 135f;
	public float detectionRadius = 2f;
	[Space(10)]
	public Bullet bulletPrefab;
	public float projDmg = 5f;
	public float projSpeed = 10f;
	public float timeBetweenShots = 0.5f;
	public AudioClip gunShotClip;
	[Space(10)]
    public LayerMask raycastMask;
	

	private float msTimeBetweenShots;
	private bool playerGotDetected = false;
	private float physicsSimulationTime = 1f;

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

	void FixedUpdate()
	{
        if(turretGunPoint)
		{
			// Coverage area for player detection.
			Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, raycastMask);
			
			if(colliders.Length > 0)
			{
				Player p = colliders[0].GetComponent<Player>();
				Rigidbody pRigidBody = p.GetComponent<Rigidbody>();

			    Vector3 predictedPos = pRigidBody.position + (pRigidBody.velocity * Time.fixedDeltaTime * 10 * (9 / projSpeed));

				
                // Rotating the turret towards the predicted position of the ball.
                Vector3 dirToLookAt = predictedPos - turretBody.position;
				dirToLookAt.y = 0;				
				turretBody.rotation = Quaternion.LookRotation(dirToLookAt);

                // Do the shooting.
                if(Time.time >= msTimeBetweenShots)
				{
					if(!p.dead)
					{
						SoundsManager.instance.PlaySound(gunShotClip, turretGunPoint.position);
						Bullet bullet = (Bullet)Instantiate(bulletPrefab, turretGunPoint.position, turretGunPoint.rotation);
						bullet.SetProjSpeedAndDmg(projSpeed, projDmg);
						msTimeBetweenShots = Time.time + timeBetweenShots;
					}
				}
			}
		}
	}
}
