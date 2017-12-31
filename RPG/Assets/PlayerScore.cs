using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerScore : MonoBehaviour {

	Player player;

	// Use this for initialization
	void Start () 
	{
		player = GetComponent<Player> ();
		StartCoroutine (SyncScoreLoop ());
	}

	void OnDestroy()
	{
		if(player !=null)
		SyncNow ();
	}

	IEnumerator SyncScoreLoop()
	{
		yield return new WaitForSeconds (5f);

		while (true) {

			SyncNow ();

			}
	}

	void SyncNow()
	{
		if (UserAcountManager.isLoggedIn) {
			UserAcountManager.instance.GetData (OnDataReceived);

		}
	}


	void OnDataReceived(string data)
	{
		if (player.kills == 0 && player.deaths == 0)
			return;
		
		int kills = DataTranslator.DataToKills (data);
		int deaths = DataTranslator.DataToDeaths (data);

		int newKills = player.kills + kills;
		int newDeaths = player.deaths + deaths;

		string newData = DataTranslator.ValuesToData (newKills, newDeaths);

		Debug.Log ("syncing " + newData);

		player.kills = 0;
		player.deaths = 0;

		UserAcountManager.instance.SendData (newData);

	}
}
