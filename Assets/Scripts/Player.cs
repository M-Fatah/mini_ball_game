using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour 
{
	public float health = 100;
    public float rollForce = 500;
	public float coinCollectionDistance = 1;
	[HideInInspector]
	public bool dead = false;

	public event System.Action OnPlayerDeath; 
	
    private PlayerController playerController;

	void Start()
	{
		playerController = GetComponent<PlayerController>();
	}

	void Update()
	{
		if(!dead)
		{
			Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			Vector3 dir = moveInput.normalized * rollForce;
			playerController.SetForce(dir);

			Collider[] coins = Physics.OverlapSphere(transform.position, coinCollectionDistance, 1 << 9);

			for(int i = 0; i < coins.Length; i++)
			{
				coins[i].GetComponent<Coin>().SetTarget(transform);
			}
		}
	}

	public void TakeDmg(float _dmg)
	{
		health -= _dmg;

		if(health <= 0)
		{
			dead = true;
			GetComponent<PlayerController>().enabled = false;
			
			if(OnPlayerDeath != null)
			{
				OnPlayerDeath();
			}
		}
	}
}
