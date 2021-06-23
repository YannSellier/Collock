using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	public PhotonView pv;

	///==========================================================================================================
	///		UNITY BUIL-IN
	///==========================================================================================================

	#region Unity Buil-in functions

	public void Awake()
	{
		print("Awake GameMAnager: " + gameObject.name);

		if (!instance) instance = this;
		else Destroy(gameObject);

		DontDestroyOnLoad(gameObject);

		pv = GetComponent<PhotonView>();

	}
	public void Start()
	{
		print("Start GameMAnager: " + gameObject.name);
	}
	void Update()
	{

	}

	#endregion



	
	///==========================================================================================================
	///		PLAYERS
	///==========================================================================================================

	#region Players variables

	public Player localPlayer;

	public List<Player> players = new List<Player>();

	#endregion

	#region Players functions

	public void DeclarePlayer(Player player)
	{
		players.Add(player);
		DeclareLocalPlayer(player);
		//if (player.pv.IsMine) DeclareLocalPlayer(player);
	}
	public void DeclareLocalPlayer(Player player)
	{
		localPlayer = player;
	}

	#endregion




	///==========================================================================================================
	///		FILES
	///==========================================================================================================

	#region Files variables

	public FileBank fileBank;

	#endregion

	#region Files functions

	[PunRPC]
	public void DisplayFile(int indexFile, bool bShowToAll = false)
	{
		if(bShowToAll)
		{
			pv.RPC("DisplayFile", RpcTarget.All, indexFile, false);
		}
		else
		{
			
			localPlayer.DisplayFile(fileBank.files[indexFile]);
		}
	}

	#endregion

}
