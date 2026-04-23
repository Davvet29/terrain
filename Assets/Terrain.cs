using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Color = UnityEngine.Color;


public class Terrain
{
    int verticalSquares;
    [SerializeField] List<Vector3> vertList = new();

    [SerializeField] List<Vector2> UVList = new();
    [SerializeField] Vector2[] newUV;
    [SerializeField] List<int> triangleList = new();




    public Mesh Regenerate(int resolution, float size, bool flipTriangles, Texture2D noiseMap, float height, bool randomGen, float noiseSizeDenominator, Vector2 noiseMapSize, List<UnityEngine.Color> colors, float textureSize, float midHeight, float highHeight)
    {
        triangleList.Clear();
        vertList.Clear();
        UVList.Clear();

        verticalSquares = resolution + 1;
        Mesh mesh = new Mesh
        {
        };
        CalculateVerts(size);
        CalculateTriangles(flipTriangles);
        ApplyElevation(noiseMap, height, randomGen, noiseSizeDenominator, noiseMapSize);
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.SetVertices(vertList);
        mesh.SetTriangles(triangleList, 0);
        mesh.RecalculateNormals();
        SetColor(mesh, colors, midHeight, highHeight);
        ApplyUV(mesh, textureSize);
        return mesh;
    }

    private void CalculateVerts(float size)
    {
        float pointDistance = (2 * size) / (verticalSquares - 1);
        float startPointX = -size;
        float startPointZ = size;
        for (int i = 0; i < verticalSquares; i++)
        {
            for (int j = 0; j < verticalSquares; j++)
            {
                vertList.Add(new Vector3(startPointX + (j * pointDistance), 0, startPointZ - (i * pointDistance)));
            }
        }
    }

    private void CalculateTriangles(bool flipTriangles)
    {
        for (int i = 0; i < verticalSquares - 1; i++)
        {
            for (int j = 0; j < verticalSquares - 1; j++)
            {
                if (!flipTriangles)
                {
                    triangleList.Add(((i * verticalSquares) + j));
                    triangleList.Add(((i * verticalSquares) + j) + 1);
                    triangleList.Add(((i * verticalSquares) + j) + verticalSquares + 1);

                    triangleList.Add(((i * verticalSquares) + j));
                    triangleList.Add(((i * verticalSquares) + j) + verticalSquares + 1);
                    triangleList.Add(((i * verticalSquares) + j) + verticalSquares);
                }
                else
                {
                    triangleList.Add(((i * verticalSquares) + j));
                    triangleList.Add(((i * verticalSquares) + j) + 1);
                    triangleList.Add(((i * verticalSquares) + j) + verticalSquares);

                    triangleList.Add(((i * verticalSquares) + j) + 1);
                    triangleList.Add(((i * verticalSquares) + j) + verticalSquares + 1);
                    triangleList.Add(((i * verticalSquares) + j) + verticalSquares);


                }

            }
        }

    }

    public void ApplyUV(Mesh mesh, float textureSize)
    {
        for (int i = 0; i < vertList.Count; i++)
        {
            UVList.Add(new Vector2(vertList[i].x / textureSize, vertList[i].z / textureSize));
        }
        mesh.uv = UVList.ToArray();
    }

    private void SetColor(Mesh mesh, List<UnityEngine.Color> heightColors, float midHeight, float highHeight)
    {
        List<Color> colors = new();
        for (int i = 0; i < vertList.Count; i++)
        {
            if (vertList[i].y < midHeight)
            {
                colors.Add(heightColors[0]);
            }
            else if (vertList[i].y >= midHeight && vertList[i].y < highHeight)
            {
                colors.Add(heightColors[1]);
            }
            else
            {
                colors.Add(heightColors[2]);
            }
        }
        mesh.SetColors(colors);
    }

    private void ApplyElevation(Texture2D noiseMap, float height, bool randomTerrain, float noiseSizeDenominator, Vector2 noisePosition)
    {
        int vertWidth = (int)Mathf.Sqrt(vertList.Count);
        float pixelColor;

        for (int i = 0; i < vertWidth; i++)
        {
            for (int j = 0; j < vertWidth; j++)
            {
                float u = (float)j / (vertWidth - 1);
                float v = (float)i / (vertWidth - 1);
                if (randomTerrain)
                {
                    pixelColor = Mathf.PerlinNoise(u * noiseSizeDenominator, v * noiseSizeDenominator);                }
                else
                {
                    pixelColor = noiseMap.GetPixelBilinear(u, v).grayscale;
                }
                float setHeight = pixelColor * height;
                vertList[CoordinateToVert(i, j)] = new Vector3(vertList[CoordinateToVert(i, j)].x, setHeight, vertList[CoordinateToVert(i, j)].z);

            }
        }
    }

    private int CoordinateToVert(int y, int x)
    {
        int vertWidth = (int)Mathf.Sqrt(vertList.Count);
        return x + (y * vertWidth);
    }
}
