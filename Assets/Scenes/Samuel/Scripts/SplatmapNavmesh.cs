using UnityEngine;

public class SplatmapNavmesh : MonoBehaviour
{
    public Terrain terrain;
    public int textureIndex = 1; // Set this to the correct texture index

    void Start()
    {
        if (terrain == null)
        {
            Debug.LogError("Terrain not assigned!");
            return;
        }

        ReadSplatmap();
    }

    void ReadSplatmap()
    {
        TerrainData terrainData = terrain.terrainData;
        int width = terrainData.alphamapWidth;
        int height = terrainData.alphamapHeight;

        Debug.Log($"Splatmap Resolution: {width}x{height}, Checking Texture Index: {textureIndex}");

        float[,,] splatmapData = terrainData.GetAlphamaps(0, 0, width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float textureStrength = splatmapData[x, y, textureIndex];

                // Debugging output
                if (x % 50 == 0 && y % 50 == 0) // Print some sample points
                {
                    Debug.Log($"Pixel ({x},{y}) - Texture {textureIndex} Strength: {textureStrength}");
                }

                // Check if this texture is dominant at this location
                if (textureStrength > 0.5f)
                {
                    // Debug.Log($"Walkable texture found at ({x},{y}) with strength {textureStrength}");
                }
            }
        }
    }
}



