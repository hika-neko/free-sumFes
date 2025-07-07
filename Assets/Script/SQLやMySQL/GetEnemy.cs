using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class Enemy
{
	public int enemy_id;		  // id
	public string enemy_kind;	  // ���
	public int attack;			  // �U����
	public float speed;			  // ���x
	public string prefab_name;	  // �v���n�u��
	public int is_boss;			  // boss��
	public string drop_item;      // �h���b�v�A�C�e��
}

[System.Serializable]
public class EnemyListWrapper
{
	public List<Enemy> enemys;
}


public class GetEnemy : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		StartCoroutine(GetEnemyData());
	}

	IEnumerator GetEnemyData()
	{
		string url = "http://localhost/Unity�A�g/get_enemys_info.php";
		UnityWebRequest www = UnityWebRequest.Get(url);
		yield return www.SendWebRequest();

		if (www.result != UnityWebRequest.Result.Success)
		{
			Debug.LogError("�ʐM���s" + www.error);
			yield break;
		}

		string json = www.downloadHandler.text;
		EnemyListWrapper data = JsonUtility.FromJson<EnemyListWrapper>(json);
		foreach (var e in data.enemys)
		{
			Debug.Log($"ID:{e.enemy_id} ���:{e.enemy_kind} " +
				$"�U����: {e.attack} ���x:{e.speed}" +
				$"�v���n�u��: {e.prefab_name} �{�X��: {e.is_boss} ���W�i: {e.drop_item}");
		}
		foreach (var e in EnemyManager.Instance.enemyList)
		{
			Debug.Log($"{e.enemy_kind}  �{�X��:{e.is_boss}");
		}
	}
}
