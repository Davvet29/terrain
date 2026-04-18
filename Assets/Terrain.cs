using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;


public class Terrain
{
    int verticalSquares;
    [SerializeField] List<Vector3> vertList = new();

    [SerializeField] List<Vector2> UVList = new();
    [SerializeField] Vector2[] newUV;
    [SerializeField] List<int> triangleList = new();


    
    public Mesh Regenerate(int resolution, float size, bool flipTriangles, Texture2D noiseMap, float height, bool randomGen, float noiseSizeDenominator, Vector2 noiseMapSize)
    {
        triangleList.Clear();
        vertList.Clear();
        UVList.Clear(); 

        verticalSquares = (int)Mathf.Sqrt(resolution) + 1;
        Mesh mesh = new Mesh
        {
            };
        CalculateVerts(size);
        CalculateTriangles(flipTriangles);
        ApplyElevation(noiseMap , height, randomGen, noiseSizeDenominator, noiseMapSize);
        mesh.SetVertices(vertList);
        mesh.SetTriangles(triangleList, 0);
        mesh.RecalculateNormals();
        SetColor(mesh);
        ApplyUV(mesh);
        return mesh;
    }

    private void CalculateVerts(float size)
    {
        float pointDistance = size/((float)(verticalSquares-1)/2);
        Debug.Log(pointDistance);
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
        for (int i = 0; i < verticalSquares-1; i++)
        {
            for (int j = 0; j < verticalSquares-1; j++)
            {
                if (!flipTriangles)
                {
                    triangleList.Add(((i*verticalSquares)+j));
                    triangleList.Add(((i*verticalSquares)+j)+1);
                    triangleList.Add(((i*verticalSquares)+j) + verticalSquares+1);

                    triangleList.Add(((i*verticalSquares)+j));
                    triangleList.Add(((i*verticalSquares)+j) + verticalSquares+1);
                    triangleList.Add(((i*verticalSquares)+j) + verticalSquares);
                }
                else
                {
                    triangleList.Add(((i*verticalSquares)+j));
                    triangleList.Add(((i*verticalSquares)+j)+1);
                    triangleList.Add(((i*verticalSquares)+j) + verticalSquares);

                    triangleList.Add(((i*verticalSquares)+j)+1);
                    triangleList.Add(((i*verticalSquares)+j) + verticalSquares+1);
                    triangleList.Add(((i*verticalSquares)+j) + verticalSquares);


                }

            }
        }
        
    }

    public void ApplyUV(Mesh mesh)
    {
        for (int i = 0; i  < vertList.Count(); i++)
        {
            UVList.Add(new Vector2(vertList[i].x, vertList[i].z));
        }
        mesh.uv = UVList.ToArray();
    }

    private void SetColor(Mesh mesh)
    {
        List<UnityEngine.Color> colors = new();

        for (int i = 0; i  < vertList.Count(); i++)
        {
            if(vertList[i].y < 3)
            {
                colors.Add(UnityEngine.Color.green);
            }
            else if(vertList[i].y > 3 && vertList[i].y < 6)
            {
                colors.Add(UnityEngine.Color.yellow);
            }
            else
            {
                colors.Add(UnityEngine.Color.red);
            }
        }
        mesh.SetColors(colors);
    }

    private void ApplyElevation(Texture2D noiseMap, float height, bool randomTerrain, float noiseSizeDenominator, Vector2 noisePosition)
    {
    
        int mapSize = (int)noiseMap.Size().x;
        int vertWidth = (int)Mathf.Sqrt(vertList.Count());
        float pixelColor;
        float pointDistance = mapSize / vertWidth;

        for (int i = 0; i < vertWidth; i++)
        {
            for (int j = 0; j < vertWidth; j++)
            {
                if(randomTerrain)
                {
                    pointDistance = 1 / (float)noiseSizeDenominator;
                    pixelColor = Mathf.PerlinNoise(pointDistance * j, pointDistance * i);
                    UnityEngine.Debug.Log(pixelColor + "pixel colour");
                }
                else
                {
                    pixelColor = noiseMap.GetPixel((int)pointDistance * j, (int)pointDistance * i).grayscale;
                }
                float setHeight = pixelColor * height;
                vertList[CoordinateToVert(i, j)] = new Vector3(vertList[CoordinateToVert(i, j)].x, setHeight, vertList[CoordinateToVert(i, j)].z);

            }
        }
    }

    private int CoordinateToVert(int y, int x)
    {
        int vertWidth = (int)Mathf.Sqrt(vertList.Count());
        return x + (y * vertWidth);
    }
}
