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
		int stageId = 1;
		StartCoroutine(GetStageEnemies(stageId));
	}

	IEnumerator GetEnemyFromServer()
	{
		using (UnityWebRequest www = UnityWebRequest.Get("http://localhost/Unity�A�g/get_enemys_info.php"))
		{
			yield return www.SendWebRequest();

			if (www.result != UnityWebRequest.Result.Success)
			{
				Debug.LogError("�f�[�^�擾���s: " + www.error);
			}
			else
			{
				string json = www.downloadHandler.text;
				EnemyListWrapper wrapper = JsonUtility.FromJson<EnemyListWrapper>(json);
				enemyList = wrapper.enemy;
				Debug.Log("enemy�� " + enemyList.Count + " ���擾");
			}
		}
	}
	public IEnumerator GetStageEnemies(int stageId)
	{
		string url = $"http://localhost/Unity�A�g/get_stage_enemys.php?stage_id={stageId}";
		UnityWebRequest www = UnityWebRequest.Get(url);
		yield return www.SendWebRequest();

		if (www.result != UnityWebRequest.Result.Success)
		{
			Debug.LogError("�G�f�[�^�擾���s: " + www.error);
			yield break;
		}

		string json = www.downloadHandler.text;
		EnemyListWrapper wrapper = JsonUtility.FromJson<EnemyListWrapper>(json);
		enemyList = wrapper.enemy;
		Debug.Log($"�X�e�[�W{stageId}�̓G�� {enemyList.Count} ���擾");
	}
}
