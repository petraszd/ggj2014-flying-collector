using UnityEngine;
using System.Collections;


public class PlayerFlying : MonoBehaviour {
  public float speed;
  public GUIText heightText;
  public ShootInfo sInfo;

  private float t;

  void Start () {
    t = 0.0f;
  }

  void Update () {
    UpdatePosition();
    UpdateCameraRotation();

    t += Time.deltaTime * speed;
  }

  public ShootInfo GetSInfo () {
    return sInfo;
  }

  private void UpdatePosition () {
    transform.position = sInfo.GetPos(t);

    // TODO: get 500.0f from ground
    float h = GetHeight();
    if (h < 0.0f) {
      h = 0.0f;
    }
    heightText.text = "" + h;
  }

  private void UpdateCameraRotation () {
    float angle = sInfo.GetCurrentAngle(t);
    Camera.main.transform.eulerAngles = new Vector3(angle, 0, 0);
  }

  public float GetHeight () {
    Transform navigator = transform.GetChild(0);
    return navigator.position.y + 450.0f;  // TODO: magic numbers
  }
}
