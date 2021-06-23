using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FileInfos
{
	public string fileTitle = "None";
	public List<Sprite> fileContentImgs;
}


[CreateAssetMenu(fileName = "FileBank", menuName = "Custom/Create File Bank", order = 1)]
public class FileBank : ScriptableObject
{
	public List<FileInfos> files;
}
