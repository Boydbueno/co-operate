using UnityEngine;
using System.Collections;
using System;

public class Ship : MonoBehaviour {

    public GameObject Shield;
    public GameObject ThrusterPortside;
    public ParticleSystem PortFlare;
    public ParticleSystem PortCore;
    public GameObject ThrusterStarboardside;
    public ParticleSystem StarboardFlare;
    public ParticleSystem StarboardCore;
    public GameObject BoostThruster;

    public GameObject Gun;
    public GameObject LaserBullet;
    public float targetGunRotation;
    private float maxGunRotationSpeed = 20;

    private float flareMinSize = 0.5f;
    private float flareGrowth = 0.3f;
    private float coreMinSize = 0.4f;
    private float coreGrowth = 0.9f;

    private bool isKeySwitchActive = false;
    private bool isKeySwitch2Active = false;
    private bool isReactorActive = false;

    private float maxThrust = 2;

    private float boostThrust = 20;

    private bool isBoostActive = false;

    public float thrustPort = 0;
    public float thrustStarboard = 0;

    private SerialPortManager serialPortManager;
    private Rigidbody2D rigidbody;

    void Start () {
        serialPortManager = SerialPortManager.Instance;
        rigidbody = GetComponent<Rigidbody2D>();

        serialPortManager.onApplicationStop += DeactivateAllSystems;
    }

	void FixedUpdate () {
        if (isReactorActive) {

            if (Gun.transform.localEulerAngles.z != targetGunRotation) {
                HandleGunRotation();
            }

            if (isBoostActive) {
                rigidbody.AddForceAtPosition(boostThrust * transform.up, BoostThruster.transform.position, ForceMode2D.Force);
            } else {
                rigidbody.AddForceAtPosition(thrustPort * transform.up, ThrusterPortside.transform.position, ForceMode2D.Force);
                rigidbody.AddForceAtPosition(thrustStarboard * transform.up, ThrusterStarboardside.transform.position, ForceMode2D.Force);
            }
        }
    }

    public void Fire() {
        GameObject laser = (GameObject)Instantiate(LaserBullet, Gun.transform.position, Gun.transform.rotation);
        Rigidbody rigidbody = laser.GetComponent<Rigidbody>();
        rigidbody.AddForce(new Vector3(0, 15, 0));
    }

    public void RotateGun(float rotate) {
        if (rotate < 0.5f) { // right
            float diffPerc = rotate / 0.5f;
            targetGunRotation = 40 - (40 * diffPerc);
        } else { // left
            float diffPerc = (rotate - 0.5f) / 0.5f;
            targetGunRotation = 360 - (40 * diffPerc);
        }
    }

    private void HandleGunRotation() { 
        float diff = Gun.transform.localEulerAngles.y - targetGunRotation;

        if ((diff < 0 && diff > -180) || (diff > 0 && diff > 180)) {
            if (Math.Abs(diff) < maxGunRotationSpeed * Time.deltaTime) {
                Gun.transform.localEulerAngles = new Vector3(0, targetGunRotation, 0);
            } else {
                Gun.transform.localEulerAngles = new Vector3(0, Gun.transform.localEulerAngles.y + (maxGunRotationSpeed * Time.deltaTime), 0);
            }
        } else {
            if (Math.Abs(diff) < maxGunRotationSpeed * Time.deltaTime) {
                Gun.transform.localEulerAngles = new Vector3(0, targetGunRotation, 0);
            } else {
                Gun.transform.localEulerAngles = new Vector3(0, Gun.transform.localEulerAngles.y - (maxGunRotationSpeed * Time.deltaTime), 0);
            }
        }
    }


    public void SetPortThrust(float value) {
        UpdatePortThrust(value * maxThrust);
    }

    public void ModifyPortThrust(float value) {
        if ((value > 0 && thrustPort == maxThrust) || (value < 0 && thrustPort == 0)) return;

        UpdatePortThrust(thrustPort + value);

        if (thrustPort > maxThrust) {
            UpdatePortThrust(maxThrust);
        } else if (thrustPort < 0) {
            UpdatePortThrust(0);
        }
    }

    private void UpdatePortThrust(float value) {
        thrustPort = value;

        float thrustPercentage = value / maxThrust;

        PortFlare.startSize = flareMinSize + (flareGrowth * thrustPercentage);
        
        if (value > maxThrust * 0.25f) {
            PortCore.gameObject.SetActive(true);
            PortCore.startSize = coreMinSize + (coreGrowth * thrustPercentage);
        } else {
            PortCore.gameObject.SetActive(false);
        }
    }

    public void SetStarboardThrust(float value) {
        UpdateStartboardThrust(value * maxThrust);
    }

    public void ModifyStarboardThrust(float value) {
        if ((value > 0 && thrustStarboard == maxThrust) || (value < 0 && thrustStarboard == 0)) return;

        UpdateStartboardThrust(thrustStarboard + value);

        if (thrustStarboard > maxThrust) {
            UpdateStartboardThrust(maxThrust);
        } else if (thrustStarboard < 0) {
            UpdateStartboardThrust(0);
        }
    }

    private void UpdateStartboardThrust(float value) {
        thrustStarboard = value;

        float thrustPercentage = value / maxThrust;

        StarboardFlare.startSize = flareMinSize + (flareGrowth * thrustPercentage);

        if (value > maxThrust * 0.25f) {
            StarboardCore.gameObject.SetActive(true);
            StarboardCore.startSize = coreMinSize + (coreGrowth * thrustPercentage);
        } else {
            StarboardCore.gameObject.SetActive(false);
        }
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
            if (isKeySwitch2Active)
                serialPortManager.AddToSerialQueue("ENG:1");
        } else {
            isKeySwitchActive = false;
        }

        if (value == 0 && !isReactorActive) {
            serialPortManager.AddToSerialQueue("ENG:0");
        }
    }

    public void SetSecondKeySwitchState(float value) {
        if (value == 1) {
            isKeySwitch2Active = true;
            if (isKeySwitchActive)
                serialPortManager.AddToSerialQueue("ENG:1");
        } else {
            isKeySwitch2Active = false;
        }


        if (value == 0 && !isReactorActive) {
            serialPortManager.AddToSerialQueue("ENG:0");
        }
    }

    public void SetEngineState(float value) {
        if (!isReactorActive && isKeySwitchActive && value == 1) {
            isReactorActive = true;
            ActivateThrusters();
            ActivateGun();
        }
    }

    public void ActivateThrusters() {
        PortFlare.gameObject.SetActive(true);
        StarboardFlare.gameObject.SetActive(true);
    }

    public void ActivateGun() {
        Gun.gameObject.SetActive(true);
    }

    public void SetBoostState(float value) {
        if (value == 0) {
            DeactivateBoost();
        } else {
            ActivateBoost();
        }
    }

    public void ToggleBoost() {
        if (isBoostActive) {
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

    private void DeactivateAllSystems() {
        DeactivateBoost();
        isReactorActive = false;
        SetEngineState(0);
        SetKeySwitchState(0);
        SetShieldsState(0);
    }
}
