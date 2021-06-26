using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour
{
	public float minimum = 0.0f;
	public float maximum = 1f;
	public float duration = 2.0f;
	public float startTime;
	public SpriteRenderer sprite;
	void Awake()
	{
		if (!sprite) sprite = GetComponent<SpriteRenderer>();
	}
	void Update()
	{
		if (Time.time > duration + startTime+1) return;

		float t = (Time.time - startTime) / duration;
		sprite.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(minimum, maximum, t));
	}


	
}
