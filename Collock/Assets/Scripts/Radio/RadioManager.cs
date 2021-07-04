using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioManager : DragDropWindowManager
{

	#region Components

	public List<GameObject> signals;
	public List<Inventory> invs;

	#endregion


	///==========================================================================================================
	///		OVERRIDE
	///==========================================================================================================


	public override void OnDrop()
	{

		//if (bIsUsed && !bIsAuthor) return;

		//base.OnDrop();

		UpdateSignalDisplay();


	}
	public override void OnDragFromThis(DDTrigger ddt)
	{
		base.OnDragFromThis(ddt);
		

	}




	///==========================================================================================================
	///		MANAGEMENT
	///==========================================================================================================

	#region management variables

	private int currentSim = -1;


	#endregion

	#region management functions

	public void SelectSim(int indexSlot)
	{
		ChangeSelectedSim(indexSlot);
	}
	[PunRPC]
	public void ChangeSelectedSim(int newSim, bool bRep = true)
	{
		//if (bIsUsed && !bIsAuthor)	return;

		if(bRep)
		{
			pv.RPC("ChangeSelectedSim", RpcTarget.All, newSim, false);
			return;
		}

		if (newSim == -1 || currentSim == newSim || inventories[newSim].ItemCount() == 0)
			newSim = -1;

		currentSim = newSim;
		UpdateSignalDisplay();
	}

	#endregion





	///==========================================================================================================
	///		DISPLAY
	///==========================================================================================================

	#region Display varaiables

	public List<string> signalSimNames;

	#endregion

	#region Display functions

	public void UpdateSignalDisplay()
	{
		int indexSignal = currentSim == -1 ? -1 : ConvertSimToSignal(inventories[currentSim].items[0].itemName);

		for (int i = 0; i < signals.Count; i++)
			signals[i].SetActive(i == indexSignal);
	}
	public int ConvertSimToSignal(string simName)
	{
		return signalSimNames.IndexOf(simName);
	}
	public void UpdateAllDisplay()
	{
		UpdateSimDisplay();
		UpdateSignalDisplay();
	}
	public void UpdateSimDisplay()
	{
		foreach(var inv in invs)
		{
			inv.UpdateAllDisplay();
		}
	}

	#endregion





	///==========================================================================================================
	///		OPEN
	///==========================================================================================================

	#region Open callabakc functions

	public override void OnOpen()
	{
		base.OnOpen();

		UpdateAllDisplay();
	}

	#endregion
}
