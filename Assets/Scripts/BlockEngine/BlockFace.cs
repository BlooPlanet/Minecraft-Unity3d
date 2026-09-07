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
    }
}