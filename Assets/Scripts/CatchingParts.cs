using UnityEngine;
using System.Collections;

public class CatchingParts : MonoBehaviour {
  public AudioClip audioCorrect;
  public AudioClip audioIncorrect;
  public AudioClip audioPick;
	public AudioClip audioWin;
	public AudioClip audioLoss;

  private string selectedTag = "";

  public GUITexture winGUI;
  public GUITexture lossGUI;

  public Transform partsHolder;

  public BoonGenerator boonGen;

  public ParticleSystem[] pSystems;

  private GameObject body = null;
  private GameObject tail = null;
  private GameObject propeller = null;
  private GameObject wings = null;

  private bool isWin = false;

  void LateUpdate () {
    PlayerFlying pf = gameObject.GetComponent<PlayerFlying>();
    if (pf.GetHeight() < 0.0f) {
      OnGameLose();
    }
  }

  void OnTriggerEnter(Collider other) {
    if (selectedTag == "" && other.tag.Length > 6 && other.tag[6] == 'f') {
      OnFullPlaneHit(other.gameObject);
    } else if (other.tag == selectedTag) {
      OnGoodCollision(other.gameObject);
    } else {
      OnBadHit(other.gameObject);
    }
  }

  void OnGoodCollision (GameObject obj) {
    wings = CheckForFigures(wings, obj, "Wings");
    tail = CheckForFigures(tail, obj, "Tail");
    propeller = CheckForFigures(propeller, obj, "Propeller");
    body = CheckForFigures(body, obj, "Body");

    CheckForWinStatus();
  }

  GameObject CheckForFigures (GameObject reqObj, GameObject obj, string reqName) {
    if (reqObj == null && obj.name == reqName) {
      audio.PlayOneShot (audioCorrect, 1.0f);
      ResetChild(obj.transform);
      return obj;
    }
    return reqObj;
  }

  private void ResetChild(Transform newChild) {
    Destroy(newChild.GetComponent<Boon>());
    newChild.parent = partsHolder;

    // This is horrible
    Transform replacable = partsHolder.parent.GetChild(0)
      .Find(newChild.gameObject.name);
    Destroy(replacable.gameObject);

    newChild.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
    newChild.localEulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
    newChild.localScale = new Vector3(1.0f, 1.0f, 1.0f);
  }

  private int GetLeftPartsCounts () {
    int nLeft = 0;
    if (body == null) {
      nLeft++;
    }
    if (tail == null) {
      nLeft++;
    }
    if (propeller == null) {
      nLeft++;
    }
    if (wings == null) {
      nLeft++;
    }
    return nLeft;
  }

  private void CheckForWinStatus () {
    if (GetLeftPartsCounts() == 0) {
      OnGameWin();
    }
  }

  private void OnGameLose () {
    if (!isWin) {
      lossGUI.enabled = true;
      audio.PlayOneShot(audioLoss, 1.0f);
    }

    Destroy(this);
    Destroy(gameObject.GetComponent<PlayerFlying>());
    Destroy(transform.GetComponentInChildren<Navigator>());

    FinishGame();
  }

  private void OnFullPlaneHit (GameObject obj) {
    boonGen.StopGeneratingFullPlanes();
    selectedTag = obj.tag.Substring(0, 6);  // Stupid. Removes 'f' at the end
    audio.PlayOneShot(audioPick, 1.0f);

    NoticeFullPlaneBoonsToFlyAway("Plane1f");
    NoticeFullPlaneBoonsToFlyAway("Plane2f");
    NoticeFullPlaneBoonsToFlyAway("Plane3f");

    MeshRenderer renderer = (MeshRenderer) obj
      .GetComponentInChildren(typeof(MeshRenderer));
    StartFlareParticles(renderer.material.color);
  }

  private void OnBadHit (GameObject obj) {
    audio.PlayOneShot(audioIncorrect, 1.0f);
  }

  private void OnGameWin () {
    isWin = true;
    winGUI.enabled = true;
    StopFlareParticles();
    audio.PlayOneShot(audioWin, 1.0f);

    Propeller p = GetComponentInChildren<Propeller>();
    p.enabled = true;

    Navigator navigator = GetComponentInChildren<Navigator>();
    navigator.IsWin = true;

    FinishGame();
  }

  private void NoticeFullPlaneBoonsToFlyAway (string tag) {
    foreach (GameObject obj in GameObject.FindGameObjectsWithTag(tag)) {
      Boon boon = obj.GetComponent<Boon>();
      boon.StartFlyAway();
    }
  }

  private void StartFlareParticles (Color pColor) {
    foreach (ParticleSystem ps in pSystems) {
      ps.startColor = pColor;
      ps.Play();
    }
  }

  private void StopFlareParticles () {
    foreach (ParticleSystem ps in pSystems) {
      ps.Stop();
    }
  }

  private void FinishGame () {
    GameEnding end = Camera.main.GetComponent<GameEnding>();
    end.LoadStartDelayed();
  }
}
