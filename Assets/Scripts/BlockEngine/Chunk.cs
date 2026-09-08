using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlockEngine {
    [RequireComponent(typeof(MeshFilter),typeof(MeshRenderer),typeof(MeshCollider))]
    public class Chunk : MonoBehaviour {

        World world;
        
        public const int Width = 16, Height = 128, Depth = 16;
        BlockState[] blockArray;
        MeshFilter meshFilter;
        MeshCollider meshCollider;

        public void Init(Material blockMat, World _world) {
            blockArray = new BlockState[Width * Height * Depth];
            for (int i = 0; i < blockArray.Length; i++) {
                blockArray[i] = BlockState.None;
            }

            meshFilter = GetComponent<MeshFilter>();
            meshCollider = GetComponent<MeshCollider>();
            GetComponent<MeshRenderer>().material = blockMat;

            world = _world;
        }

        public void BuildMesh() {
            Mesh mesh = new Mesh();
            mesh.Clear();

            meshCollider.sharedMesh = null;

            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();

            for (int x = 0; x < Width; x++) {
                for (int z = 0; z < Depth; z++) {
                    for (int y = 0; y < Height; y++) {
                        Vector3Int blockPos = new Vector3Int(x, y, z);
                        if (GetBlock(blockPos) == BlockState.Solid) {
                            
                            for (int i = 0; i < BlockFace.Directions.Length; i++) {
                                Vector3Int direction = BlockFace.Directions[i];
                                if (CoordInBound(blockPos + direction)) {
                                    if (GetBlock(blockPos + direction) == BlockState.None) {
                                        triangles.AddRange(BlockFace.TrianglesArray(vertices.Count));
                                        vertices.AddRange(BlockFace.GetVerts(i,blockPos));
                                    }
                                }
                                else {
                                    Vector3Int worldBlockPos = blockPos + direction + GetCoord();
                                    if (world.GetBlock(worldBlockPos) == BlockState.None) {
                                        triangles.AddRange(BlockFace.TrianglesArray(vertices.Count));
                                        vertices.AddRange(BlockFace.GetVerts(i,blockPos));
                                    }
                                }
                               
                            }
                        }
                    }
                }
            }

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.RecalculateNormals();
            
            meshFilter.mesh = mesh;
            meshCollider.sharedMesh = mesh;
        }

        public void GenerateBlocks() {
            ClearBlocks();
            for (int x = 0; x < Width; x++) {
                for (int z = 0; z < Depth; z++) {
                    for (int y = 0; y < Height; y++) {
                        if (y < 64) {
                            Vector3Int blockPos = new Vector3Int(x, y, z);
                            SetBlock(blockPos,BlockState.Solid);
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
    }

}
