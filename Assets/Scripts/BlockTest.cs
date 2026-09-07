using System;
using System.Collections.Generic;
using BlockEngine;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class BlockTest : MonoBehaviour {
    public void Start() {
        Mesh mesh = new Mesh();
        mesh.Clear();
        
        Vector3Int blockPos = Vector3Int.zero;

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        triangles.AddRange(BlockFace.TrianglesArray(vertices.Count));
        vertices.AddRange(BlockFace.TopVertsArray(blockPos));
        
        triangles.AddRange(BlockFace.TrianglesArray(vertices.Count));
        vertices.AddRange(BlockFace.DownVertsArray(blockPos));
        
        triangles.AddRange(BlockFace.TrianglesArray(vertices.Count));
        vertices.AddRange(BlockFace.FrontVertsArray(blockPos));
        
        triangles.AddRange(BlockFace.TrianglesArray(vertices.Count));
        vertices.AddRange(BlockFace.BackVertsArray(blockPos));
        
        triangles.AddRange(BlockFace.TrianglesArray(vertices.Count));
        vertices.AddRange(BlockFace.RightVertsArray(blockPos));
        
        triangles.AddRange(BlockFace.TrianglesArray(vertices.Count));
        vertices.AddRange(BlockFace.LeftVertsArray(blockPos));

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        GetComponent<MeshFilter>().mesh = mesh;
    }
}