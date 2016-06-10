using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public GameObject Shield;

    private float maxThrust = 3;

    private Rigidbody2D rigidbody;

    private float speed;

    void Start () {
        rigidbody = GetComponent<Rigidbody2D>();
    }
	
	void Update () {
	
	}

    public void SetVelocity(float value) {
        speed = value;
        rigidbody.velocity = transform.up * (speed * maxThrust);
    }

    public void Rotate(float rotate) {
        transform.localEulerAngles = new Vector3(0, 0, (360 * rotate) - 180);
        rigidbody.velocity = transform.up * (speed * maxThrust);
    }

    public void SetShieldsState(float value) {
        if (value == 0) {
            Shield.SetActive(false);
        } else {
            Shield.SetActive(true);
        }
    }
}
