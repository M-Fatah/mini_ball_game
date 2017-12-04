using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
	public float coinSpeed = 5;
	public int coinScore = 10;
	public AudioClip coinClip;

	private Transform target;

	public event System.Action<int> OnCoinCollected;

	void Update()
	{
		if(target)
		{
			transform.Translate((target.position - transform.position).normalized * coinSpeed * Time.deltaTime);
		}
	}


	void OnTriggerEnter(Collider c)
	{
		if(c.CompareTag("Player"))
		{
			SoundsManager.instance.PlaySound(coinClip, transform.position);
			// SoundsManager.instance.PlaySFX(coinClip);

			if(OnCoinCollected != null)
			{
				OnCoinCollected(coinScore);
			}

			Destroy(gameObject);
		}
	}

	public void SetTarget(Transform _target)
	{
		target = _target;
	}
}
