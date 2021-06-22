using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LinkStruct
{
	public LinkingPoint from;
	public LinkingPoint to;	

	public LinkStruct(LinkingPoint lp1, LinkingPoint lp2)
	{
		from = lp1;
		to = lp2;
	}
}

public class LinkingObj : Interactable
{


	///==========================================================================================================
	///		UNITY BUILT-IN
	///==========================================================================================================

	#region Unity's functions

	public override void Awake()
	{
		base.Awake();
		InitLinkPointsLists();


	}

	public void Update()
	{
	}

	#endregion




	///==========================================================================================================
	///		INTERACTION
	///==========================================================================================================


	#region Interaction variables


	private bool bIsInProgress = false;


	#endregion


	#region Interactable functions override


	[PunRPC]
	public override void StartInteraction(Vector2 mousePos)
	{
		Collider2D coll = StaticLib.GetAimedCollider2D(mousePos);
		int indexPoint = linkingPointsColls.IndexOf(coll);
		if (bIsInProgress || !CanStartLinkOn(indexPoint)) return;

		pv.RPC("ChangeInProgressState", RpcTarget.All, true);
		StartLinkOn(indexPoint);

	}	


	public override void UpdateInteraction()
	{
		//if (currentLink && linkStart)
		pv.RPC("UpdateLink", RpcTarget.MasterClient, StaticLib.GetMouseWorldPos2D(), false);

		//UpdateLink(StaticLib.GetMouseWorldPos2D());
	}

	[PunRPC]
	public override void EndInteraction(Vector2 mousePos)
	{
		if (bIsInProgress)
			EndLink(mousePos, true);
			//pv.RPC("EndLink",RpcTarget.MasterClient,StaticLib.GetMouseWorldPos2D(), true);

		pv.RPC("ChangeInProgressState", RpcTarget.All, false);
	}


	#endregion




	///==========================================================================================================
	///		LINKING
	///==========================================================================================================

	#region Linking variables


	protected List<Collider2D> linkingPointsColls;
	protected List<LinkingPoint> linkingPoints;

	protected LinkingPoint linkStart;
	protected Link currentLink;

	public string linkPrefabPath = "LinkPrefab";

	[SerializeField]
	public List<LinkStruct> unauthorizedLinks;

	public List<LinkStruct> successLinks;

	public bool bLinkChainAuthorized = false;


	#endregion


	#region Initializing linking


	private void InitLinkPointsLists()
	{
		linkingPointsColls = new List<Collider2D>();
		for(int i = 0; i < transform.childCount; i++)
		{
			Transform ch = transform.GetChild(i);

			if(ch)
				linkingPointsColls.Add(ch.GetComponent<Collider2D>());
			
		}


		linkingPoints = new List<LinkingPoint>();
		foreach (var lpc in linkingPointsColls)
		{
			linkingPoints.Add(lpc.GetComponent<LinkingPoint>());
		}
	}


	#endregion

	#region Linking functions


	// Starting a link
	[PunRPC]
	protected void StartLinkOn(int indexPoint)
	{
		if (indexPoint == -1) return;

		LinkingPoint lp = linkingPoints[indexPoint];
		lp.RemoveLink();
		if (!bLinkChainAuthorized && lp.GetPrevious()) lp.GetPrevious().RemoveLink();

		linkStart = lp;

		Debug.Log("Start link on " +  lp.gameObject.name);

		InstantiateLink();
		UpdateLink(StaticLib.GetMouseWorldPos2D());


	}
	private void InstantiateLink()
	{ 
		if(PhotonNetwork.IsConnected)
			currentLink = PhotonNetwork.Instantiate(linkPrefabPath, new Vector3(0, 0, 0), Quaternion.identity, 0).GetComponent<Link>();
		else
		{
			GameObject gm = new GameObject();
			gm.AddComponent<LineRenderer>();
			gm.AddComponent<PhotonView>();
			currentLink = gm.AddComponent<Link>();			
		}
	}


	// Updating the link
	[PunRPC]
	public virtual void UpdateLink(Vector2 mousePos, bool bCheckForEnd = true)
	{
		if (!currentLink) return;

		UpdateLinkPos(mousePos);

	}
	private void UpdateLinkPos(Vector2 mousePos)
	{
		currentLink.pv.RPC("UpdatePos", RpcTarget.All, (Vector2)linkStart.transform.position, mousePos);
	}


	// Ending the link (return true if the Link has been successfull)
	[PunRPC]
	public bool EndLink(Vector2 mousePos, bool bShouldCancel = true)
	{
		bool bLinkSucess = true;

		Collider2D coll = StaticLib.GetAimedCollider2D(mousePos);
		int indexLP = linkingPointsColls.IndexOf(coll);
		if (indexLP == -1 || !CanEndLinkOn(indexLP) || !CanLinkFromTo(linkStart,linkingPoints[indexLP]))
		{
			Debug.Log("Index aimed point " + indexLP);
			if (bShouldCancel)
				CancelLink();
			else
				return false;

			bLinkSucess = false;
		}
		else
		{
			LinkingPoint lp = linkingPoints[indexLP];

			UpdateLink(lp.transform.position, false);
			linkStart.LinkTo(lp, currentLink);


			Debug.Log("End link on " + lp.gameObject.name);

			if (IsLinkingCorrect())
				Debug.Log("Linking successfull");
		}



		currentLink = null;
		linkStart = null;

		return bLinkSucess;
	}
	private void CancelLink()
	{
		if (currentLink) PhotonNetwork.Destroy(currentLink.gameObject);
	}


	#endregion

	#region Link Validation functions


	// Get linking possibilities
	protected virtual bool CanStartLinkOn(int indexPoint)
	{
		return true;
	}
	protected virtual bool CanEndLinkOn(int indexPoint)
	{
		return !linkingPoints[indexPoint].HasLink() && linkingPoints[indexPoint] != linkStart;
	}
	protected bool CanLinkFromTo(LinkingPoint lp1, LinkingPoint lp2)
	{
		LinkStruct ls1 = new LinkStruct(lp1,lp2);
		LinkStruct ls2 = new LinkStruct(lp2,lp1);
		return !unauthorizedLinks.Contains(ls1) && !unauthorizedLinks.Contains(ls2);
	}


	// Success linking check
	protected virtual bool IsLinkingCorrect()
	{
		foreach(var ls in successLinks)
		{
			if (ls.from.GetNext() != ls.to && ls.to.GetNext() != ls.from)
				return false;
		}
		return true;
	}


	#endregion




	///==========================================================================================================
	///		MULTIPLAYER
	///==========================================================================================================




	#region Sync functions

	[PunRPC]
	public void ChangeInProgressState(bool newState)
	{
		bIsInProgress = newState;
	}

	

	

	#endregion

}
