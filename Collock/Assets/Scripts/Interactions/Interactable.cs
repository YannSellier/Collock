using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EInteractType
{
	ON_HOLD
}

public class Interactable:MonoBehaviour
{
	public PhotonView pv;

	public virtual void Awake()
	{
		pv = GetComponent<PhotonView>();
	}

	[PunRPC]
	public virtual bool StartInteraction(Vector2 mousePos) // return true if the interaction has begun
	{
		return true;
	}
	public virtual void UpdateInteraction()
	{

	}
	[PunRPC]
	public virtual void EndInteraction(Vector2 mousePos)
	{

	}
}
