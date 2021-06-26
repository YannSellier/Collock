using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkingPoint : MonoBehaviour
{
	GameObject linkObj = null;


	LinkingPoint previousPoint = null;
	public LinkingPoint GetPrevious() { return previousPoint; }

	LinkingPoint nextPoint = null;
	public LinkingPoint GetNext() { return nextPoint; }

	public int nbOfLink = 0;
	

	public void LinkTo(LinkingPoint nextPoint, Link linkObj)
	{
		this.nextPoint = nextPoint;
		this.linkObj = linkObj.gameObject;
		nextPoint.previousPoint = this;

		nbOfLink++;
		nextPoint.nbOfLink++;
	}
	public void RemoveLink()
	{
		if (nextPoint == null) return;

		nextPoint.previousPoint = null;
		nextPoint.RemoveLink();
		nextPoint = null;
		if(linkObj) PhotonNetwork.Destroy(linkObj);
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

}
