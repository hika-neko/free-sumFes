using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
	public enum Mode { Castle, Fight }

	[SerializeField] private Mode currentMode = Mode.Castle;

	private string castleSceneName = "Castle";
	private string fightSceneName = "Stage";
	private string currentSceneName = "";

	void Start()
	{
		LoadModeScene(currentMode);
	}

	void Update()
	{

	}

	public void ToggleMode()
	{
		// モード切り替え
		currentMode = currentMode == Mode.Castle ? Mode.Fight : Mode.Castle;
		LoadModeScene(currentMode);
	}

	void LoadModeScene(Mode mode)
	{
		// 今のシーンをアンロード（存在していれば）
		if (!string.IsNullOrEmpty(currentSceneName))
		{
			SceneManager.UnloadSceneAsync(currentSceneName);
		}

		// 新しいモードのシーン名を取得
		string newScene = (mode == Mode.Castle) ? castleSceneName : fightSceneName;

		// Additiveでロード
		SceneManager.LoadScene(newScene, LoadSceneMode.Additive);
		currentSceneName = newScene;

		// ロード後にアクティブシーンに設定（オプション）
		StartCoroutine(SetActiveAfterLoad(newScene));
	}

	System.Collections.IEnumerator SetActiveAfterLoad(string sceneName)
	{
		yield return null;
		Scene loaded = SceneManager.GetSceneByName(sceneName);
		if (loaded.IsValid())
		{
			SceneManager.SetActiveScene(loaded);
		}
	}
}
