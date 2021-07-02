using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDPickupAction : DDPickup
{
	DoAction actionObj;
	public override void GetPickedUp()
	{
		actionObj.Act();
		base.GetPickedUp();
	}
}
