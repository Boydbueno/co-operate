using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour {

    public GameObject Shield;

    private bool isKeySwitchActive = false;
    private bool isReactorActive = false;

    private float maxThrust = 3;
    private float boostThrust = 9;
    private float currentThrust = 0;
    private float targetThrust = 0;
    private bool isBoostActive = false;

    private float maxRotationSpeed = 40;

    private float targetRotation;

    private SerialPortManager serialPortManager;

    void Start () {
        serialPortManager = SerialPortManager.Instance;

        serialPortManager.onApplicationStop += DeactivateAllSystems;
    }
	
	void FixedUpdate () {
        if (transform.localEulerAngles.z != targetRotation && isReactorActive) {
            HandleRotation();
        }

        if (currentThrust != targetThrust) {
            // Todo: Build this up
            currentThrust = targetThrust;
        }

        if (isBoostActive && isReactorActive) {
            transform.position += transform.up * boostThrust * Time.deltaTime;
        } else if (currentThrust != 0 && isReactorActive) {
            transform.position += transform.up * currentThrust * Time.deltaTime;
        }
    }

    public void SetThrust(float value) {
        targetThrust = value * maxThrust;
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
