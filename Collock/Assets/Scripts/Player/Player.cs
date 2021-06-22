using System.Collections;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

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
		if (Input.GetMouseButtonDown(0))
			StartInteracting();

		if (Input.GetMouseButton(0))
			UpdateAllInteractions();

		if (Input.GetMouseButtonUp(0))
			EndAllInteraction();
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

		interactable.pv.RPC("StartInteraction",RpcTarget.MasterClient, StaticLib.GetMouseWorldPos2D());
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
		interactable.pv.RPC("EndInteraction", RpcTarget.MasterClient, StaticLib.GetMouseWorldPos2D());
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


	#endregion



	///==========================================================================================================
	///		INVENTORY
	///==========================================================================================================

	#region Inventory variables

	public Inventory inv;

	#endregion



	///==========================================================================================================
	///		MULTIPLAYER
	///==========================================================================================================

	#region Multiplayer variables

	public PhotonView pv;

	#endregion

}
