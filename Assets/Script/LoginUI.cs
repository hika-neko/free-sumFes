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

	private int selectedIndex = 0;
	private bool isCreating = false;
	private enum Mode
	{
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
			PlayerPrefs.SetInt("king_id", result.king_id);
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
			Debug.Log("新規作成: king_id = " + result.king_id);
			Debug.Log("入力された名前: " + addNameInput.text);
			gameObject.SetActive(false);
		}
		isCreating = false;
	}
	IEnumerator LoginKing(string name, string id)
	{
		WWWForm form = new WWWForm();
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
					gameObject.SetActive(false);
				}
			}
		}
		//string url = $"http://localhost/Unity連携/login_king.php";
		//UnityWebRequest www = UnityWebRequest.Get(url);
		//yield return www.SendWebRequest();

		//if (www.result != UnityWebRequest.Result.Success 
		//	&& www.downloadHandler.text == "not_found")
		//{
		//	Debug.LogError("データ取得失敗: " + www.error);
		//}
		//else
		//{
		//	gameObject.SetActive(false);
		//}
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
