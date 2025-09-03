using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;


public class LoginUI : MonoBehaviour
{
	public static bool IsLoginUIActive { get; private set; } = false;

	void OnEnable()
	{
		IsLoginUIActive = true;
	}

	void OnDisable()
	{
		IsLoginUIActive = false;
	}

	[SerializeField] private GameObject addKingPanel;
	[SerializeField] private GameObject loginPanel;

	[SerializeField] private TMP_InputField addNameInput;
	[SerializeField] private TextMeshProUGUI kingIdText;
	[SerializeField] private Button submitAddButton;
	[SerializeField] private Button toggleLoginButton;

	[SerializeField] private TMP_InputField loginNameInput;
	[SerializeField] private TMP_InputField loginIdInput;
	[SerializeField] private Button submitLoginButton;
	[SerializeField] private Button toggleAddButton;

	[SerializeField] private List<Selectable> selectablesElement;
	[SerializeField] private Image selector;

	public int kingLevel;
	private int selectedIndex = 0;
	private bool isCreating = false;
	private bool isLogin = false;
	private enum Mode
	{
		None,
		AddKing,
		Login,
	}
	private Mode currentMode = Mode.AddKing;
	private void Start()
	{
		kingIdText.gameObject.SetActive(false);
		SwitchToMode(Mode.AddKing);
		SelectCurrent();
	}

