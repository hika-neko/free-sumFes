using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderAttack : MonoBehaviour
{
	private int damage = 1;
	[SerializeField] GameObject effect;
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			KingMovement king = other.GetComponent<KingMovement>();
			if (king != null)
			{
				king.TakeDamage(damage);
			}

			Destroy(gameObject); // 落雷は単発攻撃なので消える
		}
		else if (other.CompareTag("Ground"))
		{
			// 地面に落ちたら消える（エフェクトあれば出してもいい
			if(effect != null)
			{
				Instantiate(effect,transform.position,Quaternion.identity);
			}
			Destroy(gameObject);
		}
	}
}
