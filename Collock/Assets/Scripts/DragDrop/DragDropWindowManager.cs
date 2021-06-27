using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDropWindowManager: MonoBehaviour, IOpen
{
    
	public void OnOpen()
	{
		GameManager.instance.localPlayer.OpenDDInterface(true);
	}

	public void OnClose()
	{
		GameManager.instance.localPlayer.OpenDDInterface(true);

	}

}
