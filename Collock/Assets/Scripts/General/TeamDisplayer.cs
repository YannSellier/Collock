using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TeamDisplayer : MonoBehaviour
{



	///==========================================================================================================
	///		GENERAL
	///==========================================================================================================

	#region Unity Buil-in functions

	void Awake()
	{
		displayText = GetComponentInChildren<TextMeshProUGUI>();
	}
	private void Start()
	{
		GetPlayerList();
	}
	void Update()
	{

	}

	#endregion




	///==========================================================================================================
	///		PLAYERS LIST
	///==========================================================================================================

	#region variables

	private List<string> playersNames;
	private TextMeshProUGUI displayText;

	#endregion

	#region Displaying functions

	private void GetPlayerList()
	{
		playersNames = new List<string>();
		foreach (var player in PhotonNetwork.PlayerList)
			playersNames.Add(player.NickName);

		UpdateDisplay();
	}

	private void UpdateDisplay()
	{
		string listText = "";
		foreach (var name in playersNames)
			listText += name + "\n"; 
		displayText.text = listText;
	}

	#endregion



}
