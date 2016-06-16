using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour {

    public GameObject Shield;

    private float maxThrust = 3;
    private float maxRotationSpeed = 40;

    private Rigidbody2D rigidbody;

    private float speed;

    private float targetRotation;

    void Start () {
        rigidbody = GetComponent<Rigidbody2D>();
    }
	
	void FixedUpdate () {

        //transform.localEulerAngles = new Vector3(0, 0, (360 * rotate) - 180);

        if (transform.localEulerAngles.z == targetRotation) {
            return;
        }

        Debug.Log(transform.localEulerAngles.z + " : " + targetRotation);
        float diff = transform.localEulerAngles.z - targetRotation;

        if ((diff < 0 && diff > -180) || (diff > 0 && diff > 180)) {
            if (Math.Abs(diff) < maxRotationSpeed * Time.deltaTime) {
                transform.localEulerAngles = new Vector3(0, 0, targetRotation);
            } else {
                transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z + (maxRotationSpeed * Time.deltaTime));
            }
        } else {
            if (Math.Abs(diff) < maxRotationSpeed * Time.deltaTime) {
                transform.localEulerAngles = new Vector3(0, 0, targetRotation);
            } else {
                transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z - (maxRotationSpeed * Time.deltaTime));
            }
        }

        rigidbody.velocity = transform.up * (speed * maxThrust);

    }

    public void SetVelocity(float value) {
        speed = value;
        rigidbody.velocity = transform.up * (speed * maxThrust);
    }

    public void Rotate(float rotate) {
        targetRotation = (360 * rotate) - 180;
        if (targetRotation < 0)
            targetRotation = 360 + targetRotation;
    }

    public void SetShieldsState(float value) {
        if (value == 0) {
            Shield.SetActive(false);
        } else {
            Shield.SetActive(true);
        }
    }
}
