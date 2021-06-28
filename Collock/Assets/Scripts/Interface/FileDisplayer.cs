using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileDisplayer : MonoBehaviour
{

	///==========================================================================================================
	///		GENERAL
	///==========================================================================================================

	#region Conponents variables

	[HideInInspector] public PhotonView pv;

	#endregion

	#region Unity's functions

	public void Awake()
	{
		pv = GetComponent<PhotonView>();
	}

	public void Update()
	{
	}

	#endregion




	///==========================================================================================================
	///		FILE DISPLAYER
	///==========================================================================================================

	#region Variables

	public GameObject displayWindow;
	public GameObject nextBtns;


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
			if(titleDisplayer) titleDisplayer.text = file.fileTitle;


			Sprite sprite = file.fileContentImgs[indexPage];
			imageDisplayer.sprite = sprite;
			if (sprite != null)
			{
				float aspectRatio = sprite.rect.width / sprite.rect.height;
				var fitter = imageDisplayer.GetComponent<UnityEngine.UI.AspectRatioFitter>();
				fitter.aspectRatio = aspectRatio;
			}

			imageDisplayer.color = new Color(1, 1, 1, 1);

			nextBtns.SetActive(file.fileContentImgs.Count > 1);
		}
		else
		{
			imageDisplayer.color = new Color(1, 1, 1, 0);
		}

		// Then turn on the window
		displayWindow.SetActive(bShouldDisplay);
	}
	[PunRPC]
	public void CloseDisplay(bool bForceClosing = false)
	{
		print("Close display " + bForceClosing + " and canopenfile: " + GameManager.instance.localPlayer.CanOpenFile(false));
		if (GameManager.instance.localPlayer.CanOpenFile(false) || bForceClosing)
		{

			print("Close diaplsay succesfull");

			GameManager.instance.localPlayer.OpenFile(false);
			DisplayItem(false);
		}
	}

	#endregion

	#region Selection functions

	public void NextPage(int dir)
	{
		ChangePage(indexPage + dir);
	}
	protected virtual void ChangePage(int newPage)
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
