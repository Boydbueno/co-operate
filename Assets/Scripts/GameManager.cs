using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public Ship player;

    private SerialPortManager serialPortManager;

    void Start() {
        serialPortManager = SerialPortManager.Instance;
    }

    void Update() {
        if (Input.GetKeyDown("space")) {
            player.SetBoostState(1);
        } 

        if (Input.GetKeyDown("backspace")) {
            player.SetBoostState(0);
        }
    }

    public void Execute(string commandString) {
        string[] commandParts = commandString.Split(':');
        string command = commandParts[0];
        float value = float.Parse(commandParts[1]);

        Debug.Log(value);

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
