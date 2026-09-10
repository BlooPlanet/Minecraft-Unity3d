using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BlockEngine {
    [RequireComponent(typeof(MeshFilter),typeof(MeshRenderer),typeof(MeshCollider))]
    public class Chunk : MonoBehaviour {

        World world;
        
        public const int Width = 16, Height = 128, Depth = 16;
        BlockState[] blockArray;
        MeshFilter meshFilter;
        MeshCollider meshCollider;

        public void BuildMesh() {
            Mesh mesh = new Mesh();
            mesh.Clear();
            mesh.subMeshCount = 2;

            meshCollider.sharedMesh = null;

            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            List<int> waterTriangles = new List<int>();
            List<Color> vertexColors = new List<Color>();
            List<Vector3> normals = new List<Vector3>();
            
            for (int x = 0; x < Width; x++) {
                for (int z = 0; z < Depth; z++) {
                    for (int y = 0; y < Height; y++) {
                        Vector3Int blockPos = new Vector3Int(x, y, z);
                        if (GetBlock(blockPos) != BlockState.None && GetBlock(blockPos) != BlockState.Water) {
                            Color blockCol = BlockID.GetColor(GetBlock(blockPos));
                            
                            for (int i = 0; i < BlockFace.Directions.Length; i++) {
                                Vector3Int direction = BlockFace.Directions[i];
                                if (CoordInBound(blockPos + direction)) {
                                    if (GetBlock(blockPos + direction) == BlockState.None || GetBlock(blockPos + direction) == BlockState.Water) {
                                        triangles.AddRange(BlockFace.TrianglesArray(vertices.Count));
                                        vertices.AddRange(BlockFace.GetVerts(i,blockPos));
                                        
                                        for (int j = 0; j < 4; j++) {
                                            vertexColors.Add(blockCol);
                                        }
                                        
                                        for (int j = 0; j < 4; j++) {
                                            normals.Add(direction);
                                        }
                                       

                                    }
                                }
                                else {
                                    Vector3Int worldBlockPos = blockPos + direction + GetCoord();
                                    if (world.GetBlock(worldBlockPos) == BlockState.None || world.GetBlock(worldBlockPos) == BlockState.Water) {
                                        triangles.AddRange(BlockFace.TrianglesArray(vertices.Count));
                                        vertices.AddRange(BlockFace.GetVerts(i,blockPos));
     
                                        for (int j = 0; j < 4; j++) {
                                            vertexColors.Add(blockCol);
                                        }
                                        
                                        for (int j = 0; j < 4; j++) {
                                            normals.Add(direction);
                                        }
                                    }
                                }
                               
                            }
                        }
                        
                        if (GetBlock(blockPos) == BlockState.Water) {
                            Color blockCol = BlockID.GetColor(GetBlock(blockPos));
                            
                            for (int i = 0; i < BlockFace.Directions.Length; i++) {
                                Vector3Int direction = BlockFace.Directions[i];
                                if (CoordInBound(blockPos + direction)) {
                                    if (GetBlock(blockPos + direction) != BlockState.Water && GetBlock(blockPos + direction) == BlockState.None) {
                                        waterTriangles.AddRange(BlockFace.TrianglesArray(vertices.Count));
                                        vertices.AddRange(BlockFace.GetVerts(i,blockPos));
                                        
                                        for (int j = 0; j < 4; j++) {
                                            vertexColors.Add(blockCol);
                                        }
                                        
                                        for (int j = 0; j < 4; j++) {
                                            normals.Add(direction);
                                        }
                                    }
                                }
                                else {
                                    Vector3Int worldBlockPos = blockPos + direction + GetCoord();
                                    if (world.GetBlock(worldBlockPos) != BlockState.Water && world.GetBlock(worldBlockPos) == BlockState.None) {
                                        waterTriangles.AddRange(BlockFace.TrianglesArray(vertices.Count));
                                        vertices.AddRange(BlockFace.GetVerts(i,blockPos));
                                        
                                        for (int j = 0; j < 4; j++) {
                                            vertexColors.Add(blockCol);
                                        }
                                        
                                        for (int j = 0; j < 4; j++) {
                                            normals.Add(direction);
                                        }
                                    }
                                }
                               
                            }
                        }
                    }
                }
            }

            mesh.vertices = vertices.ToArray();
            mesh.SetTriangles(triangles.ToArray(),0);
            mesh.SetTriangles(waterTriangles.ToArray(),1);
            mesh.SetNormals(normals);
            //mesh.colors = vertexColors.ToArray();
            
            
            meshFilter.mesh = mesh;
            meshCollider.sharedMesh = mesh;
        }

        public void Init(Material blockMat,Material waterMat, World _world) {
            blockArray = new BlockState[Width * Height * Depth];
            for (int i = 0; i < blockArray.Length; i++) {
                blockArray[i] = BlockState.None;
            }

            meshFilter = GetComponent<MeshFilter>();
            meshCollider = GetComponent<MeshCollider>();
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.materials = new[] {
                blockMat,
                waterMat
            };

            world = _world;
        }

        public void GenerateBlocks() {
            ClearBlocks();
            OctaveNoise noise = new OctaveNoise(4, 70, 0.5f, 2f);
            int surfaceLevel = 60;
            for (int x = 0; x < Width; x++) {
                for (int z = 0; z < Depth; z++) {
                    //float value = Mathf.PerlinNoise((x + GetCoord().x) / 50f, (z + GetCoord().z )/ 50f) * 2f - 1;
                    float value = world.curve.Evaluate(noise.EvaluateOctaveNoise(x + GetCoord().x, z + GetCoord().z) * 2f - 1);
                    int amplitude = surfaceLevel + (int)(value * 32);
                    for (int y = 0; y < amplitude; y++) {
                        Vector3Int blockPos = new Vector3Int(x, y, z);
                        SetBlock(blockPos,BlockState.Solid);
                    }
                }
            }
            GenerateWater();
        }

        public void GenerateWater() {
            for (int x = 0; x < Width; x++) {
                for (int z = 0; z < Depth; z++) {
                    bool hitSurface = true;
                    for (int y = world.seaLevel; y >= 0; y--) {
                        Vector3Int blockPos = new Vector3Int(x, y, z);
                        if (hitSurface) {
                            if (GetBlock(blockPos) == BlockState.None) {
                                SetBlock(blockPos,BlockState.Water);
                            }
                            else {
                                hitSurface = false;
                            }
                        }
                    }
                }
            }
        }

        public void ClearBlocks() {
            for (int b = 0; b < blockArray.Length; b++) {
                blockArray[b] = BlockState.None;
            }
        }

        public BlockState GetBlock(Vector3Int blockPos) {
            if (CoordInBound(blockPos)) {
                int index = blockPos.x + blockPos.y * Width + blockPos.z * Width * Height;
                return blockArray[index];
            }

            return BlockState.None;
        }

        public void SetBlock(Vector3Int blockPos, BlockState block) {
            int index = blockPos.x + blockPos.y * Width + blockPos.z * Width * Height;
            blockArray[index] = block;
        }

        bool CoordInBound(Vector3Int blockPos) {
            return blockPos.x >= 0 && blockPos.x < Width && blockPos.y >= 0 && blockPos.y < Height && blockPos.z >= 0 &&
                   blockPos.z < Depth;
        }

        public Vector3Int GetCoord() {
            return Vector3Int.FloorToInt(transform.position);
        }

        public Vector2Int RandomCoordTex() {
            int x = Random.Range(0, 16);
            int y = Random.Range(0, 16);
            return new Vector2Int(x, y);
        }
    }

}
