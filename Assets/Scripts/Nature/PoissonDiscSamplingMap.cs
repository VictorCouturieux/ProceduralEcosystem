using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class PoissonDiscSamplingMap {

	public static List<Vector3> GeneratePoints(float radius, 
		Vector3 sampleRegionSize, 
		Vector3 sampleRegionCenter, 
		List<Vector3> terrainPoints,
		int numSamplesBeforeRejection = 30) {
		
		float cellSize = radius/Mathf.Sqrt(2);

		int[,] grid = new int[Mathf.CeilToInt(sampleRegionSize.x/cellSize), Mathf.CeilToInt(sampleRegionSize.z/cellSize)];
		List<Vector3> points = new List<Vector3>();
		List<Vector3> spawnPoints = new List<Vector3>();
		
		spawnPoints.Add(sampleRegionCenter);
		while (spawnPoints.Count > 0) {
			int spawnIndex = Random.Range(0,spawnPoints.Count);
			Vector3 spawnCentre = spawnPoints[spawnIndex];
			bool candidateAccepted = false;

			for (int i = 0; i < numSamplesBeforeRejection; i++)
			{
				float angle = Random.value * Mathf.PI * 2;
				Vector3 dir = new Vector3(Mathf.Sin(angle), 0,  Mathf.Cos(angle));
				Vector3 candidate = spawnCentre + dir * Random.Range(radius, 2*radius);
				if (IsValid(candidate, sampleRegionSize, cellSize, radius, points, grid)) {
	
//					foreach (Vector3 terrainPoint in terrainPoints) {
//						if (candidate.x>=terrainPoint.x && candidate.x<terrainPoint.x+1
//						    && candidate.z>=terrainPoint.z && candidate.z<terrainPoint.z+1) {
//							candidate.y = terrainPoint.y - 0.5f;
//							break;
//						}
//					}
					
//					Vector3 altiPoint = terrainPoints.Find(x => 
//						x.x>=Mathf.Round(candidate.x -0.5f) &&
//						x.z>=Mathf.Round(candidate.z -0.5f));
////					Debug.Log("altiPoint.y : " +altiPoint.y);
//					candidate.y = altiPoint.y;
					
					Vector3 altiPoint = terrainPoints.Find(terrainPoint => 
						candidate.x>=terrainPoint.x && candidate.x<terrainPoint.x+1
						&& candidate.z>=terrainPoint.z && candidate.z<terrainPoint.z+1);
					candidate.y = altiPoint.y-0.5f;
					
					points.Add(candidate);
					spawnPoints.Add(candidate);
					grid[(int)(candidate.x/cellSize),(int)(candidate.z/cellSize)] = points.Count;
					candidateAccepted = true;
					break;
				}
			}
			if (!candidateAccepted) {
				spawnPoints.RemoveAt(spawnIndex);
			}

		}

		return points;
	}

	static bool IsValid(Vector3 candidate, Vector3 sampleRegionSize, float cellSize, float radius, List<Vector3> points, int[,] grid) {
		if (candidate.y < 7.4) {
			//en dessus du niveau de la mer
			return false;
		}
		if (candidate.x >=0 && candidate.x < sampleRegionSize.x && candidate.z >= 0 && candidate.z < sampleRegionSize.z) {
			int cellX = (int)(candidate.x/cellSize);
			int cellZ = (int)(candidate.z/cellSize);
			int searchStartX = Mathf.Max(0,cellX -2);
			int searchEndX = Mathf.Min(cellX+2,grid.GetLength(0)-1);
			int searchStartZ = Mathf.Max(0,cellZ -2);
			int searchEndZ = Mathf.Min(cellZ+2,grid.GetLength(1)-1);

			for (int x = searchStartX; x <= searchEndX; x++) {
				for (int z = searchStartZ; z <= searchEndZ; z++) {
					int pointIndex = grid[x,z]-1;
					if (pointIndex != -1) {
						float sqrDst = (candidate - points[pointIndex]).sqrMagnitude;
						if (sqrDst < radius*radius) {
							return false;
						}
					}
				}
			}
			return true;
		}
		return false;
	}
}
