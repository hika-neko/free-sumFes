using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLoop : MonoBehaviour
{
    [SerializeField] Sprite[] loopSprites;
    [SerializeField] float interval = 0.5f;

    private SpriteRenderer sr;
    private int currentIndex;
    private float timer;

	private void Start()
	{
        sr = GetComponent<SpriteRenderer>();
        if(loopSprites.Length > 0)
        {
            sr.sprite = loopSprites[0];
        }
	}

	private void Update()
	{
        if (loopSprites.Length == 0) return;
        timer += Time.deltaTime;
        if(timer >= interval)
        {
            timer = 0;
            currentIndex = (currentIndex + 1) % loopSprites.Length;
            sr.sprite = loopSprites[currentIndex];
        }
	}
}
