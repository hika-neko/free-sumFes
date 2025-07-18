using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorMove : MonoBehaviour
{
	[SerializeField] List<RectTransform> fightersPanel;
	[SerializeField] private RectTransform selector;

	private int currentIndex = 0;
	private int columnCount = 6; // 1行の要素数（任意に調整）

	private void Update()
	{
		if(FighterUnlock.Instance.isOpen)
		{
			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				if (currentIndex + 1 < fightersPanel.Count &&
					(currentIndex + 1) % columnCount != 0)
				{
					currentIndex++;
					MoveSelector();
				}
			}

			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				if (currentIndex % columnCount != 0)
				{
					currentIndex--;
					MoveSelector();
				}
			}

			//if (Input.GetKeyDown(KeyCode.DownArrow))
			//{
			//	int next = currentIndex + columnCount;
			//	if (next < fightersPanel.Count)
			//	{
			//		currentIndex = next;
			//		MoveSelector();
			//	}
			//}

			//if (Input.GetKeyDown(KeyCode.UpArrow))
			//{
			//	int prev = currentIndex - columnCount;
			//	if (prev >= 0)
			//	{
			//		currentIndex = prev;
			//		MoveSelector();
			//	}
			//}

			if (Input.GetButtonDown("Submit"))
			{
				int fighterId = currentIndex + 2;
				Debug.Log("選択中のファイターを解放: " + fighterId);
				FighterUnlock.Instance.TryUnlockFighterById(fighterId);
			}
		}
	}
	void MoveSelector()
	{
		selector.anchoredPosition =
			new Vector2(fightersPanel[currentIndex].anchoredPosition.x, fightersPanel[currentIndex].anchoredPosition.y - 55);
	}
}