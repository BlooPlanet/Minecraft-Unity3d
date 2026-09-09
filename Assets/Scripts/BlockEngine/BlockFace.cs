using System.Collections.Generic;
using UnityEngine;

namespace BlockEngine {
    public static class BlockFace {
        public static Vector3[] TopVertsArray(Vector3Int blockPos) {
            return new[] {
                blockPos + new Vector3(0, 1, 0), blockPos + new Vector3(0, 1, 1),
                blockPos + new Vector3(1, 1, 1), blockPos + new Vector3(1, 1, 0),
            };
        }

        public static Vector3[] DownVertsArray(Vector3Int blockPos) {
            return new[] {
                blockPos + new Vector3(0, 0, 0), blockPos + new Vector3(1, 0, 0),
                blockPos + new Vector3(1, 0, 1), blockPos + new Vector3(0, 0, 1),
            };
        }

        public static Vector3[] FrontVertsArray(Vector3Int blockPos) {
            return new[] {
                blockPos + new Vector3(1, 0, 1), blockPos + new Vector3(1, 1, 1),
                blockPos + new Vector3(0, 1, 1), blockPos + new Vector3(0, 0, 1),
            };
        }
        
        public static Vector3[] BackVertsArray(Vector3Int blockPos) {
            return new[] {
                
                blockPos + new Vector3(0, 0, 0), blockPos + new Vector3(0, 1, 0),
                blockPos + new Vector3(1, 1, 0), blockPos + new Vector3(1, 0, 0),
            };
        }
        
        public static Vector3[] RightVertsArray(Vector3Int blockPos) {
            return new[] {
                blockPos + new Vector3(1, 0, 0), blockPos + new Vector3(1, 1, 0),
                blockPos + new Vector3(1, 1, 1), blockPos + new Vector3(1, 0, 1),

            };
        }
        
        public static Vector3[] LeftVertsArray(Vector3Int blockPos) {
            return new[] {
                blockPos + new Vector3(0, 0, 1), blockPos + new Vector3(0, 1, 1),
                blockPos + new Vector3(0, 1, 0), blockPos + new Vector3(0, 0, 0),
            };
        }

        public static int[] TrianglesArray(int v) {
            return new[] {
                0 + v,1 + v,2 + v,
                0 + v,2 + v,3 + v
            };
        }

        public static Vector3Int[] Directions = new[] {
            Vector3Int.up,
            Vector3Int.down,
            Vector3Int.forward,
            Vector3Int.back,
            Vector3Int.right,
            Vector3Int.left
        };

        public static Vector3[] GetVerts(int index, Vector3Int blockPos) {
            List<Vector3[]> vertices = new List<Vector3[]>() {
                TopVertsArray(blockPos),
                DownVertsArray(blockPos),
                FrontVertsArray(blockPos),
                BackVertsArray(blockPos),
                RightVertsArray(blockPos),
                LeftVertsArray(blockPos),
            };
            return vertices[index];
        }

        public static Vector2[] GetUV(int x, int y) {
            int minPixelX = x * 16;
            int maxPixelX = (x + 1) * 16;
            
            int minPixelY = y * 16;
            int maxPixelY = (y + 1) * 16;

            float minX = minPixelX / 256f;
            float maxX = maxPixelX / 256f;
            float maxY = maxPixelY / 256f;
            float minY = minPixelY / 256f;

            return new[] {
                new Vector2(minX,minY),
                new Vector2(maxX,minY),
                new Vector2(maxX,maxY),
                new Vector2(minX,maxY),
                
            };

        }

        public static Vector2Int GetUvCoordFromBlockID(BlockState blockId) {
            if (blockId == BlockState.Stone) {
                return new Vector2Int(1, 15);
            }else if (blockId == BlockState.Grass) {
                return new Vector2Int(0, 15);
            }

            return Vector2Int.zero;
        }
    }
}