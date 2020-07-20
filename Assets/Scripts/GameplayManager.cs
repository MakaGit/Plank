using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Plank
{
    public class GameplayManager : MonoBehaviour
    {
        private Queue<Chunk> _spawnedChunks = new Queue<Chunk>();

        private Chunk _currentChunk = null;
        private GameObject _bridge = null;
        private bool isBridgeBuildt = false;
        private Player player = null;

        private Coroutine stretchCoroutine = null;
        private void Start()
        {
            _currentChunk = GenerateStartChunk();

            player = Instantiate(SettingsManager.Instance.playerPref, _currentChunk.transform.position, _currentChunk.transform.rotation);
            SettingsManager.Instance.mainCamera._target = player.transform;

            _currentChunk.transform.position = Vector3.zero;

            _currentChunk.nextChunk = GenerateNextChunk(_currentChunk);

        }

        private Chunk GenerateStartChunk()
        {
            var prefab = SettingsManager.Instance.StartChunks[Random.Range(0, SettingsManager.Instance.StartChunks.Count)];

            var spawnedChunk = Instantiate(prefab);

            _spawnedChunks.Enqueue(spawnedChunk);

            return spawnedChunk;
        }

        private Chunk GenerateNextChunk(Chunk initialChunk)
        {
            var prefab = SettingsManager.Instance.Chunks[Random.Range(0, SettingsManager.Instance.Chunks.Count)];

            var spawnedChunk = Instantiate(prefab);

            spawnedChunk.transform.position = initialChunk.transform.position + 
                new Vector3(0f, 0f, Random.Range(SettingsManager.Instance.minChunkDistance, SettingsManager.Instance.maxChunkDistance));

            spawnedChunk.transform.rotation = initialChunk.transform.rotation;

            _spawnedChunks.Enqueue(spawnedChunk);

            if (_spawnedChunks.Count > 5)
            {
                var chunkToDelete = _spawnedChunks.Dequeue();
                Destroy(chunkToDelete.gameObject);
            }

            return spawnedChunk;
        }

        private void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                _bridge = BuildBridge();
            }

            if(Input.GetMouseButtonUp(0) && !isBridgeBuildt)
            {
                StopCoroutine(stretchCoroutine);
                CheckBridgeLength(_bridge);
            }
        }

        private GameObject BuildBridge()
        {
            var bridge = Instantiate(_currentChunk.bridge, _currentChunk.transform.position,
                Quaternion.LookRotation(_currentChunk.nextChunk.transform.position, Vector3.up), _currentChunk.transform);

            stretchCoroutine = StartCoroutine(StretchBridge(bridge));

            return bridge;
        }

        private void CheckBridgeLength(GameObject bridge)
        {
            var bridgeLenght = bridge.transform.localScale.y;
            var distance = _currentChunk.nextChunk.transform.position - _currentChunk.transform.position;

            if (bridgeLenght <= 1)
            {
                Destroy(bridge);
            }
            else if (bridgeLenght >= (distance.z - _currentChunk.nextChunk.chunkLength.z) && bridgeLenght <= (distance.z + _currentChunk.nextChunk.chunkLength.z))
            {
                ScoreManager.Instance.ModifyNumber(1);
                StartCoroutine(TurnBridge(bridge));

                if (bridgeLenght >= (distance.z - _currentChunk.nextChunk.chunkRedLength.z) && bridgeLenght <= (distance.z + _currentChunk.nextChunk.chunkRedLength.z))
                {
                    ScoreManager.Instance.ModifyScore(4);
                    return;
                }
                else if (bridgeLenght >= (distance.z - _currentChunk.nextChunk.chunkOrangeLength.z) && bridgeLenght <= (distance.z + _currentChunk.nextChunk.chunkOrangeLength.z))
                {
                    ScoreManager.Instance.ModifyScore(3);
                    return;
                }
                else if (bridgeLenght >= (distance.z - _currentChunk.nextChunk.chunkGreenLength.z) && bridgeLenght <= (distance.z + _currentChunk.nextChunk.chunkGreenLength.z))
                {
                    ScoreManager.Instance.ModifyScore(2);
                    return;
                }
                else
                {
                    ScoreManager.Instance.ModifyScore(1);
                }
            }
            else
            {
                StartCoroutine(TurnBridge(bridge));
                SceneManager.LoadScene(0);
            }
        }

        private IEnumerator StretchBridge(GameObject bridge)
        {
            isBridgeBuildt = false;

            var distance = (_currentChunk.nextChunk.transform.position - _currentChunk.transform.position + (_currentChunk.chunkLength * 2) ).magnitude;

            for (float i = bridge.transform.localScale.y; i <= distance; i += 0.16f)
            {
                bridge.transform.localScale += new Vector3(0f, 0.16f);
                bridge.transform.position += new Vector3(0f, 0.08f);
                yield return null;
            }

            isBridgeBuildt = true;

            CheckBridgeLength(bridge);
        }

        private IEnumerator TurnBridge(GameObject bridge)
        {
            var targetPosition =_currentChunk.transform.position + new Vector3(0, 0, bridge.transform.position.y);

            player.MoveToTarget(_currentChunk.nextChunk.transform.position);
            _currentChunk = _currentChunk.nextChunk;
            _currentChunk.nextChunk = GenerateNextChunk(_currentChunk);

            for (float x = 0f; x <= 2f; x += 0.02f)
            {
                bridge.transform.position = Vector3.Slerp(bridge.transform.position, targetPosition, x);
                bridge.transform.rotation = Quaternion.Slerp(bridge.transform.rotation, Quaternion.AngleAxis(90f, Vector3.right), x);
                yield return null;
            }

            yield break;
        }
    }
}