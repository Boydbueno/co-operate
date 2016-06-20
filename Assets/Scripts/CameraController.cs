using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public Ship player;
    	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z); ;
	}
}
