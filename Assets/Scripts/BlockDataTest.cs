using System;
using System.Collections.Generic;
using System.Security;
using BlockEngine;
using UnityEngine;


// all test are passed 
[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class BlockDataTest : MonoBehaviour {
    public MeshFilter meshFilter;
    
    public void Start() {
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        Vector3Int blockPos = new Vector3Int(0, 0, 0);
        
        // top
        triangles.AddRange(BlockData.FaceTrisData(vertices.Count));
        vertices.AddRange(BlockData.TopVertices(blockPos));
        
        // bottom
        triangles.AddRange(BlockData.FaceTrisData(vertices.Count));
        vertices.AddRange(BlockData.BottomVertices(blockPos));
        
        // front
        triangles.AddRange(BlockData.FaceTrisData(vertices.Count));
        vertices.AddRange(BlockData.FrontVertices(blockPos));

        // back
        triangles.AddRange(BlockData.FaceTrisData(vertices.Count));
        vertices.AddRange(BlockData.BackVertices(blockPos));
        
        // right
        triangles.AddRange(BlockData.FaceTrisData(vertices.Count));
        vertices.AddRange(BlockData.RightVertices(blockPos));
        
        // left
        triangles.AddRange(BlockData.FaceTrisData(vertices.Count));
        vertices.AddRange(BlockData.LeftVertices(blockPos));

        
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        meshFilter.mesh = mesh;
    }
}