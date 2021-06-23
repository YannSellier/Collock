using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilePickup : Pickable
{
	public FileItem fileItem;

	public override Item GetItem() { return fileItem; }
}