	private void Update()
	{
		if (selectablesElement == null || selectablesElement.Count == 0) return;

		int kingId = PlayerPrefs.GetInt("king_id");
		StartCoroutine(GetUnlockedFighters(kingId));

		if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.Tab))
		{
			selectedIndex = (selectedIndex - 1 + selectablesElement.Count) % selectablesElement.Count;
			SelectCurrent();
		}
		else if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			selectedIndex = (selectedIndex + 1) % selectablesElement.Count;
			SelectCurrent();
		}
		else if(Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
		{
			ActivateSelected();
		}
	}
	private void SelectCurrent()
	{
		var obj = selectablesElement[selectedIndex].gameObject;
		EventSystem.current.SetSelectedGameObject(obj);

		if(selector != null)
		{
			selector.rectTransform.position = obj.transform.position;
		}
	}

	private void ActivateSelected()
	{
		var current = selectablesElement[selectedIndex];
		if (current is TMP_InputField inputField) 
		{
			inputField.ActivateInputField();
		}
		else if(current is Button button)
		{
			button.onClick.Invoke();
		}
	}

	public void SwitchToAdd()
	{
		SwitchToMode(Mode.AddKing);
	}
	public void SwitchToLogin()
	{
		SwitchToMode(Mode.Login);
	}
	private void SwitchToMode(Mode mode)
	{
		currentMode = mode;
		addKingPanel.SetActive(mode == Mode.AddKing);
		loginPanel.SetActive(mode == Mode.Login);
		if(mode == Mode.AddKing)
		{
			selectablesElement = new List<Selectable>()
			{
				addNameInput,
				submitAddButton,
				toggleLoginButton
			};
		}
		else if (mode == Mode.Login)
		{
			selectablesElement = new List<Selectable>()
			{
				loginNameInput,
				submitLoginButton,
				toggleAddButton,
				loginIdInput,
			};
		}
		selectedIndex = 0;
		SelectCurrent();
	}

	public void OnClickConfirm()
	{
		if (currentMode == Mode.AddKing)
		{
			string name = addNameInput.text.Trim();
			if (string.IsNullOrEmpty(name))
			{
				Debug.Log("名前を入力してください");
				return;
			}
			StartCoroutine(CreateNewKing(name));
		}
		else if (currentMode == Mode.Login)
		{
			string name = loginNameInput.text.Trim();
			string id = loginIdInput.text.Trim();
			// 1) 空チェック
			if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(id))
			{
				Debug.Log("名前とIDを入力してください");
				return;
			}

			// 2) 数値チェック
			if (!int.TryParse(id, out int kingId))
			{
				Debug.Log($"IDは数字で入力してください: '{id}'");
				return;
			}

			StartCoroutine(LoginKing(name, id));
		}
	}
	IEnumerator CreateNewKing(string name)
	{
		if (isCreating) yield break; // 2重呼び出し防止
		isCreating = true;

		WWWForm form = new WWWForm();
		form.AddField("king_name", name);

		UnityWebRequest www = UnityWebRequest.Post("http://localhost/Unity連携/add_king.php", form);
		yield return www.SendWebRequest();

		if (www.result != UnityWebRequest.Result.Success)
		{
			Debug.LogError("データ取得失敗: " + www.error);
		}
		else
		{
			var result = JsonUtility.FromJson<KingInfo>(www.downloadHandler.text);
			kingIdText.gameObject.SetActive(true);
			kingIdText.text = $"あなたのidは {result.king_id.ToString()}";
			yield return new WaitForSeconds(3f);
			kingIdText.gameObject.SetActive(false);
			GameObject player = GameObject.FindWithTag("Player");
			if(player != null)
			{
				KingMovement move = player.GetComponent<KingMovement>();
				move.IsMoveEnabled = true;
			}
			//Debug.Log("新規作成: king_id = " + result.king_id);
			//Debug.Log("入力された名前: " + addNameInput.text);
			gameObject.SetActive(false);
			PlayerPrefs.SetInt("king_id", result.king_id);
			PlayerPrefs.SetInt("Player_Level", result.level);
			PlayerPrefs.Save();
		}
		isCreating = false;
	}
	IEnumerator LoginKing(string name, string id)
	{
		if (isLogin) yield break; // 2重呼び出し防止
		isLogin = true;
		WWWForm form = new WWWForm();
		int kingId;
		if (!int.TryParse(id, out kingId))
		{
			Debug.LogError("IDの変換に失敗しました: " + id);
			yield break;
		}
		form.AddField("king_id", id);
		form.AddField("king_name", name);

		using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/Unity連携/login_king.php",form))
		{
			yield return www.SendWebRequest();

			if(www.result != UnityWebRequest.Result.Success)
			{
				Debug.LogError("通信エラー: " + www.error);
			}
			else
			{
				string response = www.downloadHandler.text;

				if(response == "not_found")
				{
					Debug.Log("ログイン失敗: 情報が一致しない");
				}
				else if(response == "db_error" || response == "invalid_input")
				{
					Debug.LogWarning("サーバー側エラー: " + response);
				}
				else
				{
					Debug.Log("ログイン成功: " + response);

					KingInfo info = JsonUtility.FromJson<KingInfo>(response);
					KingMoneyManager.Instance.SetKingInfo(info);
					yield return new WaitForSeconds(3f);
					GameObject player = GameObject.FindWithTag("Player");
					if (player != null)
					{
						KingMovement move = player.GetComponent<KingMovement>();
						move.IsMoveEnabled = true;
					}
					PlayerPrefs.SetInt("king_id", kingId);
					PlayerPrefs.SetInt("Player_Level", info.level);
					PlayerPrefs.Save();
					gameObject.SetActive(false);
				}
			}
		}
		isLogin = false;
	}

	public IEnumerator GetUnlockedFighters(int kingId)
	{
		string url = $"http://localhost/Unity連携/get_king_fighter_unlocked.php?king_id={kingId}";

		using (UnityWebRequest www = UnityWebRequest.Get(url))
		{
			yield return www.SendWebRequest();

			if(www.result != UnityWebRequest.Result.Success)
			{
				Debug.LogWarning("通信失敗: " + www.error);
				yield break;
			}

			var json = www.downloadHandler.text;
			var data = JsonUtility.FromJson<UnlockedFightersResponse>(json);

			if(data.success)
			{
				foreach(var fighterId in data.unlocked_fighters)
				{
					var target = FighterManager.Instance.fighterList.Find(f => f.fighter_id == fighterId);
					if(target != null)
					{
						target.unlocked = 1;
					}
				}
			}
			else 
			{
				Debug.LogWarning("解決済情報取得失敗: " + data.message);
			}
		}
	}

	[System.Serializable]
	public class UnlockedFightersResponse
	{
		public bool success;
		public List<int> unlocked_fighters;
		public string message;
	}

	[System.Serializable]
	public class KingInfo
	{
		public int king_id;
		public string king_name;
		public int money;
		public int level;
		public string prefab_name;
	}
}
