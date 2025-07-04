using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class MoneyManager : MonoBehaviour
{
	public static MoneyManager Instance { get; private set; }

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject); // �V���O���g������
			return;
		}
		Instance = this;
		DontDestroyOnLoad(gameObject); // �V�[�����܂����ł��ێ�
	}

	public void ChangeMoney(int kingId, int delta)
	{
		StartCoroutine(ChangeMoneyOnServer(kingId, delta));
	}

	private IEnumerator ChangeMoneyOnServer(int kingId, int delta)
	{
		WWWForm form = new WWWForm();
		form.AddField("king_id", kingId);
		form.AddField("delta", delta);

		using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/Unity�A�g/update_money.php", form))
		{
			yield return www.SendWebRequest();

			if (www.result != UnityWebRequest.Result.Success)
			{
				Debug.LogError("Money update failed: " + www.error);
			}
			else
			{
				Debug.Log("Money updated: " + www.downloadHandler.text);
			}
		}
	}
}