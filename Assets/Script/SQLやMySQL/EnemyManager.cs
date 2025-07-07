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
				enemyList = wrapper.enemys;
				Debug.Log("enemy�� " + enemyList.Count + " ���擾");
			}
		}
	}

	//public IEnumerator UnlockFighterOnServer(int fighterId)
	//{
	//	string url = "http://localhost/Unity�A�g/update_fighter_unlock.php";
	//	WWWForm form = new WWWForm();
	//	//form.AddField("king_id", king_id);
	//	form.AddField("fighter_id", fighterId);

	//	using (UnityWebRequest www = UnityWebRequest.Post(url, form))
	//	{
	//		yield return www.SendWebRequest();
	//		if (www.result != UnityWebRequest.Result.Success)
	//		{
	//			Debug.LogError("�ʐM���s " + www.error);
	//		}
	//		else
	//		{
	//			Debug.Log("�������: " + www.downloadHandler.text);
	//		}
	//	}
	//}
}
