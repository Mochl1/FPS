using UnityEngine.UI;
using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

	public Text killCount;
	public Text deathCount;

	// Use this for initialization
	void Start () 
	{
		if(UserAcountManager.isLoggedIn)
		UserAcountManager.instance.GetData (OnReceivedData);
	}
	
	void OnReceivedData(string data)
	{
		if (killCount == null || deathCount == null)
			return;
		
		killCount.text = DataTranslator.DataToKills (data).ToString () + " Kills";
		deathCount.text = DataTranslator.DataToDeaths (data).ToString () + " Deaths";

	}
}
