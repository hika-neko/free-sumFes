using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
[System.Serializable]
public class StageInfo
{
	public string stage_name;
	public int boss_appearNum;
}
public class StageInfoLoader : MonoBehaviour
{
	public EnemySpawn enemySpawn; // Inspectorでアサイン or 自動取得する

	void Start()
	{
		int stageId = PlayerPrefs.GetInt("Player_Level"); // ← 第二引数はデフォルト値（なかったとき用）
		Debug.Log("Player_Levelは: "+stageId);
		StartCoroutine(GetStageInfoCoroutine(stageId));
	}

	IEnumerator GetStageInfoCoroutine(int id)
	{
		string url = $"http://localhost/Unity連携/get_stage_info.php?stage_id={id}";

		using (UnityWebRequest www = UnityWebRequest.Get(url))
		{
			yield return www.SendWebRequest();

			if (www.result != UnityWebRequest.Result.Success)
			{
				Debug.LogError("ステージ情報取得失敗: " + www.error);
				yield break;
			}

			string json = www.downloadHandler.text;
			StageInfo stageInfo = JsonUtility.FromJson<StageInfo>(json);

			Debug.Log($"ステージ名: {stageInfo.stage_name}, ボス出現数: {stageInfo.boss_appearNum}");

			if (enemySpawn != null)
			{
				enemySpawn.boss_appear_num = stageInfo.boss_appearNum;
				EnemyDeathCount.Instance.SetBossAppearNum(stageInfo.boss_appearNum);
			}
			else
			{
				Debug.LogWarning("enemySpawn がアサインされていません！");
			}
		}
	}
}
