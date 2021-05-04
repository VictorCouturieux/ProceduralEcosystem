using UnityEngine;
using System.Collections;

public static class TextureGenerator {


	public static void ApplyToMaterial(Material material) {
//		Debug.Log(savedMinHeight + " : " + savedMaxHeight);
		
//		UpdateMeshHeights(material, savedMinHeight, savedMaxHeight, saveBaseColors, saveBaseStartHeight);
	}
	
	public static void UpdateMeshHeights(Material material, float minHeight, float maxHeight, Color[] baseColors, float[] baseStartHeight, float[] baseBlends) {
		
		material.SetFloat("minHeight", minHeight);
		material.SetFloat("maxHeight", maxHeight);
		
		material.SetInt("baseColourCount", baseColors.Length);
		material.SetColorArray("baseColours", baseColors);
		material.SetFloatArray("baseStartHeights", baseStartHeight);
		material.SetFloatArray("baseBlends", baseBlends);
		
//		Debug.Log("minHeight : " + material.GetFloat("minHeight"));
//		Debug.Log("maxHeight : " + material.GetFloat("maxHeight"));
//		
//		Debug.Log("baseColourCount : " + material.GetInt("baseColourCount"));
//		for (int i = 0; i < material.GetInt("baseColourCount"); i++) {
//			Debug.Log("baseColors [" + i + "] : " + material.GetColorArray("baseColours")[i]);
//		}
//		for (int i = 0; i < material.GetInt("baseColourCount"); i++) {
//			Debug.Log("baseStartHeight [" + i + "] : " + material.GetFloatArray("baseStartHeights")[i]);
//		}
		
	}
	
	public static Texture2D TextureFromColourMap(Color[] colourMap, int width, int height) {
		Texture2D texture = new Texture2D (width, height);
		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.SetPixels (colourMap);
		texture.Apply ();
		return texture;
	}


	public static Texture2D TextureFromHeightMap(float[,] heightMap) {
		int width = heightMap.GetLength (0);
		int height = heightMap.GetLength (1);

		Color[] colourMap = new Color[width * height];
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				colourMap [y * width + x] = Color.Lerp (Color.black, Color.white, heightMap [x, y]);
			}
		}

		return TextureFromColourMap (colourMap, width, height);
	}
	
}
