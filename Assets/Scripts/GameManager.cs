using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public Ship Ship;

    void Update() {
        if (Input.GetButtonDown("start")) {
            Ship.SetKeySwitchState(1);
            Ship.SetEngineState(1);
        }

        if (Input.GetAxisRaw("vertical") != 0 || Input.GetAxis("verticalAxis") != 0 || Input.GetAxis("verticalLeftAnalog") != 0) {
            float value = (Input.GetAxisRaw("vertical") + Input.GetAxis("verticalAxis") + Input.GetAxis("verticalLeftAnalog")) * Time.deltaTime * 2;
            Ship.ModifyStarboardThrust(value);
        }

        if (Input.GetAxisRaw("modifyValue") != 0) {
            Ship.ModifyPortThrust(Input.GetAxisRaw("modifyValue") * Time.deltaTime * 2);
        }

        if (Input.GetButtonDown("boost")) {
            Ship.ToggleBoost();
        }

        if (Input.GetButtonDown("fire")) {
            Ship.Fire();
        }

        if (Input.GetButtonDown("keyswitch")) {
            Ship.SetSecondKeySwitchState(1);
        }

        if (Input.GetButtonUp("keyswitch")) {
            Ship.SetSecondKeySwitchState(0);
        }
    }

    public void Execute(string commandString) {
        string[] commandParts = commandString.Split(':');
        string command = commandParts[0];
        float value = float.Parse(commandParts[1]);
        Debug.Log(command);
        switch(command) {
            case "SLIDER":
                Ship.SetPortThrust(value);
                break;
            case "BOOST_SWITCH":
                Ship.SetBoostState(value);
                break;
            case "ROTARY":
                Ship.RotateGun(value);
                break;
            case "TEST_SWITCH":
                Ship.SetShieldsState(value);
                break;
            case "KEY_SWITCH":
                Ship.SetKeySwitchState(value);
                break;
            case "START_BTN":
                Ship.SetEngineState(value);
                break;
        }
    }
}
