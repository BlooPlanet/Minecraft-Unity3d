using UnityEngine;

namespace BlockEngine {
    public static class BlockID {
        public static Color GetColor(BlockState blockId) {
            if (blockId == BlockState.Solid) {
                return new Color(1,0.5424528f,0.5424528f,1f);
            }else if (blockId == BlockState.Water) {
                return new Color(1,1,1,1);
            }

            return new Color(0,0,0,255);
        }
    }
}