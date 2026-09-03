using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.TextCore;
using Vector3 = UnityEngine.Vector3;

namespace BlockEngine {
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer),typeof(MeshCollider))]
    public class Chunk : MonoBehaviour {

        public Vector3Int coordinate;
        public World world;
        
        public const int Width = 16, Height = 128, Depth = 16;
        private BlockState[] blockArray;
        MeshFilter meshFIlter;
        
        public void Init(World _world ,Vector3Int chunkCoord,Material blockMat) {
            this.world = _world;
            coordinate = chunkCoord;
            meshFIlter = GetComponent<MeshFilter>();
            GetComponent<MeshRenderer>().material = blockMat;
            
            blockArray = new BlockState[Width * Height * Depth];
            for (int i = 0; i < blockArray.Length; i++) {
                blockArray[i] = BlockState.None;
            }
        }

        public void BuildMesh() {
            Mesh mesh = new Mesh();
            mesh.Clear();
            
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            
            for (int x = 0; x < Width; x++) {
                for (int z = 0; z < Depth; z++) {
                    for (int y = 0; y < Height; y++) {
                        Vector3Int blockPos = new Vector3Int(x, y, z);
                        
                        if (GetBlock(blockPos) == BlockState.Solid) {
                            
                            // Check block's all side
                            for (int i = 0; i < BlockData.faceDirections.Length; i++) {
                                Vector3Int faceDir = BlockData.faceDirections[i];
                                
                                // adding face
                                if (CoordInChunk(blockPos + faceDir)) {
                                    if (GetBlock(blockPos + faceDir) == BlockState.None) {
                                        triangles.AddRange(BlockData.FaceTrisData(vertices.Count));
                                        vertices.AddRange(BlockData.GetVertices(i,blockPos));
                                    }
                                }
                                else {
                                    Vector3Int worldBlockPos = blockPos + faceDir + coordinate;
                                    if (world.GetBlock(worldBlockPos) == BlockState.None) {
                                        triangles.AddRange(BlockData.FaceTrisData(vertices.Count));
                                        vertices.AddRange(BlockData.GetVertices(i,blockPos));
                                    }
                                }
                                
                            }
                        }
                    }
                }
            }
            
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            meshFIlter.mesh = mesh;
        }

        public BlockState GetBlock(Vector3Int blockPos) {
            if (CoordInChunk(blockPos)) {
                int blockIndex = blockPos.x + blockPos.y * Width + blockPos.z * Width * Height;
                return blockArray[blockIndex];
            }

            return BlockState.None;
        }

        public void SetBlock(Vector3Int blockPos, BlockState state) {
            int index = blockPos.x + blockPos.y * Width + blockPos.z * Width * Height;
            blockArray[index] = state;
        }

        public bool CoordInChunk(Vector3Int blockPos) {
            return blockPos.x >= 0 && blockPos.x < Width && blockPos.y >= 0 && blockPos.y < Height && blockPos.z >= 0 &&
                   blockPos.z < Depth;
        }

        public void GenerateBlocks() {
            for (int x = 0; x < Width; x++) {
                for (int z = 0; z < Depth; z++) {
                    for (int y = 0; y < Height; y++) {
                        Vector3Int blockPos = new Vector3Int(x, y, z);
                        if (y <= 80) {
                            SetBlock(blockPos,BlockState.Solid);
                        }
                    }
                }
            }
        }
    }
}

