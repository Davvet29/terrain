using System.Drawing;
using UnityEngine;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainScript : MonoBehaviour
{
    private Terrain terrain;
    public int resolution;
    public float size;
    public bool flipTriangles;
    public Texture2D noiseMap;
    public float height;
    
    public Texture2D image;
    public bool randomTerrain;
    public float noiseSizeDenominator;
    public Vector2 noiseMapSize;
    public void Regenerate()
    {
        if (terrain == null) terrain = new Terrain();
        
        Mesh mesh = terrain.Regenerate(resolution, size, flipTriangles, noiseMap, height, randomTerrain, noiseSizeDenominator, noiseMapSize);
        mesh.name = "TerrainMesh";
        GetComponent<MeshFilter>().mesh = mesh;
    }
    
    
    void Start()
    {
        Regenerate();
    }

    
    void Update()
    {
    }
}
