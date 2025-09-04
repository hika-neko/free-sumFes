using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
	private void Update()
	{
		if(transform.position.y <= -10)
		{
			Destroy(transform.parent.gameObject);
		}
	}
	private void OnTriggerEnter2D(Collider2D other) 
	{
		if(other.CompareTag("Player"))
		{
			KingMovement king = other.GetComponent<KingMovement>();
			if (king != null) 
			{
				king.TakeDamage(1);
			}
		}
	}
}
