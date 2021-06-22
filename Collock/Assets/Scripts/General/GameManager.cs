using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;


	public void Awake()
	{ 

		if (!instance) instance = this;
		else Destroy(this);

		DontDestroyOnLoad(gameObject);

	}


	public Player localPlayer;

	public List<Player> players = new List<Player>();



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
}
