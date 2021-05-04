using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildGenerator : MonoBehaviour {
    public int XSize = 100;
    public int ZSize = 100;

    public float radius = 7;
    public float radiusForest = 1f;
    public int rejectionSamples = 1;
    public int rejectionForest = 1;

    public float displayRadius = 1;
    public float heightSpown = 40;

    //public GameObject terrain;
    public MapGenerator MapGenerator;

    public Transform treeAsset;
    public Transform treeParent;
    
    private Vector3 originV;
    private Vector3 heightV;
    private Vector3 centerV;
    private Vector3 sizeV;

    List<Vector3> spownPoints;
    List<Vector3> spownforest;
    List<Vector3> spownPointsOnMesh;

    private List<Vector3> terrainPoints;
    
    void OnValidate() {
        spownForest();
    }
    
    private void Awake() {
        spownForest();
    }

    private void spownForest() {
        if (rejectionSamples<1)
            rejectionSamples = 1;
        if (rejectionSamples>5) 
            rejectionSamples = 5;
        float minimumRadius = 2 * rejectionSamples;
        if (radius<minimumRadius)
            radius = minimumRadius;
        
        
        if (rejectionForest<1)
            rejectionForest = 1;
        if (rejectionForest>5) 
            rejectionForest = 5;
        float minimumRadiusForest = 2 * rejectionForest;
        if (radiusForest<minimumRadiusForest)
            radiusForest = minimumRadiusForest;
        
        originV = Vector3.zero;
        heightV = new Vector3(0, heightSpown, 0);
        sizeV = new Vector3(XSize, 0, ZSize);
        centerV = heightV + sizeV  / 2;
        
        terrainPoints = MapGenerator.GenerateMapPoints();
        spownforest = PoissonDiscSamplingMap.GeneratePoints(radius, sizeV, centerV, terrainPoints, rejectionSamples);

        spownPoints = new List<Vector3>();
        foreach (Vector3 spown in spownforest) {
            List<Vector3> list = PoissonDiscSamplingMap.GeneratePoints(radiusForest, sizeV, spown, terrainPoints, rejectionForest);
            foreach (Vector3 tree in list) {
                if (treeAsset != null)
                    spownPoints.Add(tree);
            }      
        }
    }
    
    private void Start() {
        //spownForest();
        if (spownPoints != null) {
            foreach (Vector3 point in spownPoints) {
                Instantiate(treeAsset, point, treeAsset.rotation, treeParent);
            }
        }
    }
    
    void OnDrawGizmos() {
        //spownPoints = new List<Vector3>();
//        Gizmos.DrawWireCube(centerV, sizeV);
//        if (spownPoints != null) {
//            foreach (Vector3 point in spownPoints) {
//                Gizmos.color = Color.red;
//                Gizmos.DrawSphere(point, displayRadius);
//            }
//        }
    }
    
}
