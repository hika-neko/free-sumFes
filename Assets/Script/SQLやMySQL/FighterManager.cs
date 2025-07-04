using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FighterManager : MonoBehaviour
{
	public static FighterManager Instance;
	public List<Fighter> fighterList = new List<Fighter>();

	void Awake()
	{
		Instance = this;
	}

	void Start()
	{
		StartCoroutine(GetFightersFromServer());
	}

	IEnumerator GetFightersFromServer()
	{
		using (UnityWebRequest www = UnityWebRequest.Get("http://localhost/Unity�A�g/get_fighters_info.php"))
		{
			yield return www.SendWebRequest();

			if (www.result != UnityWebRequest.Result.Success)
			{
				Debug.LogError("�f�[�^�擾���s: " + www.error);
			}
			else
			{
				string json = www.downloadHandler.text;
				FighterListWrapper wrapper = JsonUtility.FromJson<FighterListWrapper>(json);
				fighterList = wrapper.fighter;
				Debug.Log("fighter�� " + fighterList.Count + " ���擾");
			}
		}
	}

	public IEnumerator UnlockFighterOnServer(int fighterId)
	{
		string url = "http://localhost/Unity�A�g/update_fighter_unlock.php";
		WWWForm form = new WWWForm();
		//form.AddField("king_id", king_id);
		form.AddField("fighter_id", fighterId);

		using (UnityWebRequest www = UnityWebRequest.Post(url,form))
		{
			yield return www.SendWebRequest();
			if(www.result != UnityWebRequest.Result.Success)
			{
				Debug.LogError("�ʐM���s " + www.error);
			}
			else
			{
				Debug.Log("�������: " + www.downloadHandler.text);
			}
		}
	}
}
