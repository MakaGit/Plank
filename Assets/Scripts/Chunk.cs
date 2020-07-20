using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plank
{
    public class Chunk : MonoBehaviour
    {
        [SerializeField] public Vector3 chunkLength;
        [SerializeField] public Vector3 chunkGreenLength;
        [SerializeField] public Vector3 chunkOrangeLength;
        [SerializeField] public Vector3 chunkRedLength;

        public Chunk nextChunk = null;
        public GameObject bridge = null;
    }
}