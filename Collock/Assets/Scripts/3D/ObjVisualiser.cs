using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjVisualiser : MonoBehaviour,IOpen
{

	///==========================================================================================================
	///		GENERAL
	///==========================================================================================================

	#region Components

	[Header("Components")]
	public GameObject obj;

	#endregion

	#region Unity Built-in functions

	public void Update()
	{
		if (bIsRot) RotObj(currentDir);
	}

	#endregion




	///==========================================================================================================
	///		VISUALISATION
	///==========================================================================================================

	#region Visualisation variables

	[Header("Rot params")]
	public float rotationSpeed = 45;
	public Vector2[] dirs;

	Vector2 currentDir;
	bool bIsRot = false;

	#endregion

	#region visualisation functions

	public void RotObj(Vector2 dir)
	{
		obj.transform.RotateAround(obj.transform.position, dir, rotationSpeed * Time.deltaTime);
	}
	public void StartRot(int indexBtn)
	{
		currentDir = dirs[indexBtn];
		bIsRot = true;
	}
	public void StopRot()
	{
		bIsRot = false;
	}

	#endregion



	///==========================================================================================================
	///		OPEN WINDOW
	///==========================================================================================================

	#region Open window callback functions

	public void OnOpen()
	{
		obj.SetActive(true);
	}
	public void OnClose()
	{
		obj.SetActive(false);
	}

	#endregion
}
