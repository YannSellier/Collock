using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoEnableHint : DoAction
{
	public bool bEnable = false;
	public int hintCode = 0;

	public override void Act()
	{
		base.Act();

		HintManager.instance.hintBank.GetHintByCode(hintCode).bActive = bEnable;
	}
}
