using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;
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

    [SerializeField] UnityEngine.Color lowColor;
    [SerializeField] UnityEngine.Color mediumColor;
    [SerializeField] UnityEngine.Color highColor;

    List<UnityEngine.Color> Colors = new();
    public void Regenerate()
    {
        Colors.Add(lowColor);
        Colors.Add(mediumColor);
        Colors.Add(highColor);

        if (terrain == null) terrain = new Terrain();

        Mesh mesh = terrain.Regenerate(resolution, size, flipTriangles, noiseMap, height, randomTerrain, noiseSizeDenominator, noiseMapSize, Colors);
        mesh.name = "TerrainMesh";
        GetComponent<MeshFilter>().mesh = mesh;
        Colors.Clear();
    }


    void Start()
    {
        Regenerate();
    }


    void Update()
    {
    }
}
