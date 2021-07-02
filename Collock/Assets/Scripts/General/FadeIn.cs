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
	public bool bShouldTime = false;

	private float time = 0;
	public void StartTimer()
	{
		print("Start timer on " + gameObject);
		bShouldTime = true;
	}
	void Awake()
	{
		if (!sprite) sprite = GetComponent<SpriteRenderer>();
		sprite.color = new Color(1, 1, 1, 0);
		GameManager.instance.DeclareFadeInObj(this);
	}
	void Update()
	{
		if (!bShouldTime) return;

		time += Time.deltaTime;

		if (time > duration + startTime+1) return;

		float t = (time - startTime) / duration;
		sprite.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(minimum, maximum, t));
	}


	
}
