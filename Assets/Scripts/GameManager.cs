using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public SerialPortManager serialPortManager;

    public Player player;

    private bool hasGameStarted = false;
    private bool isKeySwitchActive = false;

    public void Execute(string commandString) {
        string[] commandParts = commandString.Split(':');
        string command = commandParts[0];
        float value = float.Parse(commandParts[1]);

        Debug.Log(value);

        switch(command) {
            case "SLIDER":
                if (!hasGameStarted) return;
                player.SetVelocity(value);
                break;
            case "ROTARY":
                if (!hasGameStarted) return;
                player.Rotate(value);
                break;
            case "TEST_SWITCH":
                if (!hasGameStarted) return;
                player.SetShieldsState(value);
                if (value == 0) {
                    serialPortManager.AddToArduinoQueue("SHLD:0");
                } else {
                    serialPortManager.AddToArduinoQueue("SHLD:1");
                }
                break;
            case "KEY_SWITCH":
                if (value == 1) {
                    isKeySwitchActive = true;
                    serialPortManager.AddToArduinoQueue("ENG:1");
                } else {
                    isKeySwitchActive = false;
                    serialPortManager.AddToArduinoQueue("ENG:0");
                }
                break;
            case "START_BTN":
                if (!hasGameStarted && isKeySwitchActive && value == 1) {
                    // Start
                    StartGame();
                }
                break;
        }
    }

    public void StartGame() {
        // Turn some LEDS on
        hasGameStarted = true;
    }
}
