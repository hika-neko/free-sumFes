using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageTest : MonoBehaviour
{
	private EnemyAnimator enemyAnimator;

	void Start()
	{
		enemyAnimator = GetComponent<EnemyAnimator>();
		Invoke("SimulateHit", 2f); // 2�b��Ɏ����Ń_���[�W����
	}

	void SimulateHit()
	{
		Debug.Log("�G���_���[�W���󂯂܂����I");
		enemyAnimator.PlayHit();
	}
}