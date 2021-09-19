using UnityEngine;
using System.Collections;

public class BoonGenerator : MonoBehaviour {
  public GameObject[] boonsPrototypes;
  public Material[] materials;
  public float stepSize;
  public int randomPerLine;
  public float randomRadius;
  public Transform player;

  private PlayerFlying playerFlying;
  private Vector3 genPos;
  private float genT;

  private bool isGenerateFullPlanes;

  void Start () {
    playerFlying = (PlayerFlying) player.GetComponent<PlayerFlying>();
    genPos = Vector3.zero;
    genT = 0.0f;
    isGenerateFullPlanes = true;

    StartCoroutine("GenerateBoons");
  }

  public void StopGeneratingFullPlanes () {
    isGenerateFullPlanes = false;
  }

  private IEnumerator GenerateBoons () {
    while (IsNeedForBoonsBatch()) {
      genPos = playerFlying.GetSInfo().GetPos(genT);
      for (int i = 0; i < Random.Range(1, randomPerLine); ++i) {
        GenerateBoon(genPos);
      }
      genT += stepSize;
    }

    yield return new WaitForSeconds(0.1f);
    if (IsNeedForBoonsAtAll()) {
      StartCoroutine("GenerateBoons");
    }
  }

  private void GenerateBoon (Vector3 pos) {
    int modelIndex = GetObjectIndex();
    int colorIndex = GetColorIndex();

    GameObject proto = boonsPrototypes[modelIndex];

    GameObject boon = (GameObject) Instantiate(
        proto, pos, Quaternion.identity);
    boon.name = proto.name;
    boon.AddComponent("Boon");

    boon.transform.parent = transform;
    boon.tag = "Plane" + (colorIndex + 1);
    if (modelIndex == 0) {
      boon.tag += "f";
    }

    boon.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
    boon.transform.Translate(Random.insideUnitSphere * randomRadius);
    AssignMaterial(boon.transform, materials[colorIndex]);
  }

  private void AssignMaterial (Transform boonTr, Material mat) {
    // This is stupip. But it works and now it is GameJam.
    // So, I am leaving this here...
    foreach (Transform holder in boonTr) {
      foreach (Transform mesh in holder) {
        mesh.gameObject.renderer.material = mat;
      }
    }
  }

  private bool IsNeedForBoonsBatch () {
    return genPos.z < player.position.z + 500.0f;
  }

  private bool IsNeedForBoonsAtAll () {
    return genPos.y >= -500.0f;  // TODO: extract magic number
  }

  private int GetColorIndex () {
    return Random.Range(0, materials.Length);
  }

  private int GetObjectIndex () {
    if (isGenerateFullPlanes) {
      return 0;
    }
    return Random.Range(1, boonsPrototypes.Length);
  }
}
