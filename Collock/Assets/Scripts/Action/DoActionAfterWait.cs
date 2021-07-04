using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoActionAfterWait : MonoBehaviour
{

	public List<DoAction> actions;
	public float delay;

	private float startTime = 0;


	public void Start()
	{
		startTime = Time.time;
	}

	public void Update()
	{
		if(Time.time - startTime >= delay)
		{
			foreach(var action in actions)
			{
				action.Act();
			}
			Destroy(gameObject);
		}
	}
}
