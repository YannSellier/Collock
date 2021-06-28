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

public class LinkingObj : Interactable, IWaitingCallBacks, IOpen
{
	public static LinkingObj instance;

	///==========================================================================================================
	///		UNITY BUILT-IN
	///==========================================================================================================

	#region Unity's functions

	public override void Awake()
	{
		if (!instance) instance = this;
		else Destroy(gameObject);

		base.Awake();
		InitLinkPointsLists();


	}
	public void Start()
	{
		waitingRoom.Setup(this);
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

	public override bool StartInteraction(Vector2 mousePos)
	{

		Collider2D coll = StaticLib.GetAimedCollider2D(mousePos);
		int indexPoint = linkingPointsColls.IndexOf(coll);
		if (bIsInProgress || !CanStartLinkOn(indexPoint) || !bIsOpen) return false;

		//pv.RPC("SERVER_StartInteraction", RpcTarget.MasterClient, mousePos);
		SERVER_StartInteraction(indexPoint);
		ShowValidationBtns(false);


		return true;
	}
	[PunRPC]
	public void SERVER_StartInteraction(int indexPoint)
	{
		

		pv.RPC("ChangeInProgressState", RpcTarget.All, true);
		StartLinkOn(indexPoint);

	}	
	


	public override void UpdateInteraction()
	{
		if (!bIsCodeAuthor || !bIsInProgress || !bIsOpen) return;
		//pv.RPC("UpdateLink", RpcTarget.MasterClient, StaticLib.GetMouseWorldPos2D(), false);

		UpdateLink(StaticLib.GetMouseWorldPos2D());
	}



	public override void EndInteraction(Vector2 mousePos)
	{
		if (bIsInProgress && !bIsVoting && bIsCodeAuthor)
			ShowValidationBtns(true);

		//pv.RPC("SERVER_EndInteraction", RpcTarget.MasterClient, mousePos);
		SERVER_EndInteraction(mousePos);

		
	}
	[PunRPC]
	public virtual void SERVER_EndInteraction(Vector2 mousePos)
	{
		if (bIsInProgress)
		{
			EndLink(mousePos, true);

			//pv.RPC("ShowValidationBtns", RpcTarget.All, true);
			//pv.RPC("ChangeInProgressState", RpcTarget.All, false);
			openWindowObj.pv.RPC("ChangeCanClose", RpcTarget.All, false);
		}
	}


	#endregion




	///==========================================================================================================
	///		LINKING
	///==========================================================================================================

	#region Linking variables


	public List<Collider2D> linkingPointsColls;
	public List<LinkingPoint> linkingPoints;

	protected LinkingPoint linkStart;
	protected Link currentLink;

	public string linkPrefabPath = "LinkPrefab";

	[SerializeField]
	public List<LinkStruct> unauthorizedLinks;

	public List<LinkStruct> successLinks;

	public bool bLinkChainAuthorized = false;

	public SpriteRenderer codeImg;
	public Sprite[] codeImgs = new Sprite[3];

	public GameObject successWindow;

	#endregion


	#region Initializing linking


	private void InitLinkPointsLists()
	{
		if (linkingPoints.Count > 0 && linkingPointsColls.Count > 0) return;

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

	
	public void EnableAllLinks(bool bEnable)
	{
		foreach (var point in linkingPoints)
			point.EnableLink(bEnable);
	}


	// Starting a link
	[PunRPC] protected void StartLinkOn(int indexPoint)
	{
		if (indexPoint == -1) return;

		LinkingPoint lp = linkingPoints[indexPoint];
		lp.RemoveLink();
		if (!bLinkChainAuthorized && lp.GetPrevious())
			lp.GetPrevious().RemoveLink();

		linkStart = lp;

		Debug.Log("Start link on " +  lp.gameObject.name);

		bIsCodeAuthor = true;

		InstantiateLink();
		currentLink.Setup(indexPoint);
		UpdateLink(StaticLib.GetMouseWorldPos2D());


	}
	private void InstantiateLink()
	{
		if (PhotonNetwork.IsConnected)
		{
			currentLink = PhotonNetwork.Instantiate(linkPrefabPath, new Vector3(0, 0, 0), Quaternion.identity, 0).GetComponent<Link>();
			//currentLink.pv.RPC("EnableLink",RpcTarget.All);
			
		}
		else
		{
			GameObject gm = new GameObject();
			gm.AddComponent<LineRenderer>();
			gm.AddComponent<PhotonView>();
			currentLink = gm.AddComponent<Link>();
		}
	}
	


	// Updating the link
	[PunRPC] public virtual void UpdateLink(Vector2 mousePos, bool bCheckForEnd = true)
	{
		if (!currentLink) return;

		UpdateLinkPos(mousePos);

	}
	private void UpdateLinkPos(Vector2 mousePos)
	{
		currentLink.pv.RPC("UpdatePos", RpcTarget.All, (Vector2)linkStart.transform.position, mousePos);
	}


	// Ending the link (return true if the Link has been successfull)
	[PunRPC] public bool EndLink(Vector2 mousePos, bool bShouldCancel = true)
	{
		bool bLinkSucess = true;

		Collider2D coll = StaticLib.GetAimedCollider2D(mousePos);
		int indexLP = linkingPointsColls.IndexOf(coll);
		if (indexLP == -1 || !CanEndLinkOn(indexLP) || !CanLinkFromTo(linkStart,linkingPoints[indexLP]))
		{
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
			linkStart.LinkTo(lp);


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

	[PunRPC]
	public void RemoveAllLinks(bool bShouldRep = true)
	{
		if(bShouldRep)
		{
			pv.RPC("RemoveAllLinks", RpcTarget.All, false);
			return;
		}

		foreach (var lp in linkingPoints)
		{
			lp.RemoveLink();
		}
	}


	#endregion

	#region Link Validation functions


	// Get linking possibilities
	protected virtual bool CanStartLinkOn(int indexPoint)
	{
		return !bIsVoting;
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

	[PunRPC] public void SuccessLinking()
	{
		StartCoroutine("SuccessActionChain");
	}
	public IEnumerator SuccessActionChain()
	{
		ChangeCodeImg(2);

		yield return new WaitForSeconds(2);

		successWindow.SetActive(true);

		yield return new WaitForSeconds(2);

		GetComponent<SuccessAction>().SuccessAct();
	}

	[PunRPC] public void FailLinking()
	{
		print("Fail");
		StartCoroutine("FailActionChain");
		openWindowObj.pv.RPC("ChangeCanClose", RpcTarget.All, true);
	}
	public IEnumerator FailActionChain()
	{
		ChangeCodeImg(1);

		yield return new WaitForSeconds(2);

		CancelLinking();
		ChangeCodeImg(0);
	}


	[PunRPC] public void ChangeCodeImg(int indexImg, bool bShouldRep = true)
	{
		if (bShouldRep)
		{
			pv.RPC("ChangeCodeImg", RpcTarget.All, indexImg, false);
			return;
		}

		codeImg.sprite = codeImgs[indexImg];
	}


	#endregion





	///==========================================================================================================
	///		VOTING
	///==========================================================================================================

	#region Voting variables

	private bool bIsVoting = false;
	private bool bIsCodeAuthor = false;

	public GameObject validationBtns;
	public WaitingPlayers waitingRoom;

	public OpenWindowObj openWindowObj;

	#endregion

	#region Voting functions

	[PunRPC] public void CancelLinking()
	{
		/*if (!PhotonNetwork.IsMasterClient)
		{
			pv.RPC("CancelLinking", RpcTarget.MasterClient);
			return;
		}*/
		bIsCodeAuthor = false;
		RemoveAllLinks();

		print("Cancel linking");
		pv.RPC("ChangeInProgressState", RpcTarget.All, false);
		pv.RPC("ShowValidationBtns", RpcTarget.All, false);
		openWindowObj.pv.RPC("ChangeCanClose", RpcTarget.All, true);
		
		waitingRoom.pv.RPC("CancelWaitingRoom", RpcTarget.All);
	}
	public void ValidateLinking()
	{

		if (!bIsVoting)
		{
			pv.RPC("ChangeIsVotingState", RpcTarget.All, true);
			waitingRoom.StartWaitingRoom();
		}

		waitingRoom.EnterWaitingRoom(true);
		ShowValidationBtns(false);
	}

	[PunRPC] public void ShowValidationBtns(bool bShow)
	{
		print("Set buttons active: " + bShow);
		validationBtns.SetActive(bShow);
	}

	#endregion

	#region Waiting callbacks

	public void StartVote()
	{
		print("The vote has started");
		ShowValidationBtns(true);
		
		EnableAllLinks(true);
		OpenSelf();
	}
	public void CancelWaitingRoom()
	{
		pv.RPC("ChangeIsVotingState", RpcTarget.All, false);

		//CancelLinking();
	}
	public void EndWaitingRoom()
	{
		if (!bIsCodeAuthor) return;

		if (IsLinkingCorrect())
			pv.RPC("SuccessLinking", RpcTarget.All);
		else
			pv.RPC("FailLinking", RpcTarget.All);
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
	[PunRPC]
	public void ChangeIsVotingState(bool newState)
	{
		bIsVoting = newState;
	}

	#endregion





	///==========================================================================================================
	///		OPENING
	///==========================================================================================================

	#region Open variables

	[HideInInspector]public bool bIsOpen = false;

	public OpenWindowObj opener;

	#endregion

	#region IOpen callbacks functions

	public void OnOpen()
	{
		bIsOpen = true;
		EnableAllLinks(true);
	}
	public void OnClose()
	{
		bIsOpen = false;
		EnableAllLinks(false);
	}

	#endregion

	#region Opener functions

	private void OpenSelf()
	{
		opener.OpenWindow();
	}
	private void CloseSelf()
	{
		opener.CloseWindow();
	}

	#endregion
}
