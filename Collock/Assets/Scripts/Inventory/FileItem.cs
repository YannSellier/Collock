using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FileItem : Item
{

	
	public int indexFile = 0;
	public bool bShouldOpenToAll = false;


	public override void Use()
	{
		base.Use();

		GameManager.instance.DisplayFile(indexFile, bShouldOpenToAll);
	}

}
