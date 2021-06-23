using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileDisplayer : MonoBehaviour
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
	///		FILE DISPLAYER
	///==========================================================================================================

	#region Variables

	public GameObject displayWindow;
	public Text titleDisplayer;
	public Image imageDisplayer;

	public FileInfos file;

	private int indexPage = 0;

	#endregion


	#region Display functions

	public void Setup(FileInfos fileToDisplay)
	{
		file = fileToDisplay;
		indexPage = 0;
	}
	public void DisplayItem(bool bShouldDisplay)
	{
		// Set the content
		if (file != null)
		{
			titleDisplayer.text = file.fileTitle;
			imageDisplayer.sprite = file.fileContentImgs[indexPage];
		}

		// Then turn on the window
		displayWindow.SetActive(bShouldDisplay);
	}
	public void CloseDisplay()
	{
		DisplayItem(false);
	}

	#endregion

	#region Selection functions

	public void NextPage(int dir)
	{
		ChangePage(indexPage + dir);
	}
	private void ChangePage(int newPage)
	{
		indexPage = KeepPageInBound(newPage);
		DisplayItem(true);
	}
	private int KeepPageInBound(int index)
	{
		return Mathf.Clamp(index, 0, file.fileContentImgs.Count - 1);
	}

	#endregion
}
