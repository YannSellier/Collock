using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkCode : LinkingObj
{


	///==========================================================================================================
	///		LINKINGOBJ OVERRIDE
	///==========================================================================================================

	#region functions override


	public override void UpdateInteraction()
	{
		pv.RPC("UpdateLink", RpcTarget.MasterClient, StaticLib.GetMouseWorldPos2D(), true);

		//if (currentLink && linkStart)
			//UpdateLink(StaticLib.GetMouseWorldPos2D(),true);
	}

	[PunRPC]
	public override void UpdateLink(Vector2 mousePos, bool bCheckForEnd = true)
	{
		base.UpdateLink(mousePos);

		if (bCheckForEnd && EndLink(mousePos,false))
		{
			int indexPoint = linkingPointsColls.IndexOf(StaticLib.GetAimedCollider2D(mousePos));
			pv.RPC("StartLinkOn", RpcTarget.MasterClient, indexPoint);
		}
	}

	[PunRPC]
	public void TryLink()
	{

	}

	public override void EndInteraction(Vector2 mousePos)
	{
		pv.RPC("SERVER_EndInteraction", RpcTarget.MasterClient, mousePos);
	}
	[PunRPC]
	public override void SERVER_EndInteraction(Vector2 mousePos)
	{
		base.SERVER_EndInteraction(mousePos);

		foreach(var lp in linkingPoints)
		{
			lp.RemoveLink();
		}
	}


	#endregion


}
