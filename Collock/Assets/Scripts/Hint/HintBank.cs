using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hint
{
	public int code;
	public string msg;
	public bool bActive = true;
	public float displayTime = 0;
}

[CreateAssetMenu(fileName = "HintBank", menuName = "Custom/Create Hints Bank", order = 1)]
public class HintBank : ScriptableObject
{

	public List<Hint> hints;

	public void Setup()
	{
		foreach(var hint in hints)
		{
			hint.bActive = true;
		}
	}

	public Hint GetHintByCode(int code)
	{

		foreach(var hint in hints)
		{
			if (hint.code == code)
				return hint;
		}
		return null;
	}

}
