using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Phase = PhaseManager.Phase;

public class SceneSwitch : MonoBehaviour
{
	public static SceneSwitch Instance { get; private set; }

	private Dictionary<(string toScene, string fromScene), Vector3> spawnPoints = new Dictionary<(string, string), Vector3>()
	{
		// 遷移先　遷移元　どの座標
		{("Saloon", "Castle"), new Vector3(-5, -2, 0)},
		{("WeaponShop", "Castle"), new Vector3(-5, -2, 0) },
		{("Castle", "Saloon"), new Vector3(15.5f, -2, 0) },
		{("Castle", "WeaponShop"), new Vector3(0.1f, -2, 0) },
		{("Castle", "Castle"), new Vector3(-0.1f, -2, 0) },
		{("Castle", "Forest"), new Vector3(-0.1f, -2, 0) },
		{("Forest", "Castle"), new Vector3(-0.1f, -2, 0) },
	};

	private Dictionary<Phase, string> phaseNames = new Dictionary<Phase, string>
	{
		{Phase.Castle, "城下町" },
		{Phase.Expedition, "遠征" },
		{Phase.Saloon, "酒場"},
		{Phase.WeaponShop, "鍛冶屋"}
	};

	[SerializeField] private Phase currentMode = Phase.Castle;
	[SerializeField] TextMeshProUGUI phaseText;
	private string castleSceneName = "Castle";
	private string firstFight = "Forest";
	private string barSceneName = "Saloon";
	private string shopSceneName = "WeaponShop";
	private string currentSceneName = "";
	private string lastSceneName = "";
	private bool isLoadingScene;
	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			// DontDestroyOnLoad(gameObject); // 必要なら
		}
		else
		{
			Destroy(gameObject);
		}
	}

	void Start()
	{
		SetCurrentMode(PhaseManager.Instance.CurrentPhase);
		LoadModeScene(currentMode, castleSceneName);

		PhaseManager.Instance.OnPhaseChanged += (newPhase) =>
		{
			SetCurrentMode(ConvertPhaseManagerToSceneSwitch(newPhase));
			LoadModeScene(currentMode, currentSceneName);
		};
	}

	private void Update()
	{
		UpdatePhaseText();
	}
	public void LoadModeScene(Phase phase, string fromScene)
	{
		if(isLoadingScene)
		{
			Debug.LogWarning("ロード中だっての");
			return;
		}

		isLoadingScene = true;
		lastSceneName = fromScene;

		// 新しいモードのシーン名を取得
		string newScene = phase switch
		{
			Phase.Castle => newScene = castleSceneName,
			Phase.Expedition => newScene = firstFight,
			Phase.Saloon => newScene = barSceneName,
			Phase.WeaponShop => newScene = shopSceneName,
			_ => ""
		};

		if (string.IsNullOrEmpty(newScene))
		{
			Debug.LogWarning("未対応のモード: " + phase);
			return;
		}

		// ロード後にアクティブシーンに設定（オプション）
		StartCoroutine(SwitchSceneInternal(newScene));
	}
	private IEnumerator SwitchSceneInternal(string newScene)
	{
		// ロード後にアクティブシーンに設定（オプション）
		yield return StartCoroutine(SwitchScene(newScene));
		isLoadingScene = false;
	}
	private Phase ConvertPhaseManagerToSceneSwitch(PhaseManager.Phase phase)
	{
		return phase switch
		{
			PhaseManager.Phase.Castle => Phase.Castle,
			PhaseManager.Phase.Expedition => Phase.Expedition,
			PhaseManager.Phase.Saloon => Phase.Saloon,
			PhaseManager.Phase.WeaponShop => Phase.WeaponShop,
			_ => Phase.Castle,
		};
	}
	public string GetCurrentSceneName()
	{
		return currentSceneName;
	}
	private Vector3 GetSpawnPositionForScene(string toScene, string fromScene)
	{
		if (spawnPoints.TryGetValue((toScene, fromScene), out Vector3 pos))
		{
			return pos;
		}
		else
		{
			Debug.LogWarning($"スポーン位置未定義: {toScene} ← {fromScene}");
			return Vector3.zero;
		}
	}

	private IEnumerator SwitchScene(string newScene)
	{
		if (!string.IsNullOrEmpty(currentSceneName))
		{
			AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(currentSceneName);
			if (unloadOp != null)
			{
				while (!unloadOp.isDone)
				{
					yield return null;
				}
			}
			else
			{
				Debug.LogWarning($"UnloadSceneAsyncがnullを返しました。シーン名: {currentSceneName}");
			}
		}
		else
		{
			Debug.LogWarning($"アンロードしようとしたシーンはロードされていません: {currentSceneName}");
		}

		AsyncOperation loadOp=SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Additive);
		while (!loadOp.isDone) 
		{
			yield return null;
		}
		currentSceneName = newScene;

		yield return SetActiveAfterLoad(newScene);
	}
	IEnumerator SetActiveAfterLoad(string sceneName)
	{
		yield return null;

		Scene loaded = SceneManager.GetSceneByName(sceneName);
		if (loaded.IsValid())
		{
			SceneManager.SetActiveScene(loaded);

			GameObject player = GameObject.FindWithTag("Player");
			if (player != null)
			{
				Vector3 spawnPos = GetSpawnPositionForScene(sceneName, lastSceneName);
				player.transform.position = spawnPos;
			}
		}
		else
		{
			Debug.LogWarning($"playerなし.");
		}
	}
	private void SetCurrentMode(Phase mode)
	{
		currentMode = mode;
		UpdatePhaseText();
	}
	public void UpdatePhaseText()
	{
		string displayName = phaseNames.ContainsKey(currentMode)
			? phaseNames[currentMode]
			: currentMode.ToString(); // fallback

		phaseText.text = "Phase:" + displayName;
	}
}
