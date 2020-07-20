using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plank
{
    public class SettingsManager : MonoBehaviour
    {
        public static SettingsManager Instance = null;

        [SerializeField] public List<Chunk> StartChunks = null;
        [SerializeField] public List<Chunk> Chunks = null;

        [SerializeField] public float minChunkDistance = 0.0f;
        [SerializeField] public float maxChunkDistance = 0.0f;

        [SerializeField] public Player playerPref = null;
        [SerializeField] public float playerSpeed = 0.0f;

        [SerializeField] public CameraFollow mainCamera = null;


        private void Start()
        {
            Instance = this;
        }
    }
}