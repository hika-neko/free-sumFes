using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderAttack : MonoBehaviour
{
	private int damage = 1;
	[SerializeField] GameObject effect;
	private GameObject markerToDestroy;

	public void SetMarker(GameObject marker)
	{
		markerToDestroy = marker;
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

			Destroy(gameObject); // 落雷は単発攻撃なので消える
		}
		else if (other.CompareTag("Ground"))
		{
			// 地面に落ちたら消える（エフェクトあれば出してもいい
			if(effect != null)
			{
				GameObject fx = Instantiate(effect,transform.position,Quaternion.identity);
				ThunderEffect fxScript = fx.GetComponent<ThunderEffect>();
				if (fxScript != null)
				{
					fxScript.SetMarker(markerToDestroy);
				}
			}
			Destroy(gameObject);
		}
	}
}
