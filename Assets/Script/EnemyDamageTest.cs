using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageTest : MonoBehaviour
{
	private EnemyAnimator enemyAnimator;

	void Start()
	{
		enemyAnimator = GetComponent<EnemyAnimator>();
		Invoke("SimulateHit", 2f); // 2秒後に自動でダメージ処理
	}

	void SimulateHit()
	{
		Debug.Log("敵がダメージを受けました！");
		enemyAnimator.PlayHit();
	}
}