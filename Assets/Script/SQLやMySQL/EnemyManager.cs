using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyManager : MonoBehaviour
{
	public static EnemyManager Instance;
	public List<Enemy> enemyList = new List<Enemy>();

	void Awake()
	{
		Instance = this;
	}

	void Start()
	{
		StartCoroutine(GetEnemyFromServer());
		int stageId = PlayerPrefs.GetInt("Player_Level");
		StartCoroutine(WaitAndGetEnemies());
	}

	IEnumerator GetEnemyFromServer()
	{
		using (UnityWebRequest www = UnityWebRequest.Get("http://localhost/Unity連携/get_enemys_info.php"))
		{
			yield return www.SendWebRequest();

			if (www.result != UnityWebRequest.Result.Success)
			{
				Debug.LogError("データ取得失敗: " + www.error);
			}
			else
			{
				string json = www.downloadHandler.text;
				EnemyListWrapper wrapper = JsonUtility.FromJson<EnemyListWrapper>(json);
				enemyList = wrapper.enemy;
				Debug.Log("enemyを " + enemyList.Count + " 件取得");
			}
		}
	}
	private IEnumerator WaitAndGetEnemies()
	{
		// stage_idがセットされるのを待つ
		yield return new WaitUntil(() => PlayerPrefs.HasKey("Player_Level"));
		yield return new WaitForSeconds(0.1f); // 念のため少し待つ

		int stageId = PlayerPrefs.GetInt("Player_Level");
		Debug.Log("ロードするPlayer_Level: " + stageId);
		StartCoroutine(GetStageEnemies(stageId));
	}

	public IEnumerator GetStageEnemies(int stageId)
	{
		string url = $"http://localhost/Unity連携/get_stage_enemys.php?stage_id={stageId}";
		UnityWebRequest www = UnityWebRequest.Get(url);
		yield return www.SendWebRequest();

		if (www.result != UnityWebRequest.Result.Success)
		{
			Debug.LogError("敵データ取得失敗: " + www.error);
			yield break;
		}

		string json = www.downloadHandler.text;
		Debug.Log(json);
		EnemyListWrapper wrapper = JsonUtility.FromJson<EnemyListWrapper>(json);
		enemyList = wrapper.enemy;
		Debug.Log($"ステージ{stageId}の敵を {enemyList.Count} 件取得");
	}
}
