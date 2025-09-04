using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : MonoBehaviour
{
	[SerializeField] GameObject arrowPrefab;
	[SerializeField] Transform firePoint;
	[SerializeField] float shootPower = 5f;
	[SerializeField] private float shootAngle = 60f;
	private int attack;
	private SpriteRenderer sr;

	void Start()
	{
		sr = GetComponent<SpriteRenderer>();
		Invoke(nameof(ShootFixedArc), 1.75f);
	}
	public void SetAttackPower(int power)
	{
		attack = power;
	}
	void ShootFixedArc()
	{
		GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
		bool isFacingfLeft = sr != null && sr.flipX;
		float angle = isFacingfLeft ? 180f - shootAngle : shootAngle;

		Arrow arrowScript = arrow.GetComponent<Arrow>();
		if (arrowScript != null)
		{
			arrowScript.InitializeWithAngle(shootPower, angle, attack, isFacingfLeft);
		}
		attack = 0;
	}
}
