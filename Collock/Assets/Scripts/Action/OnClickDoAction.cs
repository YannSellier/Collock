using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickDoAction : MonoBehaviour
{
	public List<DoAction> actions;
	public bool bDoOnAll = false;
	public PhotonView pv;
	public void Awake()
	{
		pv = GetComponent<PhotonView>();
	}
	public void CallAct(bool bRep = true)
	{
		foreach (var action in actions)
			action.Act();
	}

	[PunRPC]
	public void OnClick(bool bRep = true)
	{
		if (bDoOnAll && bRep)
		{
			pv.RPC("OnClick", RpcTarget.All, false);
			return;
		}

		CallAct();
	}
}
