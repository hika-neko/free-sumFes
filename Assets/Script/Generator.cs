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
	private float commonerSpeed = 3f;

	[SerializeField] int maxSpawnCount = 10; // �����ɑ��݂ł���ő吔
	private int currentSpawnCount = 0;
	void Start()
	{
		// �e��SpriteRenderer���擾
		parentRenderer = GetComponentInParent<SpriteRenderer>();

		// �ŏ��̃��[�J���ʒu��ۑ��i�E������Ԃ���j
		baseLocalPos = transform.localPosition;
	}
	private void Update()
	{
		if (parentRenderer == null) return;

		// flipX�ɉ����ă��[�J��X�𔽓]
		Vector3 newPos = baseLocalPos;
		newPos.x = parentRenderer.flipX ? baseLocalPos.x : -baseLocalPos.x;
		transform.localPosition = newPos;
	}
	public void Spawner(bool isFacingLeft, int level, int index)
	{
		if(currentSpawnCount + level > maxSpawnCount)
		{
			Debug.Log("�����������ɂ�蒆�f�I");
			return;
		}
		Fighter data = null;
		if(index != 0)
		{
			data = FighterManager.Instance.fighterList.Find(f => f.fighter_id == index);
			if(data == null)
			{
				Debug.LogWarning("Fighter�f�[�^��������܂���: id=" + index);
				return;
			}
		}

		for (int i = 0; i < level; i++)
		{
			// �����ʒu���炵�Đ����i���ɕ��ׂ��j
			Vector3 offset = new Vector3(i * 0.5f * (isFacingLeft ? -1 : 1), 0, 0);

			GameObject prefabToSpawn;

			if (index == 1)
			{
				// commoners[] ���烉���_���I��
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
				if(index == 0)
				{
					mover.SetSpeed(commonerSpeed);
					mover.SetLifetime(5);
				}
				else
				{
					mover.SetSpeed(data.speed);
					mover.SetLifetime(4);
				}
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