using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

	[SerializeField]
	private float speed=5f;
	[SerializeField]
	private float sensibilidad=3f;

	private PlayerMotor motor;


	void Start(){

		motor = GetComponent<PlayerMotor> ();
	 
	}

	void Update(){

		if (PauseMenu.IsOn) {
			if (Cursor.lockState != CursorLockMode.None)
				Cursor.lockState = CursorLockMode.None;

			motor.Move (Vector3.zero);
			motor.Rotate (Vector3.zero);
			motor.RotateCamera (0f);
			return;
		}

		if (Cursor.lockState != CursorLockMode.Locked) 
		{
			Cursor.lockState = CursorLockMode.Locked;
		}

		float _xMov = Input.GetAxisRaw ("Horizontal");
		float _zMov = Input.GetAxisRaw ("Vertical");

		Vector3 _movHorizontal = transform.right * _xMov; 
		Vector3 _movVertical = transform.forward * _zMov;

		Vector3 _velocity = (_movHorizontal + _movVertical).normalized * speed;

		motor.Move (_velocity);

		float _yRot = Input.GetAxisRaw ("Mouse X");
		Vector3 _rotation = new Vector3 (0f, _yRot, 0f) * sensibilidad;
		motor.Rotate (_rotation);


		float _xRot = Input.GetAxisRaw ("Mouse Y");
		float _cameraRotationX = _xRot * sensibilidad;
		motor.RotateCamera (_cameraRotationX);




	}



}
