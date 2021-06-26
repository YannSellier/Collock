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
	public void OpenWindow()
	{
		if (bOpenOnAll)
		{
			pv.RPC("OpenWindow", RpcTarget.All);
			return;
		}


		bOpen = true;

		windowToOpen.SetActive(true);
		ObjToOpenManager.GetComponent<IOpen>().OnOpen();

		foreach (var coll in openBtnColls) coll.enabled = false;
		foreach (var coll in closeBtnColls) coll.enabled = true; 

		objToClose.SetActive(false);
	}

	public void CloseWindow()
	{
		if (!bCanClose) return;


		if (bCloseOnAll)
		{
			pv.RPC("CloseWindow", RpcTarget.All);
			return;
		}


		

		bOpen = false;

		ObjToOpenManager.GetComponent<IOpen>().OnClose();
		windowToOpen.SetActive(false);
		objToClose.SetActive(true);

		foreach (var coll in openBtnColls) coll.enabled = true;
		foreach (var coll in closeBtnColls) coll.enabled = false;
	}

	[PunRPC]
	public void ChangeCanClose(bool newState)
	{
		bCanClose = newState;
	}

	#endregion

}
