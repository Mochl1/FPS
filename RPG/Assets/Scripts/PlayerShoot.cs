using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour {

	private const string PLAYER_TAG = "Player";


	[SerializeField]
	private Camera cam;

	[SerializeField]
	private LayerMask mask;

	private PlayerWeapon currentWeapon;
	private WeaponManager weaponManager;

	// Use this for initialization
	void Start () {
		if (cam == null) {
			Debug.LogError ("Player Shoot:No camera referenced");
			this.enabled = false;
		}

		weaponManager = GetComponent<WeaponManager> ();

	}
	
	// Update is called once per frame
	void Update () {
		currentWeapon = weaponManager.GetCurrentWeapon();

		if (PauseMenu.IsOn)
			return;

		if (currentWeapon.bullets < currentWeapon.maxBullets) 
		{
			if (Input.GetKeyDown (KeyCode.R)) 
			{
				weaponManager.Reload ();
				return;
			}
		}

		if (currentWeapon.fireRate <= 0f) {
			if (Input.GetButtonDown ("Fire1")) {
				Shoot ();
			}
		} else
		{
			if (Input.GetButtonDown ("Fire1")) {
				InvokeRepeating ("Shoot", 0f, 1f / currentWeapon.fireRate);
			} else if (Input.GetButtonUp ("Fire1")) 
			{
				CancelInvoke ("Shoot");
			}
		}

	}

	[Command]
	void CmdOnHit(Vector3 _pos, Vector3 _normal)
	{
		RpcDoHitEffect (_pos, _normal);
	}

	[ClientRpc]
	void RpcDoHitEffect(Vector3 _pos, Vector3 _normal)
	{
		GameObject _hitEffect = (GameObject)Instantiate (weaponManager.GetCurrentGraphics ().hitEffectPrefab, _pos, Quaternion.LookRotation (_normal));
		Destroy (_hitEffect, 2f);
	}


	[Client]
	void Shoot()
	{
		if (!isLocalPlayer && !weaponManager.isReloading) {
			Debug.Log ("Out of bullets.");
			return;
		}



		if (currentWeapon.bullets <= 0) 
		{
			weaponManager.Reload();
			return;
		}
		currentWeapon.bullets--;
		Debug.Log ("Remaining bullets: " + currentWeapon.bullets);

		CmdOnShoot ();

		RaycastHit _hit;
		if (Physics.Raycast (cam.transform.position,cam.transform.forward,out _hit, currentWeapon.range, mask)) 
		{
			//We hit something
			if (_hit.collider.tag == PLAYER_TAG)
			{
				CmdPlayerShot (_hit.collider.name, currentWeapon.damage, transform.name);
			}

			CmdOnHit (_hit.point, _hit.normal);
		}
	}

	[Command]
	void CmdOnShoot()
	{
		RpcDoShootEffect ();
	}

	[ClientRpc]
	void RpcDoShootEffect()
	{
		weaponManager.GetCurrentGraphics ().muzzleFlash.Play ();
	}

	[Command]
	void CmdPlayerShot(string _PlayerID, int _damage, string _sourceID)
	{
		Debug.Log (_PlayerID + " has been shot.");

		Player _player = GameManager.GetPlayer (_PlayerID);
		_player.RpcTakeDamage (_damage,_sourceID);
	}
		

}
