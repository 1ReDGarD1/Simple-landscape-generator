using UnityEngine;

namespace MyLandscapeGenerator.Scripts.Generator
{
    public abstract class BaseGenerator : ScriptableObject
    {
        public abstract float[,] GenerationMap();
    }
}