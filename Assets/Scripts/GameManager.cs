using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public Ship player;

    private SerialPortManager serialPortManager;

    void Start() {
        serialPortManager = SerialPortManager.Instance;
    }

    void Update() {
        if (Input.GetButtonDown("start")) {
            player.SetKeySwitchState(1);
            player.SetEngineState(1);
        }

        if (Input.GetAxisRaw("vertical") != 0 || Input.GetAxis("verticalAxis") != 0 || Input.GetAxis("verticalLeftAnalog") != 0) {
            float value = (Input.GetAxisRaw("vertical") + Input.GetAxis("verticalAxis") + Input.GetAxis("verticalLeftAnalog")) * Time.deltaTime * 2;
            player.ModifyThrust(value);
        }

        if (Input.GetAxisRaw("horizontal") != 0 || Input.GetAxis("horizontalAxis") != 0 || Input.GetAxis("horizontalLeftAnalog") != 0) {
            float value = (Input.GetAxisRaw("horizontal") + Input.GetAxis("horizontalAxis") + Input.GetAxis("horizontalLeftAnalog")) * Time.deltaTime * 50;
            player.ModifyRotation(value);
        }
    }

    public void Execute(string commandString) {
        string[] commandParts = commandString.Split(':');
        string command = commandParts[0];
        float value = float.Parse(commandParts[1]);

        switch(command) {
            case "SLIDER":
                player.SetThrust(value);
                break;
            case "BOOST_SWITCH":
                player.SetBoostState(value);
                break;
            case "ROTARY":
                player.Rotate(value);
                break;
            case "TEST_SWITCH":
                player.SetShieldsState(value);
                break;
            case "KEY_SWITCH":
                player.SetKeySwitchState(value);
                break;
            case "START_BTN":
                player.SetEngineState(value);
                break;
        }
    }
}
