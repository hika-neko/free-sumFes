using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class Enemy
{
	public int enemy_id;		  // id
	public string enemy_kind;	  // 種類
	public int attack;			  // 攻撃力
	public float speed;			  // 速度
	public string prefab_name;	  // プレハブ名
	public int is_boss;			  // bossか
	public string drop_item;      // ドロップアイテム
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
		string url = "http://localhost/Unity連携/get_enemys_info.php";
		UnityWebRequest www = UnityWebRequest.Get(url);
		yield return www.SendWebRequest();

		if (www.result != UnityWebRequest.Result.Success)
		{
			Debug.LogError("通信失敗" + www.error);
			yield break;
		}

		string json = www.downloadHandler.text;
		EnemyListWrapper data = JsonUtility.FromJson<EnemyListWrapper>(json);
		foreach (var e in data.enemys)
		{
			Debug.Log($"ID:{e.enemy_id} 種類:{e.enemy_kind} " +
				$"攻撃力: {e.attack} 速度:{e.speed}" +
				$"プレハブ名: {e.prefab_name} ボスか: {e.is_boss} 収集品: {e.drop_item}");
		}
		foreach (var e in EnemyManager.Instance.enemyList)
		{
			Debug.Log($"{e.enemy_kind}  ボスか:{e.is_boss}");
		}
	}
}
