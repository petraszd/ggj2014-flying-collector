using UnityEngine;
using System.Collections;


public class Navigator : MonoBehaviour {
  public float moveSpeed;
  public Transform plane;

  public bool IsWin = false;

	void Update () {
    float x = Input.GetAxis("Horizontal");
    float y;
    if (IsWin) {
      y = 1.0f;
    } else {
      y = Input.GetAxis("Vertical");
    }

    Vector3 direction = new Vector3(x, y, 0.0f);
    transform.Translate(direction * moveSpeed * Time.deltaTime);

    Vector3 delta = new Vector3(-y * 5.0f, 0.0f, -x * 20.0f);
    plane.localEulerAngles = delta;
    Camera.main.transform.localEulerAngles = 0.5f * -delta;
	}
}
