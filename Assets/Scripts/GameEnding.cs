using UnityEngine;
using System.Collections;

public class GameEnding : MonoBehaviour {
  public void LoadStartDelayed () {
    StartCoroutine("DelayStart");
  }

  private IEnumerator DelayStart () {
    yield return new WaitForSeconds (4.0f);
    Application.LoadLevel("Start");
  }
}
