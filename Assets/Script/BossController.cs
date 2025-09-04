using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
	[Header("ステータス")]
	private int maxHp = 10;
	private int currentHp;

	[Header("攻撃設定")]
	public float attackInterval = 3.0f;
	private float attackTimer = 0f;

	private Animator animator;
	[SerializeField] GameObject thunder;
	[SerializeField] GameObject thunderMarker;
	[SerializeField] float delayThunder = 1.5f;
	private int glideAttackDamage = 1;
	private bool isGliding = false;
	private bool isAttacking = false;
	void Start()
	{
		currentHp = maxHp;
		animator = GetComponent<Animator>();
	}

	void Update()
	{
		attackTimer += Time.deltaTime;
		if (attackTimer >= attackInterval && !isAttacking)
		{
			attackTimer = 0f;
			DecideNextAction();
		}
	}
	void DecideNextAction()
	{
		isAttacking = true;
		int attackType = Random.Range(0, 2); // 0:雷撃, 1:滑空突進

		if (attackType == 0)
			StartCoroutine(PerformThunderStrike());
		else
			StartCoroutine(PerformGlideAttack());

		isAttacking = false;
	}
	IEnumerator PerformThunderStrike()
	{
		Debug.Log("落雷攻撃");

		if (animator != null)
		{
			animator.SetTrigger("Attack");
		}
		SpawnLightning();
		yield return new WaitForSeconds(attackInterval);
	}
	public void SpawnLightning()
	{
		Vector3 playerPos = GameObject.FindWithTag("Player").transform.position;
		float spawnHeight = 10f;
		Vector3 spawnThunder = new Vector3(playerPos.x, playerPos.y + spawnHeight, 0f);
		Vector3 strikeThunder = new Vector3(playerPos.x, playerPos.y, 0f);
		GameObject marker = Instantiate(thunderMarker, strikeThunder, Quaternion.identity);
		GameObject thunderObject = Instantiate(thunder, spawnThunder, Quaternion.identity);
		thunderObject.GetComponent<ThunderAttack>().SetMarker(marker);
	}

	IEnumerator PerformGlideAttack()
	{
		Debug.Log("突進攻撃");

		Transform player = GameObject.FindWithTag("Player").transform;
		Vector3 playerPos = player.position;
		Vector3 bossPos = transform.position;

		float glideHeight = 4f;     // 出発点の高さ
		float glideDistance = 6f;   // プレイヤー前後の距離

		Vector3 startPos, endPos;
		if (playerPos.x > bossPos.x)

		{
			startPos = new Vector3(playerPos.x - glideDistance, playerPos.y + glideHeight, 0f);
			endPos = new Vector3(playerPos.x + glideDistance, playerPos.y + glideHeight, 0f);
		}
		else
		{
			startPos = new Vector3(playerPos.x + glideDistance, playerPos.y + glideHeight, 0f);
			endPos = new Vector3(playerPos.x - glideDistance, playerPos.y + glideHeight, 0f);
		}
		// 移動時間
		float duration = 1.5f;
		float time = 0f;

		gameObject.tag = "Invisible";
		isGliding = true;
		while (time < duration)
		{
			time += Time.deltaTime;
			float t = time / duration;

			// 水平方向
			Vector3 horizontal = Vector3.Lerp(startPos, endPos, t);

			// 放物線的な降下 → 上昇
			float verticalOffset = -2f * Mathf.Sin(Mathf.PI * t);
			Vector3 pos = new Vector3(horizontal.x, horizontal.y + verticalOffset, 0f);

			transform.position = pos;

			yield return null;
		}
		isGliding = false;
		gameObject.tag = "Enemy";
		yield return new WaitForSeconds(attackTimer);
	}
	public void TakeDamage(int damage)
	{
		currentHp -= damage;
		Debug.Log($"ボスが {damage} ダメージを受けた（残りHP: {currentHp}）");

		if (currentHp <= 0)
		{
			Die();
		}
	}

	void Die()
	{
		Debug.Log("ボス撃破！");
		// animator.SetTrigger("Die");

		// 撃破後に消す、演出入れるなど
		Destroy(gameObject, 2f);

		SceneSwitch.Instance.LoadModeScene(PhaseManager.Phase.Castle, "Forest");
	}
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (isGliding && other.CompareTag("Player"))
		{
			Debug.Log("突進ヒット！");
			KingMovement player = other.GetComponent<KingMovement>();
			if (player != null)
			{
				player.TakeDamage(glideAttackDamage);
			}
		}
	}
}
