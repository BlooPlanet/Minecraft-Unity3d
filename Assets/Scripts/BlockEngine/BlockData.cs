using System.Collections.Generic;
using UnityEngine;

namespace BlockEngine {
    public static class BlockData {
        public static Vector3[] TopVertices(Vector3Int blockPos) {
            return new[] {
                blockPos + new Vector3(0, 1, 0), blockPos + new Vector3(0, 1, 1),
                blockPos + new Vector3(1, 1, 1), blockPos + new Vector3(1, 1, 0),
            };
        }
        
        public static Vector3[] BottomVertices(Vector3Int blockPos) {
            return new[] {
                blockPos + new Vector3(0, 0, 0), blockPos + new Vector3(1, 0, 0),
                blockPos + new Vector3(1, 0, 1), blockPos + new Vector3(0, 0, 1),
            };
        }
        
        
        public static Vector3[] FrontVertices(Vector3Int blockPos) {
            return new[] {
                blockPos + new Vector3(1, 0, 1), blockPos + new Vector3(1, 1, 1),
                blockPos + new Vector3(0, 1, 1), blockPos + new Vector3(0, 0, 1),
            };
        }
        
        
        public static Vector3[] BackVertices(Vector3Int blockPos) {
            return new[] {
                blockPos + new Vector3(0, 0, 0), blockPos + new Vector3(0, 1, 0),
                blockPos + new Vector3(1, 1, 0), blockPos + new Vector3(1, 0, 0),
            };
        }
        
        public static Vector3[] RightVertices(Vector3Int blockPos) {
            return new[] {
                blockPos + new Vector3(1, 0, 0), blockPos + new Vector3(1, 1, 0),
                blockPos + new Vector3(1, 1, 1), blockPos + new Vector3(1, 0, 1),
            };
        }
        
        public static Vector3[] LeftVertices(Vector3Int blockPos) {
            return new[] {
                blockPos + new Vector3(0, 0, 1), blockPos + new Vector3(0, 1, 1),
                blockPos + new Vector3(0, 1, 0), blockPos + new Vector3(0, 0, 0),
            };
        }

        public static int[] FaceTrisData(int v) {
            return new[] {
                0 + v,1 + v,2 + v,
                0 + v,2 + v,3 + v
            };
        }

        public static Vector3Int[] faceDirections = new[] {
            Vector3Int.up, 
            Vector3Int.down,
            Vector3Int.forward, 
            Vector3Int.back,
            Vector3Int.right, 
            Vector3Int.left
        };

        public static Vector3[] GetVertices(int index, Vector3Int blockPos) {
            List<Vector3[]> verticesList = new List<Vector3[]>() {
                TopVertices(blockPos),
                BottomVertices(blockPos),
                FrontVertices(blockPos),
                BackVertices(blockPos),
                RightVertices(blockPos),
                LeftVertices(blockPos),
            };
            return verticesList[index];
        }
        
    }
}