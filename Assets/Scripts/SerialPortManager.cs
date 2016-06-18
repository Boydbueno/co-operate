using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System;

public class SerialPortManager : MonoBehaviour {

    public GameManager gameManager;

    private SerialPort stream;
    private string queuedActions = "";

    public static SerialPortManager Instance { get; private set; }

    public delegate void OnApplicationStop();
    public OnApplicationStop onApplicationStop;

    void Awake() {
        // First we check if there are any other instances conflicting
        if (Instance != null && Instance != this) {
            // If that is the case, we destroy other instances
            Destroy(gameObject);
        }

        // Here we save our singleton instance
        Instance = this;
    }

	void Start() {
        stream = new SerialPort("COM3", 9600);
        stream.ReadTimeout = 100;
        stream.Open();

        stream.WriteLine("STRT");
        stream.BaseStream.Flush();

        InvokeRepeating("ReadFromArduino", 1, 1);
    }
	
    void OnApplicationQuit() {
        onApplicationStop();
        AddToSerialQueue("STOP");
        ReadFromArduino();
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

    public void AddToSerialQueue(string action) {
        queuedActions = queuedActions + action + " ";
    }
}
