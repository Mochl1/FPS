using UnityEngine;
using UnityEngine.UI;

public class Scoreboarditem : MonoBehaviour {

	[SerializeField]
	Text usernameText;

	[SerializeField]
	Text killsText;

	[SerializeField]
	Text deatsText;

	public void Setup (string username, int kills, int deaths)
	{
		usernameText.text = username;
		killsText.text = "Kills: " + kills;
		deatsText.text = "Deaths " + deaths;
	}

}
