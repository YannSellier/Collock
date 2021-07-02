using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOpen
{
	public void OnOpen();
	public void OnClose();
}

public class OpenWindowObj : Interactable
{
	///==========================================================================================================
	///		UNITY BUIL-IN
	///==========================================================================================================

	#region Unity Buil-in functions

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	#endregion




	///==========================================================================================================
	///		INTERACTIONS
	///==========================================================================================================

	#region Interactable override

	[PunRPC]
	public override void EndInteraction(Vector2 mousePos)
	{
		(Collider2D coll, Interactable interactable) = StaticLib.SearchForInteractable(mousePos);
		if (interactable != this || !GameManager.instance.localPlayer.CanOpenWindow(!bOpen)) return;

		
		if (!bOpen)
		{
			GameManager.instance.localPlayer.OpenWindow(true);
			OpenWindow();
		}
		else if(bCanClose)
		{
			print("close window");
			GameManager.instance.localPlayer.OpenWindow(false);
			CloseWindow();
		}
	}

	#endregion




	///==========================================================================================================
	///		OPEN WINDOW
	///==========================================================================================================

	#region Open File variables

	public GameObject windowToOpen;
	public GameObject ObjToOpenManager;
	public GameObject objToClose;
	public List<Collider2D> closeBtnColls;
	public List<Collider2D> openBtnColls;
	public bool bOpenOnAll = false;
	public bool bCloseOnAll = false;

	private bool bOpen = false;
	public bool bCanClose = true;


	#endregion

	#region Open File functions

	[PunRPC]
	public void OpenWindow(bool bRep = true)
	{

		print("open");

		if (bOpenOnAll && bRep)
		{
			pv.RPC("OpenWindow", RpcTarget.All, false);
			return;
		}

		print("sync");
		

		bOpen = true;
		

		windowToOpen.SetActive(true);

		print(" before IOPen callbcks");


		if (ObjToOpenManager) ObjToOpenManager.GetComponent<IOpen>().OnOpen();

		print("IOPen callbcks");
		

		foreach (var coll in openBtnColls) coll.enabled = false;
		foreach (var coll in closeBtnColls) coll.enabled = true;

		print("turn off colls");

		if (objToClose)	objToClose.SetActive(false);
	}

	public void CloseWindow(bool bRep = false)
	{
		if (!bCanClose) return;


		if (bRep)
		{
			pv.RPC("CloseWindow", RpcTarget.All, false);
			return;
		}


		

		bOpen = false;

		if(ObjToOpenManager) ObjToOpenManager.GetComponent<IOpen>().OnClose();
		windowToOpen.SetActive(false);
		if(objToClose) objToClose.SetActive(true);

		foreach (var coll in openBtnColls) coll.enabled = true;
		foreach (var coll in closeBtnColls) coll.enabled = false;
	}

	[PunRPC]
	public void ChangeCanClose(bool newState, bool bRep = true)
	{
		if (bRep)
		{
			pv.RPC("ChangeCanClose", RpcTarget.All, newState, false);
			return;
		}
		bCanClose = newState;
	}

	#endregion

}
