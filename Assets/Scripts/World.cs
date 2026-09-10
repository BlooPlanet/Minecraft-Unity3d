using System;
using System.Collections;
using System.Collections.Generic;
using BlockEngine;
using UnityEngine;

public class World : MonoBehaviour {
    Dictionary<Vector3Int, Chunk> chunkMap = new Dictionary<Vector3Int, Chunk>();
    public List<Chunk> activeChunks = new List<Chunk>();
    Queue<Chunk> chunkPool = new Queue<Chunk>();

    public Material blockMat;
    public Material waterMat;
    public Color color;
    
    public Transform playerT;
    public Entity player;
    int randerDist = 8;
    public int seaLevel = 55;

    public AnimationCurve curve;

    public void Start() {
        float half = randerDist * 0.5f * Chunk.Width;
        playerT.position = new Vector3(500, 130, 500);
        
        // Init(randerDist,randerDist);
        // BuildMesh();
        
        StartCoroutine(LoadChunk(player.GetChunkCoord()));
        
    }

    Vector3Int previousChunk;
    float refreshTime;
    public void Update() {
        refreshTime += Time.deltaTime;
        if (refreshTime > 0.5f) {
            Vector3Int currentChunk = player.GetChunkCoord();
            if (currentChunk != previousChunk) {
                UnloadChunks();
                StartCoroutine(LoadChunk());
                previousChunk = player.GetChunkCoord();
            }
    
            refreshTime -= 0.5f;
        }
    }

    public void Init(int width, int depth) {
        for (int x = 0; x < width; x++) {
            for (int z = 0; z < depth; z++) {
                Vector3Int coord = new Vector3Int(x * Chunk.Width, 0, z * Chunk.Depth);
                Chunk chunk = new GameObject(coord.ToString()).AddComponent<Chunk>();
                chunk.transform.position = coord;
                chunk.transform.parent = this.transform;
                
                chunk.Init(blockMat,waterMat,this);
                chunk.GenerateBlocks();
                
                activeChunks.Add(chunk);
                chunkMap.TryAdd(coord,chunk);
            }
        }
    }

    public void BuildMesh() {
        for (int i = 0; i < activeChunks.Count; i++) {
            activeChunks[i].BuildMesh();
        }
    }

    // this only return from chunk map dictionary
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
            Vector3Int localBlockPos = worldBlockPos - chunk.GetCoord();
            return chunk.GetBlock(localBlockPos);
        }

        return BlockState.None;
    }

    public void SetBlock(Vector3Int worldBlockPos, BlockState block) {
        Chunk chunk = GetChunk(worldBlockPos);
        if (chunk != null) {
            Vector3Int localBlockPos = worldBlockPos - chunk.GetCoord();
            chunk.SetBlock(localBlockPos,block);
        }
    }
    
    IEnumerator LoadChunk() {
        Vector3Int playerChunkCoord = player.GetChunkCoord();
        for (int x = -randerDist - 1; x <= randerDist + 1; x++) {
            for (int z = -randerDist - 1; z <= randerDist + 1; z++) {
                Vector3Int chunkCoord = new Vector3Int(x * Chunk.Width, 0, z * Chunk.Depth) + playerChunkCoord;
                if (!chunkMap.ContainsKey(chunkCoord)) {
                    Chunk chunk = SpwanChunk(chunkCoord);
                    chunk.gameObject.SetActive(false);
                    chunk.GenerateBlocks();
                    chunkMap.TryAdd(chunkCoord, chunk);
                    activeChunks.Add(chunk);
                    yield return null;
                }
            }
        }
        
        for (int x = -randerDist; x <= randerDist; x++) {
            for (int z = -randerDist; z <= randerDist; z++) {
                Vector3Int chunkCoord = new Vector3Int(x * Chunk.Width, 0, z * Chunk.Depth) + playerChunkCoord;
                
                Chunk chunk = chunkMap[chunkCoord];
                if (chunk.gameObject.activeSelf == false) {
                    chunk.gameObject.SetActive(true);
                    chunk.BuildMesh();
                    Debug.Log(chunk.name);
                    yield return null;
                }
                // if (chunkDictionary.ContainsKey(chunkCoord)) {
                //     Chunk chunk;
                //     chunkDictionary.TryGetValue(chunkCoord, out chunk);
                //     chunk.gameObject.SetActive(true);
                // }
            }
        }
    }
    
    IEnumerator LoadChunk(Vector3Int playerChunkCoord) {
        for (int x = -randerDist - 1; x <= randerDist + 1; x++) {
            for (int z = -randerDist - 1; z <= randerDist + 1; z++) {
                Vector3Int chunkCoord = new Vector3Int(x * Chunk.Width, 0, z * Chunk.Depth) + playerChunkCoord;
                if (!chunkMap.ContainsKey(chunkCoord)) {
                    Chunk chunk = SpwanChunk(chunkCoord);
                    chunk.gameObject.SetActive(false);
                    chunk.GenerateBlocks();
                    chunkMap.TryAdd(chunkCoord, chunk);
                    activeChunks.Add(chunk);
                    yield return null;
                }
            }
        }
        
        for (int x = -randerDist; x <= randerDist; x++) {
            for (int z = -randerDist; z <= randerDist; z++) {
                Vector3Int chunkCoord = new Vector3Int(x * Chunk.Width, 0, z * Chunk.Depth) + playerChunkCoord;
                
                Chunk chunk = chunkMap[chunkCoord];
                if (chunk.gameObject.activeSelf == false) {
                    chunk.gameObject.SetActive(true);
                    chunk.BuildMesh();
                    Debug.Log(chunk.name);
                    yield return null;
                }
                // if (chunkDictionary.ContainsKey(chunkCoord)) {
                //     Chunk chunk;
                //     chunkDictionary.TryGetValue(chunkCoord, out chunk);
                //     chunk.gameObject.SetActive(true);
                // }
            }
        }
    }

    public void UnloadChunks() {
        for (int i = 0; i < activeChunks.Count; i++) {
            Chunk chunk = activeChunks[i];
            Vector3Int chunkCoord = activeChunks[i].GetCoord();
            if (PlayerChunkBound(chunkCoord) == false) {
                chunk.gameObject.SetActive(false);
                chunkPool.Enqueue(chunk);
                activeChunks.Remove(chunk);
                chunkMap.Remove(chunkCoord);
            }
        }
    }

    bool PlayerChunkBound(Vector3Int chunkCoord) {
        Vector3Int currentChunkCoord = player.GetChunkCoord();
        int minX = (-randerDist - 1) * Chunk.Width + currentChunkCoord.x;
        int maxX = (randerDist + 1) * Chunk.Width + currentChunkCoord.x;
        int minZ = (-randerDist - 1) * Chunk.Depth + currentChunkCoord.z;
        int maxZ = (randerDist + 1) * Chunk.Depth + currentChunkCoord.z;

        return chunkCoord.x >= minX && chunkCoord.x < maxX && chunkCoord.z >= minZ &&
               chunkCoord.z < maxZ;
    }

    public Chunk SpwanChunk(Vector3Int coord) {
        if (chunkPool.Count > 0) {
            Chunk chunk = chunkPool.Dequeue();
            chunk.gameObject.SetActive(true);
            chunk.transform.position = coord;
            return chunk;
        }
        else {
            Chunk chunk = new GameObject(coord.ToString()).AddComponent<Chunk>();
            chunk.transform.position = coord;
            chunk.transform.parent = this.transform;
            chunk.Init(blockMat,waterMat,this);
            return chunk;
        }
    }
}
