using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System;

public class SerialPortManager : MonoBehaviour {

    public GameManager gameManager;

    private SerialPort stream;
    private string queuedActions = "";

	void Start() {
        stream = new SerialPort("COM3", 9600);
        stream.ReadTimeout = 100;
        stream.Open();

        stream.WriteLine("START");
        stream.BaseStream.Flush();

        InvokeRepeating("ReadFromArduino", 1, 1);
    }
	
	void Update() {

    }

    void OnApplicationQuit() {
        stream.WriteLine("STOP");
        stream.BaseStream.Flush();
        stream.Close();
    }

    public void ReadFromArduino() {
        stream.WriteLine("R");
        stream.ReadTimeout = 50;
        try {
            // Handle the response
            string[] messages = stream.ReadLine().Split(',');
            foreach(string message in messages) {
                gameManager.Execute(message);
            }
        } catch (TimeoutException) {
        }
        if (queuedActions != "") {
            stream.WriteLine("A " + queuedActions);
            queuedActions = "";
        }
    }

    public void AddToArduinoQueue(string action) {
        queuedActions = queuedActions + action + " ";
    }
}
