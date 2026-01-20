using UnityEngine;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainScript : MonoBehaviour
{
    private Terrain terrain;
    [SerializeField] int resolution;
    
    
    public void Regenerate()
    {
        if (terrain == null) terrain = new Terrain();

        Mesh mesh = terrain.Regenerate(resolution);
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
