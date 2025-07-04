using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform king;
    [SerializeField] 

	private void Update()
	{
		transform.position =
			new Vector3(
				king.position.x + 2,
				king.position.y + 2,
				-10);
	}
}
