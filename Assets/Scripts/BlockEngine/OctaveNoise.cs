using UnityEngine;

namespace BlockEngine {
    public class OctaveNoise {

        public int octaves;
        public float persistence;
        public float lacunarity;
        public float scale;

        public OctaveNoise(int octaves, float scale, float persistence, float lacunarity) {
            this.octaves = octaves;
            this.scale = scale;
            this.persistence = persistence;
            this.lacunarity = lacunarity;
        }
        
        public float EvaluateOctaveNoise(float x, float y)
        {
            float totalValue = 0f;
            float amplitude = 1f;
            float frequency = 1f;
            float maxAmplitude = 0f; 

            for (int i = 0; i < octaves; i++)
            {
                float sampleX = (x / scale) * frequency;
                float sampleY = (y / scale) * frequency;

                float noiseValue = Mathf.PerlinNoise(sampleX, sampleY);

                totalValue += noiseValue * amplitude;
                maxAmplitude += amplitude;

                amplitude *= persistence;   
                frequency *= lacunarity;    
            }

            return totalValue / maxAmplitude;
        }
    }
}