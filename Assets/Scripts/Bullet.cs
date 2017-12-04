using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	private float projSpeed = 135f;
	private float projDmg = 5f;

	void Start()
	{
		Destroy(gameObject, 2f);
	}

	void Update()
	{
		transform.Translate(transform.forward * projSpeed * Time.deltaTime, Space.World);
	}

	public void SetProjSpeedAndDmg(float _speed, float _dmg)
	{
		projSpeed = _speed;
		projDmg = _dmg;
	}


	void OnTriggerEnter(Collider c)
	{
		if(c.CompareTag("Player"))
		{
			Player player = c.GetComponent<Player>();

			if(player)
			{
				player.TakeDmg(projDmg);
			}

			Destroy(gameObject);
		}
	}
}
