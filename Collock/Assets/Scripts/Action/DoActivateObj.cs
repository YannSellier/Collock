using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoActivateObj : DoAction
{
	public GameObject objToEnable;
	public bool bEnable = true;

	public override void Act()
	{
		base.Act();

		objToEnable.SetActive(bEnable);
	}
}
