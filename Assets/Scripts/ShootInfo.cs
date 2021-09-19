using UnityEngine;


[System.Serializable]
public class ShootInfo {
  public float angle;
  public float v0y;
  public float v0z;

  private static float EPSILON = 1.0f;
  private static float G = 10.0f;

  public Vector3 GetPos (float t) {
    float radAngle = angle * Mathf.Deg2Rad;

    float z = v0z * t * Mathf.Cos(radAngle);
    float y = v0y * t * Mathf.Sin(radAngle) - 0.5f * G * t * t;
    return new Vector3(0.0f, y, z);
  }

  public float GetCurrentAngle (float t) {
    Vector3 a = GetPos(t + EPSILON) - GetPos(t - EPSILON);
    return Mathf.Atan2(a.z, a.y) * Mathf.Rad2Deg - 90.0f;
  }
}
