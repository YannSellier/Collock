using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : Interactable
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
		if (interactable != this) return;

		pv.RPC("GetPickedUp", RpcTarget.All);
	}

	#endregion



	///==========================================================================================================
	///		PICKUP
	///==========================================================================================================

	#region PickUp variables

	public Item item;

	#endregion

	#region Pickup functions

	[PunRPC]
	public void GetPickedUp()
	{
		//Debug.Log(Camera.current);
		//Debug.Log(Camera.current.GetComponent<Inventory>());
		//Debug.Log(item.itemName);
		//Camera.current.GetComponent<Inventory>().AddItem(item);
		print(GameManager.instance);
		print(GameManager.instance.localPlayer);
		print(GameManager.instance.localPlayer.inv);
		print(item);

		GameManager.instance.localPlayer.inv.AddItem(item);
		Destroy(gameObject);
	}

	#endregion

}
