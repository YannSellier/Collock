using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoPickup : DoAction
{
	public Pickable objToPickup;

	public override void Act()
	{
		base.Act();

		objToPickup.GetPickedUp();
	}
}
