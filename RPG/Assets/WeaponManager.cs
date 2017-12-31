﻿using UnityEngine.Networking;
using UnityEngine;

public class WeaponManager : NetworkBehaviour {

	[SerializeField]
	private PlayerWeapon primaryWeapon;

	[SerializeField]
	private Transform weaponHolder;

	[SerializeField]
	private string weaponLayerName = "Weapon";	

	private PlayerWeapon currentWeapon;
	private WeaponGraphics currentGraphics;

	// Use this for initialization
	void Start () 
	{
		EquipWeapon (primaryWeapon);
	}

	public PlayerWeapon GetCurrentWeapon()
	{
		return currentWeapon;
	}

	public WeaponGraphics GetCurrentGraphics()
	{
		return currentGraphics;
	}


	void EquipWeapon(PlayerWeapon _weapon)
	{
		currentWeapon = _weapon;

		GameObject _weaponIns = (GameObject)Instantiate (_weapon.graphics, weaponHolder.position, weaponHolder.rotation);
		_weaponIns.transform.SetParent (weaponHolder);

		currentGraphics = _weaponIns.GetComponent<WeaponGraphics> ();
		if (currentGraphics == null)
			Debug.LogError ("No weapon graphics component on the weapon object: " + _weaponIns.name);
		
		if (isLocalPlayer)
			Util.SetLayerRecursively (_weaponIns, LayerMask.NameToLayer (weaponLayerName)); 	
	}

}
