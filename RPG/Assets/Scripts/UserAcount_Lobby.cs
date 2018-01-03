using UnityEngine.UI;
using UnityEngine;

public class UserAcount_Lobby : MonoBehaviour {

	public Text usernameText;

	// Use this for initialization
	void Start () 
	{
		if(UserAcountManager.isLoggedIn)
			usernameText.text = UserAcountManager.playerUsername;
	}

	public void LogOut()
	{
		if(UserAcountManager.isLoggedIn)
		UserAcountManager.instance.LogOut ();
	}
}
