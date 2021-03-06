using System.Collections;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;

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

		if (CanDragDrop())
		{
			if (Input.GetMouseButtonDown(0))
				TryStartDDOp();

			if (Input.GetMouseButton(0))
				UpdateDragDrop();

			if (Input.GetMouseButtonUp(0))
				EndDragDrop();
		}

		if(CanCallOnClick())
		{
			if (Input.GetMouseButtonDown(0))
				TryOnClickCall();
		}
	}


	#endregion





	///==========================================================================================================
	///		INTERACTIONS
	///==========================================================================================================

	#region On Click functions

	public bool CanCallOnClick()
	{
		return true;
	}
	public void TryOnClickCall()
	{
		OnClickDoAction obj = StaticLib.SearchForOnClickObj();
		if (!obj) return;

		obj.OnClick();
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
		while(interactionsInProgress.Count > 0)
		{ 
			EndInteraction(interactionsInProgress[0]);
			interactionsInProgress.RemoveAt(0);
		}
	}


	// Add an interaction in progress to the list
	private void AddInteractionInProgress(Interactable interactable)
	{
		interactionsInProgress.Add(interactable);
	}


	private bool CanInteract()
	{
		return !HintManager.instance ||  !HintManager.instance.bHintOpen;
	}


	#endregion




	///==========================================================================================================
	///		INVENTORY
	///==========================================================================================================

	#region Inventory variables

	public Inventory inv;
	public List<Inventory> invs;

	public ItemBank itemBank;

	#endregion

	#region inv functions

	public int GetIndexInv(Inventory inv)
	{
		return invs.IndexOf(inv);
	}
	public Inventory GetInv(int index)
	{
		return invs[index];
	}

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


	public List<OpenWindowObj> windowObjs;

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
	public void ForceCloseWindow()
	{
		foreach(var o in windowObjs)
		{
			o.CloseWindow();
		}
	}


	public void ForceCloseAll()
	{
		ForceClosefile();
		ForceCloseWindow();
	}

	#endregion







	///==========================================================================================================
	///		DRAG DROP
	///==========================================================================================================

	#region DragDrop variables

	public bool bDDInterfaceOpen = false;
	public bool bIsDDActive = false;

	public DragDropOp ddOpObj;


	[SerializeField]
	public DragDropWindowManager ddManager;

	[SerializeField] GraphicRaycaster m_Raycaster;
	PointerEventData m_PointerEventData;
	[SerializeField] EventSystem m_EventSystem;

	#endregion


	#region Drag Drop interface functions

	public bool CanDragDrop()
	{
		return bDDInterfaceOpen && (ddManager.bIsAuthor || !ddManager.bIsUsed) && (!HintManager.instance || !HintManager.instance.bHintOpen);
	}

	public void OpenDDInterface(bool bOpen)
	{
		bDDInterfaceOpen = bOpen;
	}


	public void TryStartDDOp()
	{
		DDTrigger ddt = SearchForDDTrigger();
		if (ddt == null || !ddt.bAuthorizeDrag) return;


		bIsDDActive = true;

		Item item = ddt.invDisplay.GetSelectedItem();
		ddManager.OnDrag(ddt);

		print("start op with " + item.itemName + "From " + ddt);
		ddOpObj.SetupDragDrop(item, invs.IndexOf(ddt.inv));
	}
	

	public void UpdateDragDrop()
	{
		if (ddOpObj == null || !bIsDDActive) return;

		ddOpObj.UpdatePosition(StaticLib.GetMouseWorldPos2D());
	}
	public DDTrigger SearchForDDTrigger()
	{



		return GetAimedObj().GetComponent<DDTrigger>();
	}
	public GameObject GetAimedObj()
	{
		//Set up the new Pointer Event
		m_PointerEventData = new PointerEventData(m_EventSystem);
		//Set the Pointer Event Position to that of the game object
		m_PointerEventData.position = Input.mousePosition;

		//Create a list of Raycast Results
		List<RaycastResult> results = new List<RaycastResult>();

		//Raycast using the Graphics Raycaster and mouse click position
		m_Raycaster.Raycast(m_PointerEventData, results);

		return results.Count > 0 ? results[0].gameObject : null;
	}

	public void EndDragDrop()
	{
		if (!bIsDDActive ) return;

		bIsDDActive = false;
		ddOpObj.EndOp(SearchForDDTrigger());
		ddManager.OnDrop();
	}

	#endregion
}
