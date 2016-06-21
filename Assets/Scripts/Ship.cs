using UnityEngine;
using System.Collections;
using System;

public class Ship : MonoBehaviour {

    public GameObject Shield;

    private bool isKeySwitchActive = false;
    public bool isReactorActive = false;

    private float maxThrust = 4;

    private float boostThrust = 15;

    private bool isBoostActive = false;

    private float maxRotationSpeed = 40;

    private float targetRotation;

    public float thrust = 0;

    private SerialPortManager serialPortManager;
    private Rigidbody2D rigidbody;


    void Start () {
        serialPortManager = SerialPortManager.Instance;
        rigidbody = GetComponent<Rigidbody2D>();

        serialPortManager.onApplicationStop += DeactivateAllSystems;
    }

	void FixedUpdate () {
        if (transform.localEulerAngles.z != targetRotation && isReactorActive) {
            HandleRotation();
        }

        if (isReactorActive) {
            if (isBoostActive) {
                rigidbody.AddRelativeForce(new Vector2(0, boostThrust));
            } else {
                rigidbody.AddRelativeForce(new Vector2(0, thrust));
            }
        }
    }

    public void SetThrust(float value) {
        thrust = value * maxThrust;
    }

    public void ModifyThrust(float value) {
        if ((value > 0 && thrust == maxThrust) || (value < 0 && thrust == 0)) return;

        thrust += value;

        if (thrust > maxThrust) {
            thrust = maxThrust;
        } else if (thrust < 0) {
            thrust = 0;
        }
    }

    public void ModifyRotation(float value) {
        targetRotation += value;
    }

    public void Rotate(float rotate) {
        targetRotation = (360 * rotate) - 180;
        if (targetRotation < 0)
            targetRotation = 360 + targetRotation;
    }

    public void SetShieldsState(float value) {
        if (value == 0) {
            serialPortManager.AddToSerialQueue("SHLD:0");
            Shield.SetActive(false);
        } else {
            serialPortManager.AddToSerialQueue("SHLD:1");
            Shield.SetActive(true);
        }
    }

    public void SetKeySwitchState(float value) {
        if (value == 1) {
            isKeySwitchActive = true;
            serialPortManager.AddToSerialQueue("ENG:1");
        } else {
            isKeySwitchActive = false;
        }

        if (value == 0 && !isReactorActive) {
            serialPortManager.AddToSerialQueue("ENG:0");
        }
    }

    public void SetEngineState(float value) {
        if (!isReactorActive && isKeySwitchActive && value == 1) {
            isReactorActive = true;
        }
    }

    public void SetBoostState(float value) {
        if (value == 0) {
            DeactivateBoost();
        } else {
            ActivateBoost();
        }
    }

    private void ActivateBoost() {
        isBoostActive = true;
    }

    private void DeactivateBoost() {
        isBoostActive = false;
    }

    private void HandleRotation() {
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
    }

    private void DeactivateAllSystems() {
        DeactivateBoost();
        isReactorActive = false;
        SetEngineState(0);
        SetKeySwitchState(0);
        SetShieldsState(0);
    }
}
