using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderEffect : MonoBehaviour
{
	private int damage = 1;
	public float lifeTime = 0.5f;
	private void Start()
	{
		Destroy(gameObject, lifeTime);
	}
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			KingMovement king = other.GetComponent<KingMovement>();
			if (king != null)
			{
				king.TakeDamage(damage);
			}

			Destroy(gameObject); // ——‹‚Í’P”­UŒ‚‚È‚Ì‚ÅÁ‚¦‚é
		}
	}
}
