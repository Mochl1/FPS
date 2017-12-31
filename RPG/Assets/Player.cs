﻿using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerSetup))]
public class Player : NetworkBehaviour {
	[SyncVar]
	private bool _isDead = false;
	public bool isDead
	{
		get { return _isDead; }
		protected set{ _isDead = value;}
	}

	[SerializeField]
	private int maxHealth = 100;

	[SyncVar]
	private int currentHealth;

	[SerializeField]
	private Behaviour[] disableOnDeath;
	private bool[] wasEnabled;

	[SerializeField]
	private GameObject[] disabbleGameObectsOnDeath;

	[SerializeField]
	private GameObject deathEffect;

	private bool firstSetup = true; 

	public void SetupPlayer()
	{
		if (isLocalPlayer) 
		{

			GameManager.instance.SetSceneCameraActive (false);
			GetComponent<PlayerSetup> ().playerUIInstance.SetActive (true);
		}

	CmdBroadCastNewPlayerSetup ();
	}

	[Command]
	private void CmdBroadCastNewPlayerSetup()
	{
		RpcSetupPlayerOnAllClients ();
	}

	[ClientRpc]
	private void RpcSetupPlayerOnAllClients()
	{
		if (firstSetup) 
		{
			wasEnabled = new bool[disableOnDeath.Length];
			for (int i = 0; i < wasEnabled.Length; i++) 
			{
				wasEnabled [i] = disableOnDeath [i].enabled;
			}
			firstSetup = false;
		}

		SetDefaults ();
	}

	void Update()
	{
		if (!isLocalPlayer)
			return;
	
		if (Input.GetKeyDown (KeyCode.K)) 
		{
			RpcTakeDamage (9999);
		}
	}

	[ClientRpc]
	public void RpcTakeDamage(int _amount)
	{
		if (isDead)
			return;
		currentHealth -= _amount;

		Debug.Log (transform.name + " now has " + currentHealth + " health. ");

		if (currentHealth <= 0)
		{
			Die ();
		}

	}

	private void Die()
	{
		isDead = true;

		//disable components
		for (int i = 0; i < disableOnDeath.Length; i++) 
		{
			disableOnDeath [i].enabled = false;
		}

		for (int i = 0; i < disabbleGameObectsOnDeath.Length; i++) 
		{
			disabbleGameObectsOnDeath [i].SetActive (false);
		}


		Collider _col = GetComponent<Collider> ();
		if (_col != null)
			_col.enabled = false;

		GameObject _gfxIns = (GameObject) Instantiate (deathEffect, transform.position, Quaternion.identity);
		Destroy (_gfxIns, .5f);

		if (isLocalPlayer) 
		{
			GameManager.instance.SetSceneCameraActive (true);
			GetComponent<PlayerSetup> ().playerUIInstance.SetActive (false);
		}

		//Call Respawn Method
		StartCoroutine(Respawn());
	}

	private IEnumerator Respawn()
	{
		yield return new WaitForSeconds (GameManager.instance.matchSettings.respawnTime);

		SetDefaults ();
		Transform _spawnPoint = NetworkManager.singleton.GetStartPosition ();
		transform.position = _spawnPoint.position;
		transform.rotation = _spawnPoint.rotation;

		yield return new WaitForSeconds (0.1f);

		SetupPlayer ();
	}

	void SetDefaults()
	{
		isDead = false;

		currentHealth = maxHealth;

		for (int i = 0; i < disableOnDeath.Length; i++) 
		{
			disableOnDeath [i].enabled = wasEnabled [i];
		}

		for (int i = 0; i < disabbleGameObectsOnDeath.Length; i++) 
		{
			disabbleGameObectsOnDeath [i].SetActive (true);
		}

		Collider _col = GetComponent<Collider> ();
		if (_col != null)
			_col.enabled = true;

	}
}
