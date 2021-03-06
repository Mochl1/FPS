﻿using UnityEngine;
using UnityEngine.Networking;


[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour {

	[SerializeField]
	Behaviour[] componentsToDisable;

	[SerializeField]
	string remoteLayerName = "RemotePlayer";

	[SerializeField]
	string dontDrawLayerName = "DontDraw";

	[SerializeField]
	GameObject playerGraphics;

	[SerializeField]
	GameObject playerUIPrefab;
	[HideInInspector]
	public GameObject playerUIInstance;


	void Start()
	{
		if (!isLocalPlayer) {
			DisableComponents ();
			AssingRemoteLayer ();
		} else {
			

			SetLayerRecursively (playerGraphics, LayerMask.NameToLayer (dontDrawLayerName));

			playerUIInstance = Instantiate (playerUIPrefab);
			playerUIInstance.name = playerUIPrefab.name;

			GetComponent<Player> ().SetupPlayer ();

			string _username = "Loading...";
			if (UserAcountManager.isLoggedIn)
				_username = UserAcountManager.playerUsername;
			else
				_username = transform.name;
			CmdSetUsername (transform.name, _username);
		}
	}

	[Command]
	void CmdSetUsername (string playerID, string username)
	{
		Player player = GameManager.GetPlayer (playerID);
		if (player != null) 
		{
			Debug.Log (username + " has joined");
			player.username = username;
		}
	}


	void SetLayerRecursively(GameObject obj, int newLayer)
	{
		obj.layer = newLayer;

		foreach (Transform child in obj.transform) 
		{
			SetLayerRecursively (child.gameObject, newLayer);
		}
	}

	public override void OnStartClient()
	{
		base.OnStartClient ();

		string _netID = GetComponent<NetworkIdentity> ().netId.ToString();
		Player _player = GetComponent<Player> ();

		GameManager.RegisterPlayer(_netID,_player);
	}	

	void AssingRemoteLayer()
	{
		gameObject.layer = LayerMask.NameToLayer (remoteLayerName);
	}

	void DisableComponents()
	{
		for (int i = 0; i < componentsToDisable.Length; i++) 
		{
			componentsToDisable [i].enabled = false;
		}
	}

	void OnDisable()
	{
		Destroy (playerUIInstance);

		if(isLocalPlayer)
		GameManager.instance.SetSceneCameraActive (true);

		GameManager.UnRegisterPlayer (transform.name);
	}
}