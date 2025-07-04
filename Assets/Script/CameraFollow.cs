using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform king;
	private SpriteRenderer sr;

	private void Start()
	{
		sr = king.GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		transform.position =
			new Vector3(
				sr.flipX ? king.position.x - 2 : king.position.x + 2,
				king.position.y + 2,
				-10);
	}
}
