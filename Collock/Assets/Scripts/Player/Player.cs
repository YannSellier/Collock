using System.Collections;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{


	///==========================================================================================================
	///		UNITY BUILT-IN
	///==========================================================================================================

	#region Unity's functions

	public void Awake()
	{
		interactionsInProgress = new List<Interactable>();
		inv = GetComponent<Inventory>();
		pv = GetComponent<PhotonView>();
	}

	public void Start()
	{
		GameManager.instance.DeclarePlayer(this);
	}

	public void Update()
	{
		UpdateInputs();
	}

	#endregion

	#region Inputs


	private void UpdateInputs()
	{

		if (CanInteract())
		{
			if (Input.GetMouseButtonDown(0))
				StartInteracting();

			if (Input.GetMouseButton(0))
				UpdateAllInteractions();

			if (Input.GetMouseButtonUp(0))
				EndAllInteraction();
		}
	}


	#endregion




	///==========================================================================================================
	///		INTERACTIONS
	///==========================================================================================================

	#region Interaction variables

	private List<Interactable> interactionsInProgress;

	#endregion


	#region Interaction functions


	// Start the interactions
	private void StartInteracting()
	{
		(Collider2D coll, Interactable interactable) = StaticLib.SearchForInteractable();
		if (!coll || interactable == null) return;

		if(interactable.StartInteraction(StaticLib.GetMouseWorldPos2D()))
			AddInteractionInProgress(interactable);
	}


	// Update the interactions
	private void UpdateInteraction(Interactable interactable)
	{
		interactable.UpdateInteraction();
	}
	private void UpdateAllInteractions()
	{
		foreach(var interactable in interactionsInProgress)
		{
			UpdateInteraction(interactable);
		}
	}


	// End the interactions
	public void EndInteraction(Interactable interactable)
	{
		interactable.EndInteraction(StaticLib.GetMouseWorldPos2D());
	}
	public void EndAllInteraction()
	{
		foreach(var interactable in interactionsInProgress)
		{
			EndInteraction(interactable);
		}
	}


	// Add an interaction in progress to the list
	private void AddInteractionInProgress(Interactable interactable)
	{
		interactionsInProgress.Add(interactable);
	}


	private bool CanInteract()
	{
		return true;
	}


	#endregion




	///==========================================================================================================
	///		INVENTORY
	///==========================================================================================================

	#region Inventory variables

	public Inventory inv;

	#endregion



	///==========================================================================================================
	///		FILE DISPLAYING
	///==========================================================================================================

	#region Interface variables

	public FileDisplayer fileDisplayer;

	#endregion

	#region Interface functions

	public void DisplayFile(FileInfos file)
	{
		if (!CanDisplayFile()) return;

		OpenFile(true);
		fileDisplayer.Setup(file);
		fileDisplayer.DisplayItem(true);
	}
	public void ForceClosefile()
	{
		fileDisplayer.CloseDisplay(true);
	}
	public bool CanDisplayFile()
	{
		return CanOpenFile(true);
	}

	#endregion




	///==========================================================================================================
	///		MULTIPLAYER
	///==========================================================================================================

	#region Multiplayer variables

	public PhotonView pv;

	#endregion




	///==========================================================================================================
	///		WINDOWS MANAGEMENT
	///==========================================================================================================

	#region windows management variables

	public bool bIsFileOpen = false;
	[PunRPC] public void SetIsFileOpen(bool newState, bool bRep = true)
	{
		if(bRep)
		{
			pv.RPC("SetIsFileOpen", RpcTarget.All, newState, false);
			return;
		}

		bIsFileOpen = newState;
	}



	public bool bIsWindowOpen = false;
	[PunRPC] public void SetIsWindowOpen(bool newState, bool bRep = true)
	{
		if (bRep)
		{
			pv.RPC("SetIsWindowOpen", RpcTarget.All, newState, false);
			return;
		}

		bIsWindowOpen = newState;
	}

	#endregion


	#region Windows management functions

	public void OpenFile(bool bOpen)
	{
		SetIsFileOpen(bOpen,false);
	}
	public bool CanOpenFile(bool bOpen)
	{
		print("Try opening File " + bOpen + " with f:" + bIsFileOpen + " and w: " + bIsWindowOpen + " => " + !(bOpen == bIsFileOpen || bIsWindowOpen));
		if (bOpen == bIsFileOpen || bIsWindowOpen) return false;

		

		return true;
	}
	public void OpenWindow(bool bOpen)
	{
		if (bIsFileOpen) ForceClosefile();

		SetIsWindowOpen(bOpen,false);
	}
	public bool CanOpenWindow(bool bOpen)
	{
		print("Try opening window " + bOpen + " with f:" + bIsFileOpen + " and w: " + bIsWindowOpen + " => " + !(bOpen == bIsWindowOpen || bIsFileOpen));
		if (bOpen == bIsWindowOpen || bIsFileOpen) return false;

		

		return true;
	}

	#endregion

}
