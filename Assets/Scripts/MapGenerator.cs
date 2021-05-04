using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {

//	public enum DrawMode {NoiseMap, ColourMap, Mesh};
//	public DrawMode drawMode;

	public int mapChunkSize = 241;
	[Range(0,6)]
	public int levelOfDetail;
	public float noiseScale;

	public int octaves;
	[Range(0,1)]
	public float persistance;
	public float lacunarity;

	public int seed;
	public Vector2 offset;

	public float uniformScale = 2.5f;
	
	public float meshHeightMultiplier;
	public AnimationCurve meshHeightCurve;

	public float minHeight {
		get { return uniformScale *  meshHeightMultiplier * meshHeightCurve.Evaluate(0); }
	}

	public float maxHeight {
		get { return uniformScale *  meshHeightMultiplier * meshHeightCurve.Evaluate(1); }
	}
	
	public Material terrainMaterial;
	public Color[] baseColors;
	[Range(0,1)]
	public float[] baseStartHeight;
	[Range(0,1)]
	public float[] baseBlends;
	
	private Vector3 originV;
	private Vector3 heightV;
	private Vector3 centerV;
	private Vector3 sizeV;

//	List<Vector3> terrainPoints;
//	List<Vector3> spownPoints;
	
	public float radius = 1;
	public int rejectionSamples = 30;

	public float displayRadius = 1;
	public float heightSpown = 40;
	
	public bool autoUpdate;

//	public TerrainType[] regions;

	public void GenerateMap() {
		float[,] noiseMap = Noise.GenerateNoiseMap (mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset);

//		Color[] colourMap = new Color[mapChunkSize * mapChunkSize];
//		for (int y = 0; y < mapChunkSize; y++) {
//			for (int x = 0; x < mapChunkSize; x++) {
//				float currentHeight = noiseMap [x, y];
//				for (int i = 0; i < regions.Length; i++) {
//					if (currentHeight <= regions [i].height) {
//						colourMap [y * mapChunkSize + x] = regions [i].colour;
//						break;
//					}
//				}
//			}
//		}
//		MapDisplay display = FindObjectOfType<MapDisplay> ();
//		if (drawMode == DrawMode.NoiseMap) {
//			display.DrawTexture (TextureGenerator.TextureFromHeightMap (noiseMap));
//		} else if (drawMode == DrawMode.ColourMap) {
//			display.DrawTexture (TextureGenerator.TextureFromColourMap (colourMap, mapChunkSize, mapChunkSize));
//		} else if (drawMode == DrawMode.Mesh) {
//			display.DrawMesh (MeshGenerator.GenerateTerrainMesh (noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail), TextureGenerator.TextureFromColourMap (colourMap, mapChunkSize, mapChunkSize));
//		}
		
		MapDisplay display = FindObjectOfType<MapDisplay> ();
		display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail));
		if (terrainMaterial!=null) {
//				Debug.Log(minHeight + " : " + maxHeight);
			TextureGenerator.UpdateMeshHeights(terrainMaterial, minHeight, maxHeight, baseColors, baseStartHeight, baseBlends);
		}
		
//		originV = Vector3.zero;
//		heightV = new Vector3(0, heightSpown, 0);
//		sizeV = new Vector3(mapChunkSize, 0, mapChunkSize);
//		centerV = heightV + sizeV  / 2;
//            terrain.transform.position = centerV;
		
		//terrainPoints = MeshGenerator.GenerateTerrainPoints(noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail);
		//spownPoints = PoissonDiscSampling.GeneratePoints(radius, sizeV, centerV, terrainPoints, rejectionSamples);
		
	}

	public List<Vector3> GenerateMapPoints() {
		float[,] noiseMap = Noise.GenerateNoiseMap (mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset);
		return MeshGenerator.GenerateTerrainPoints(noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail);
	}

	void OnDrawGizmos() {
//		Gizmos.DrawWireCube(centerV, sizeV);
//		if (spownPoints != null) {
//			foreach (Vector3 point in spownPoints) {
//				Gizmos.DrawSphere(point, displayRadius);
//			}
//		}
	}
	
	void OnValidate() {
		if (lacunarity < 1) {
			lacunarity = 1;
		}
		if (octaves < 0) {
			octaves = 0;
		}
		if (radius<=0) {
			radius = 1f;
		}
		
		TextureGenerator.UpdateMeshHeights(terrainMaterial, minHeight, maxHeight, baseColors, baseStartHeight, baseBlends);
		
	}
}

//[System.Serializable]
//public struct TerrainType {
//	public string name;
//	public float height;
//	public Color colour;
//}