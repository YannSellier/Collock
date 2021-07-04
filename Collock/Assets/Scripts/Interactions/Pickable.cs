using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickable : Interactable
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

	public override void EndInteraction(Vector2 mousePos)
	{
		pv.RPC("SERVER_EndInteraction", RpcTarget.MasterClient, mousePos);
	}
	[PunRPC]
	public void SERVER_EndInteraction(Vector2 mousePos)
	{
		(Collider2D coll, Interactable interactable) = StaticLib.SearchForInteractable(mousePos);
		if (interactable != this) return;

		GetPickedUp(true);
	}

	#endregion



	///==========================================================================================================
	///		PICKUP
	///==========================================================================================================

	#region PickUp variables


	#endregion

	#region Pickup functions

	public abstract Item GetItem();

	[PunRPC]
	public virtual void GetPickedUp(bool bRep = true)
	{
		if(bRep)
		{
			pv.RPC("GetPickedUp", RpcTarget.All, false);
			return;
		}

		GameManager.instance.localPlayer.inv.AddItem(GetItem());
		Destroy(gameObject);
	}

	#endregion

}
