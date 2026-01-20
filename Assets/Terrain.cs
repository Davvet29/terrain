using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;


public class Terrain : MonoBehaviour
{
    [SerializeField] int resolution;
    int width;
    [SerializeField] List<Vector3> vertList;
    [SerializeField] Vector2[] newUV;
    [SerializeField] List<int> triangleList;
    
    public Mesh Regenerate()
    {
        triangleList.Clear();
        vertList.Clear();

        width = (int)Mathf.Sqrt(resolution) + 1;
        Mesh mesh = new Mesh
        {
            };
        CalculateVerts();
        CalculateTriangles();
        mesh.SetVertices(vertList);
        mesh.SetTriangles(triangleList, 0);
        mesh.RecalculateNormals();
        return mesh;
    }

    private void CalculateVerts()
    {
        float pointDistance = 1/((float)(width-1)/2);
        Debug.Log(pointDistance);
        int startPointX = -1;
        int startPointZ = 1;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < width; j++)
            {
                 vertList.Add(new Vector3(startPointX + (j * pointDistance), 0, startPointZ - (i * pointDistance)));
            }
        }
    }

    private void CalculateTriangles()
    {
        for (int i = 0; i < width-1; i++)
        {
            for (int j = 0; j < width-1; j++)
            {
                triangleList.Add(((i*width)+j));
                triangleList.Add(((i*width)+j)+1);
                triangleList.Add(((i*width)+j) + width+1);

                triangleList.Add(((i*width)+j));
                triangleList.Add(((i*width)+j) + width+1);
                triangleList.Add(((i*width)+j) + width);
            }
        }
        
    }
}
