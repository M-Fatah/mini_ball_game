using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{

	private Rigidbody rb;
	private Vector3 dir;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	void FixedUpdate()
	{
		Vector3 relativeDir = Camera.main.transform.parent.TransformDirection(dir);

		rb.AddForce(relativeDir * Time.fixedDeltaTime, ForceMode.Acceleration);
	}

	public void SetForce(Vector3 _dir)
	{
		dir = _dir;
	}
}
