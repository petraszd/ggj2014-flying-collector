using UnityEngine;
using System.Collections;

public class InitGame : MonoBehaviour {

	void Update () {
    if (Input.anyKeyDown) {
      Application.LoadLevel("Main");
    }
	}
}
