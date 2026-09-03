using System.Collections;
using System.Collections.Generic;
using BlockEngine;
using UnityEngine;

public class World : MonoBehaviour {

    public Material blockMat;
    public const int randerDist = 8;
    Dictionary<Vector3Int, Chunk> chunkMap = new Dictionary<Vector3Int, Chunk>();
    List<Chunk> chunkList = new List<Chunk>();
    
    // Start is called before the first frame update
    void Start()
    {
        InitChunks();
        BuildAllChunks();
    }

    public void InitChunks() {
        for (int i = 0; i < randerDist; i++) {
            for (int j = 0; j < randerDist; j++) {
                
                Vector3Int chunkCoord = new Vector3Int(i * Chunk.Width, 0, j * Chunk.Depth);
                
                Chunk chunk = new GameObject(chunkCoord.ToString()).AddComponent<Chunk>();
                chunk.transform.position = chunkCoord;
                chunk.transform.parent = this.transform;
                
                chunk.Init(this,chunkCoord,blockMat);
                chunk.GenerateBlocks();
                
                chunkList.Add(chunk);
                chunkMap.Add(chunkCoord,chunk);
            }
        }
    }

    public void BuildAllChunks() {
        for (int i = 0; i < chunkList.Count; i++) {
            chunkList[i].BuildMesh();
        }
    }

    public Chunk GetChunk(Vector3Int worldBlockPos) {
        int cx = (worldBlockPos.x / Chunk.Width) * Chunk.Width;
        int cz = (worldBlockPos.z / Chunk.Depth) * Chunk.Depth;
        Vector3Int chunkCoord = new Vector3Int(cx, 0, cz);
        Chunk chunk;
        chunkMap.TryGetValue(chunkCoord, out chunk);
        return chunk;
    }

    public BlockState GetBlock(Vector3Int worldBlockPos) {
        Chunk chunk = GetChunk(worldBlockPos);
        if (chunk != null) {
            Vector3Int localBlockPos = worldBlockPos - chunk.coordinate;
            return chunk.GetBlock(localBlockPos);
        }

        return BlockState.None;
    }
}
