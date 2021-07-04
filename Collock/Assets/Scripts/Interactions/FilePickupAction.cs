using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilePickupAction : FilePickup
{
	public DoAction actionObj;

	[PunRPC]
	public override void GetPickedUp(bool bRep = true)
	{
		actionObj.Act();
		base.GetPickedUp(bRep);
	}
}
