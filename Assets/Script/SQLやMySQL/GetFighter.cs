using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class Fighter
{
    public int fighter_id;      // id
    public string fighter_kind; // 種類
    public int cost;            // コスト
    public int fighter_level;   // ファイターレベル
    public int attack;          // 攻撃力
    public float speed;         // 速度
    public string prefab_name;  // プレハブ名
    public int unlocked;        // 解放済みか
	public int unlock_cost;     // 初回解放コスト
}

[System.Serializable]
public class FighterListWrapper
{
    public List<Fighter> fighter;
}


public class GetFighter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetFighterData());
    }

    IEnumerator GetFighterData()
    {
        string url = "http://localhost/Unity連携/get_fighters_info.php";
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if(www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("通信失敗" + www.error);
            yield break;
        }

        string json = www.downloadHandler.text;
		FighterListWrapper data = JsonUtility.FromJson<FighterListWrapper>(json);
        foreach(var f in data.fighter)
        {
            Debug.Log($"ID:{f.fighter_id} 種類:{f.fighter_kind} コスト:{f.cost}" +
                $" レベル:{f.fighter_level} 攻撃力:{f.attack} 速度:{f.speed}" + 
                $"プレハブ名:{f.prefab_name} アンロック:{f.unlocked} 初回解放コスト:{f.unlock_cost}");
        }
        foreach(var f in FighterManager.Instance.fighterList)
        {
            Debug.Log($"{f.fighter_kind} 解放状況: {f.unlocked}");
        }
    }
}
