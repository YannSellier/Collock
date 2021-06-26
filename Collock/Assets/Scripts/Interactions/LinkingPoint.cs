using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkingPoint : MonoBehaviour
{
	public GameObject linkObj = null;
	[HideInInspector] public PhotonView pv;


	LinkingPoint previousPoint = null;
	public LinkingPoint GetPrevious() { return previousPoint; }

	LinkingPoint nextPoint = null;
	public LinkingPoint GetNext() { return nextPoint; }

	public int nbOfLink = 0;


	public void Awake()
	{
		pv = GetComponent<PhotonView>();
	}

	public void LinkTo(LinkingPoint nextPoint)
	{
		this.nextPoint = nextPoint;
		nextPoint.previousPoint = this;

		pv.RPC("ChangeNbLink", RpcTarget.All, 1);
		nextPoint.pv.RPC("ChangeNbLink", RpcTarget.All, 1);
	}
	public void RemoveLink()
	{
		print(nextPoint + " is the next point");
		if (nextPoint != null)
		{
			nextPoint.previousPoint = null;
			nextPoint.RemoveLink();
		}
		nextPoint = null;
		if (linkObj) Destroy(linkObj);
		else print("link not found so not removed");
		linkObj = null;
	}

	public bool HasLink()
	{
		return nextPoint || previousPoint;
	}

	public void EnableAllLink(bool bEnable)
	{
		EnableLink(bEnable);
		nextPoint.EnableAllLink(bEnable);
	}
	public void EnableLink(bool bEnable)
	{
		if (linkObj && linkObj.GetComponent<Link>())
			linkObj.GetComponent<Link>().EnableLink(bEnable);
	}

	[PunRPC]
	public void ChangeNbLink(int change)
	{
		nbOfLink += change;
	}

}
