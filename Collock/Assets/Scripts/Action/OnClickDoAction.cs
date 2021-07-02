using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickDoAction : MonoBehaviour
{
	public List<DoAction> actions;

	public void OnClick()
	{
		foreach(var action in actions)
			action.Act();
	}
}
