using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Generator : MonoBehaviour
{
	[SerializeField] GameObject[] commoners;
	[SerializeField] GameObject[] fighters;

	private SpriteRenderer parentRenderer;
	private Vector3 baseLocalPos;

	[SerializeField] int maxSpawnCount = 10; // 同時に存在できる最大数
	private int currentSpawnCount = 0;
	void Start()
	{
		// 親のSpriteRendererを取得
		parentRenderer = GetComponentInParent<SpriteRenderer>();

		// 最初のローカル位置を保存（右向き状態を基準）
		baseLocalPos = transform.localPosition;
	}
	private void Update()
	{
		if (parentRenderer == null) return;

		// flipXに応じてローカルXを反転
		Vector3 newPos = baseLocalPos;
		newPos.x = parentRenderer.flipX ? baseLocalPos.x : -baseLocalPos.x;
		transform.localPosition = newPos;
	}
	public void Spawner(bool isFacingLeft, int amount, int index)
	{
		if(currentSpawnCount + amount > maxSpawnCount)
		{
			Debug.Log("生成数制限により中断！");
			return;
		}
		for (int i = 0; i < amount; i++)
		{
			// 少し位置ずらして生成（横に並べる例）
			Vector3 offset = new Vector3(i * 0.5f * (isFacingLeft ? -1 : 1), 0, 0);

			GameObject prefabToSpawn;

			if (index == 0)
			{
				// commoners[] からランダム選択
				int rand = Random.Range(0, commoners.Length);
				prefabToSpawn = commoners[rand];
			}
			else
			{
				int trueIndex = index - 1;
				prefabToSpawn = fighters[trueIndex];
			}

			GameObject obj = Instantiate(prefabToSpawn, transform.position + offset, Quaternion.identity, transform);
			Mover mover = obj.GetComponent<Mover>();
			if (mover != null)
			{
				mover.SetDirection(isFacingLeft ? Vector2.left : Vector2.right);
				mover.SetGenerator(this);
			}
			SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
			if (sr != null)
			{
				sr.flipX = isFacingLeft;
			}

			currentSpawnCount++;
		}
	}
	public void DecreaseSpawnCount()
	{
		currentSpawnCount--;
	}
}