using UnityEngine;
using System.Collections;

public class Propeller : MonoBehaviour {
	void Start () {
	}

  void Update () {
    Transform mesh = transform.GetChild(0);
    mesh.Rotate(new Vector3(0.0f, 0.0f, 1.0f) * Time.deltaTime * 400.0f);
  }
}
