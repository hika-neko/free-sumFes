using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageLoop : MonoBehaviour
{
	[SerializeField] Sprite[] idleLoopSprites;
	[SerializeField] Sprite[] talkSprites;
	[SerializeField] float interval = 0.5f;

	private Image m_image;
	private int currentIndex;
	private float timer;
	private bool isTalked = false;

	private void Start()
	{
		m_image = GetComponent<Image>();
		if (idleLoopSprites.Length > 0)
		{
			m_image.sprite = idleLoopSprites[0];
		}
	}

	private void Update()
	{
		timer += Time.deltaTime;

		if (isTalked)
		{
			if (talkSprites.Length == 0) return;
			if (timer >= interval)
			{
				timer = 0;
				currentIndex = (currentIndex + 1) % talkSprites.Length;
				m_image.sprite = talkSprites[currentIndex];
			}
		}
		else 
		{
			if (idleLoopSprites.Length == 0) return;
			if (timer >= interval)
			{
				timer = 0;
				currentIndex = (currentIndex + 1) % idleLoopSprites.Length;
				m_image.sprite = idleLoopSprites[currentIndex];
			}
		}
	}

	public void TalkMaster()
	{
		isTalked = true;
		currentIndex = 0;
		timer = 0;
	}

	public void StopTalkMaster()
	{
		isTalked = false;
		currentIndex = 0;
		timer = 0;
	}
}

