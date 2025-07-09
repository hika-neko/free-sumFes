using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
	public int currentHp;
	private EnemyAnimator enemyAnimator;
	private EnemyMover enemyMover;
	void Awake()
	{
		enemyMover = GetComponent<EnemyMover>();
		enemyAnimator = GetComponent<EnemyAnimator>();
	}

	public void Initialize(int hp)
	{
		currentHp = hp;
	}

	public void TakeDamage(int damage)
	{
		currentHp -= damage;
		if(currentHp <= 0)
		{
			enemyAnimator.PlayDeath();
			enemyMover.SetMoveSpeed(0);
			this.gameObject.tag = "Untagged";
		}
		else 
		{
			enemyAnimator.PlayHit();
		}
	}
}
