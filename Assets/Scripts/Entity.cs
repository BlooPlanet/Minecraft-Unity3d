using System;
using BlockEngine;
using UnityEngine;

public class Entity : MonoBehaviour {
    
    public Transform cameraT;
    public World world;
    
    public void Update() {
        RaycastHit hit;
        if (Physics.Raycast(cameraT.position, cameraT.forward, out hit)) {
            if (Input.GetMouseButtonDown(0)) {
                BreakBlock(hit);
            }else if (Input.GetMouseButtonDown(1)) {
                PlaceBlock(hit,BlockState.Solid);
            }
        }
    }

    void BreakBlock(RaycastHit rayHit) {
        Vector3 pos = rayHit.point + rayHit.normal * -0.1f;
        Vector3Int worldBlockPos = Vector3Int.FloorToInt(pos);
        Chunk chunk = world.GetChunk(worldBlockPos);
        if (chunk != null) {
            Vector3Int localBlockPos = worldBlockPos - chunk.GetCoord();
            chunk.SetBlock(localBlockPos,BlockState.None);
            chunk.BuildMesh();
            
            for (int i = 0; i < BlockFace.Directions.Length; i++) {
                Vector3Int neighborBlocks = worldBlockPos + BlockFace.Directions[i];
                Chunk neighborChunk = world.GetChunk(neighborBlocks);
                if (chunk != neighborChunk) {
                    neighborChunk.BuildMesh();
                    Debug.Log(neighborChunk);
                }
            }
        }
        //world.SetBlock(blockPos,BlockState.None);

    }

    void PlaceBlock(RaycastHit rayHit, BlockState block) {
        Vector3 pos = rayHit.point + rayHit.normal * 0.1f;
        Vector3Int worldBlockPos = Vector3Int.FloorToInt(pos);
        Chunk chunk = world.GetChunk(worldBlockPos);
        if (chunk != null) {
            Vector3Int localBlockPos = worldBlockPos - chunk.GetCoord();
            chunk.SetBlock(localBlockPos,block);
            chunk.BuildMesh();
            
            for (int i = 0; i < BlockFace.Directions.Length; i++) {
                Vector3Int neighborBlocks = worldBlockPos + BlockFace.Directions[i];
                Chunk neighborChunk = world.GetChunk(neighborBlocks);
                if (chunk != neighborChunk) {
                    neighborChunk.BuildMesh();
                    Debug.Log(neighborChunk);
                }
            }
        }
    }

    public Vector3Int GetChunkCoord() {
        Vector3Int blockPos = Vector3Int.FloorToInt(transform.position);
        int cx = (blockPos.x / Chunk.Width) * Chunk.Width;
        int cz = (blockPos.z / Chunk.Depth) * Chunk.Depth;
        return new Vector3Int(cx, 0, cz);
    }
}