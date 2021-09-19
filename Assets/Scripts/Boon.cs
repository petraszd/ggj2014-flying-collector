using UnityEngine;
using System.Collections;

public class Boon : MonoBehaviour {
  private Vector3 rotAxis;
  private float tumble;
  private bool isFlyAway;
  private Vector3 flyAwayDir;

  void Start () {
    rotAxis = Random.onUnitSphere;
    isFlyAway = false;
  }

	void Update ()
	{
    if (isFlyAway) {
      FlyAway();
    }

    transform.RotateAround(
        transform.position,
        rotAxis,
        360.0f * Time.deltaTime);
    if (transform.position.z < Camera.main.transform.position.z) {
      Destroy(gameObject);
    }
	}

  public void StartFlyAway () {
    isFlyAway = true;
    flyAwayDir = Random.onUnitSphere;
  }

  private void FlyAway () {
    transform.Translate(flyAwayDir * 400.0f * Time.deltaTime, Space.World);
  }

  //IEnumerator
}
