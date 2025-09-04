using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
	Rigidbody2D rb2D;
	private int damage;
	SpriteRenderer SpR;
	void Awake()
	{
		SpR = GetComponent<SpriteRenderer>();
		rb2D = GetComponent<Rigidbody2D>();
		if (rb2D == null)
		{
			Debug.LogError("Rigidbody2Dがアタッチされていません！");
		}
	}
	private void Update()
	{
		if(transform.position.y <= -10)
		{
			Destroy(gameObject);
		}
	}

	public void InitializeWithAngle(float power, float angleDeg, int dmg, bool sr)
	{
		damage = dmg;
		SpR.flipX = sr;
		float angleRad = angleDeg * Mathf.Deg2Rad;
		Vector2 force = new Vector2(
			Mathf.Cos(angleRad),
			Mathf.Sin(angleRad)
		) * power;

		rb2D.AddForce(force, ForceMode2D.Impulse);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Enemy"))
		{
			EnemyStatus enemy = other.GetComponent<EnemyStatus>();
			if (enemy != null)
			{
				enemy.TakeDamage(damage);
				Destroy(gameObject);
			}
		}
		if (other.CompareTag("Boss"))
		{
			BossController enemy = other.GetComponent<BossController>();
			if (enemy != null)
			{
				enemy.TakeDamage(damage);
				Destroy(gameObject);
			}
		}
		if (other.CompareTag("Enemy"))
		{
			EnemyStatus enemy = other.GetComponent<EnemyStatus>();
			if (enemy != null)
			{
				enemy.TakeDamage(damage);
				Destroy(gameObject);
			}
		}
		if (other.CompareTag("Ground"))
		{
			Destroy(gameObject);
		}
	}
}
