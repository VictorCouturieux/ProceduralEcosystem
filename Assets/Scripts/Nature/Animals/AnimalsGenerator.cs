using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalsGenerator : MonoBehaviour
{
    public int XSize = 100;
    public int ZSize = 100;

    public float radius = 1;
    public int rejectionSamples = 30;

    public float displayRadius = 1;
    public float heightSpown = 40;

    //public GameObject terrain;
    public MapGenerator MapGenerator;

    public Transform animalsAsset;
    public Transform animalsParent;
    
    private Vector3 originV;
    private Vector3 heightV;
    private Vector3 centerV;
    private Vector3 sizeV;

    List<Vector3> spownPoints;
    List<Vector3> spownPointsOnMesh;

    private List<Vector3> terrainPoints;
    
    void OnValidate() {
        originV = Vector3.zero;
        heightV = new Vector3(0, heightSpown, 0);
        sizeV = new Vector3(XSize, 0, ZSize);
        centerV = heightV + sizeV  / 2;

        terrainPoints = MapGenerator.GenerateMapPoints();
        spownPoints = PoissonDiscSamplingMap.GeneratePoints(radius, sizeV, centerV, terrainPoints, rejectionSamples);

    }
    
    private void Awake() {
        originV = Vector3.zero;
        heightV = new Vector3(0, heightSpown, 0);
        sizeV = new Vector3(XSize, 0, ZSize);
        centerV = heightV + sizeV  / 2;
        
        spownPoints = PoissonDiscSamplingMap.GeneratePoints(radius, sizeV, centerV, terrainPoints, rejectionSamples);
    }
    
    private void Start() {
        if (spownPoints != null) {
            foreach (Vector3 point in spownPoints) {
                Instantiate(animalsAsset, point, animalsAsset.rotation, animalsParent);
            }
        }
    }
    
    void OnDrawGizmos() {
        Gizmos.DrawWireCube(centerV, sizeV);
        if (spownPoints != null) {
            foreach (Vector3 point in spownPoints) {
                Gizmos.DrawSphere(point, displayRadius);
            }
        }
    }
}
