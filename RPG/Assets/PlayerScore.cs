using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerScore : MonoBehaviour {

	int lastKills = 0;
	int lastDeaths=0;

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
		if (player.kills <= lastKills && player.deaths <= lastDeaths)
			return;
		
		int killsSinceLast = player.kills - lastKills;
		int deathsSinceLast = player.deaths - lastDeaths;

		int kills = DataTranslator.DataToKills (data);
		int deaths = DataTranslator.DataToDeaths (data);

		int newKills = killsSinceLast + kills;
		int newDeaths = deathsSinceLast + deaths;

		string newData = DataTranslator.ValuesToData (newKills, newDeaths);

		Debug.Log ("Syncing " + newData);

		lastKills = player.kills;
		lastDeaths = player.deaths;

		UserAcountManager.instance.SendData (newData);

	}
}
