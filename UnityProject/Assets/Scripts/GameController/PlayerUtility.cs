using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerUtility : MonoBehaviour {

	public static void FreezePlayers()
	{
		List<GameObject> players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
		players.ForEach(player => {
			PlatformerCharacter2D move = player.GetComponent<PlatformerCharacter2D>();
			move.frozen = true;
		});
	}

	public static void UnFreezePlayers()
	{
		List<GameObject> players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
		players.ForEach(player => {
			PlatformerCharacter2D move = player.GetComponent<PlatformerCharacter2D>();
			move.frozen = false;
		});
	}

}
