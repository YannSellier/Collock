using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenFileObj : Interactable
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

		OpenFile();
	}

	#endregion




	///==========================================================================================================
	///		OPEN FILE
	///==========================================================================================================

	#region Open File variables

	public int indexFile;
	public bool bOpenOnAll = false;

	#endregion

	#region Open File functions

	[PunRPC]
	public void OpenFile()
	{
		GameManager.instance.DisplayFile(indexFile,bOpenOnAll);
	}

	#endregion



}
