using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FighterManager : MonoBehaviour
{
	public static FighterManager Instance;
	public List<Fighter> fighterList = new List<Fighter>();
	public HashSet<int> unlockedFighter = new HashSet<int>();

	void Awake()
	{
		if(Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	void Start()
	{
		StartCoroutine(GetFightersFromServer());
	}

	public void SetFighterList(List<Fighter> list)
	{
		fighterList = list;
	}
	public List<Fighter> GetFighterList()
	{
		return fighterList;
	}

	IEnumerator GetFightersFromServer()
	{
		using (UnityWebRequest www = UnityWebRequest.Get("http://localhost/Unity˜AŒg/get_fighters_info.php"))
		{
			yield return www.SendWebRequest();

			if (www.result != UnityWebRequest.Result.Success)
			{
				Debug.LogError("ƒf[ƒ^æ“¾¸”s: " + www.error);
			}
			else
			{
				string json = www.downloadHandler.text;
				FighterListWrapper wrapper = JsonUtility.FromJson<FighterListWrapper>(json);
				fighterList = wrapper.fighter;
				Debug.Log("fighter‚ğ " + fighterList.Count + " Œæ“¾");
			}
		}

		foreach (var id in unlockedFighter)
		{
			Debug.Log("‰ğ•úÏ‚İID: " + id);
		}
	}

	public IEnumerator UnlockFighterOnServer(int king_id, int fighter_id)
	{
		Debug.Log("UnlockkFighterOnServer‚É“ü‚Á‚½‚æ");
		string url = "http://localhost/Unity˜AŒg/update_fighter_unlock.php";
		WWWForm form = new WWWForm();
		Debug.Log($"king_id: {king_id}, fighter_id: {fighter_id}");

		form.AddField("king_id", king_id);
		form.AddField("fighter_id", fighter_id);

		using (UnityWebRequest www = UnityWebRequest.Post(url,form))
		{
			yield return www.SendWebRequest();
			if(www.result != UnityWebRequest.Result.Success)
			{
				Debug.LogError("’ÊM¸”s " + www.error);
			}
			else
			{
				Debug.Log("‰ğ•ú¬Œ÷: " + www.downloadHandler.text);
			}
		}
	}
}
