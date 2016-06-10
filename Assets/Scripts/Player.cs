using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    private float thrustForce = 3;

    private Rigidbody2D rigidbody;

    void Start () {
        rigidbody = GetComponent<Rigidbody2D>();
    }
	
	void Update () {
	
	}

    public void SetVelocity(float value) {
        rigidbody.velocity = new Vector2(0, value);
    }
}
