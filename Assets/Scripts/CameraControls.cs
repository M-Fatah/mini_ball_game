using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
	public Transform player;
	public float offsetDistance = 5f;
	public float rotationSpeed = 65f;
    public float yMin = -20f;
    public float yMax = 80f;

	private float xRot;
    private float yRot;

	private Quaternion rotation;

	void Start()
	{
		xRot = transform.eulerAngles.x;
		yRot = transform.eulerAngles.y;
		yRot = Utils.ClampAngle(yRot, yMin, yMax);
    }

    void LateUpdate()
    {
		if(player)
		{
			if (Input.GetMouseButton(0))
			{
				xRot += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
				yRot -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
				yRot = Utils.ClampAngle(yRot, yMin, yMax);
			}

			transform.parent.rotation = Quaternion.Euler(0, xRot, 0);
			rotation = Quaternion.Euler(yRot, transform.parent.eulerAngles.y, 0);
			
			Vector3 position = rotation * Vector3.forward * -offsetDistance + player.position;


			transform.rotation = rotation;
			transform.position = position;

		}
    }

}
