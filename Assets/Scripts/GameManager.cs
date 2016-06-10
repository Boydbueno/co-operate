using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public SerialPortManager serialPortManager;

    public Player player;

    public void Execute(string commandString) {
        string[] commandParts = commandString.Split(':');
        string command = commandParts[0];
        float value = float.Parse(commandParts[1]);
        Debug.Log(command);
        Debug.Log(value);

        switch(command) {
            case "SLIDER":
                player.SetVelocity(value);
                break;
        }
    }
}
