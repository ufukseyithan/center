using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Game.Core {
    public class Level : MonoBehaviour {
        [Header("Required References")]
        public Player player;
        public Transform portal;
        public Layer[] layers;

        [Header("Settings")]
        public Data.Level data;
        public GameObject blockPrefab;
        [SerializeField]
        float distanceToPortal = 1;
        [SerializeField]
        float spacingDistance = 1;
        [SerializeField]
        int blockRendererResolution = 10;

        public void Initiate() {
            GameManager.Instance.inGame.EnableWatchAdButton();
            
            portal.DOScale(new Vector3(.5f, .5f, .5f), .5f).OnComplete(() => {
                player.BallCount = data.ballCount;

                for (int i = 0; i < data.layers.Length; i++) {
                    layers[i].gameObject.SetActive(true);
                    layers[i].transform.rotation = Quaternion.identity;
                    layers[i].rotationSpeed = data.layers[i].rotationSpeed;
                    layers[i].curve = data.layers[i].curve;
                    layers[i].curveSpeedDivider  = data.layers[i].curveSpeedDivider;

                    foreach (Data.Block blockData in data.layers[i].blocks) {
                        GameObject blockObject = Instantiate(blockPrefab, layers[i].transform);

                        Block block = blockObject.GetComponent<Block>();
                        block.hitpoint = blockData.hitpoint;
                        block.angle = blockData.angle;

                        LineRenderer lineRenderer = blockObject.GetComponent<LineRenderer>();
                        lineRenderer.startColor = blockData.color;
                        lineRenderer.endColor = blockData.color;
                        lineRenderer.positionCount = blockRendererResolution + 1;

                        block.Instantiate();

                        Vector3[] array = new Vector3[blockRendererResolution + 1];

                        float startAngle = blockData.startAngle;
                        for (int j = 0; j <= blockRendererResolution; j++) {
                            float totalDistance = distanceToPortal + (spacingDistance * i);

                            float x = Mathf.Sin(Mathf.Deg2Rad * startAngle) * totalDistance;
                            float y = Mathf.Cos(Mathf.Deg2Rad * startAngle) * totalDistance;

                            array[j] = new Vector2(x, y);

                            startAngle += (blockData.angle / blockRendererResolution);

                            if ((j - 1) == Mathf.Round(blockRendererResolution / 2)) {
                                block.text.transform.position = new Vector2(x, y) / 2;
                            }
                        }

                        lineRenderer.SetPositions(array);

                        MeshCollider meshCollider = blockObject.GetComponent<MeshCollider>();

                        //edgeCollider.points = Array.ConvertAll(array, element => (Vector2) element);

                        MeshFilter meshFilter = blockObject.GetComponent<MeshFilter>();

                        Mesh mesh = new Mesh();
                        meshFilter.mesh = mesh;
                        meshCollider.sharedMesh = mesh;
                        lineRenderer.BakeMesh(mesh);

                        lineRenderer.enabled = false;
                    }
                }

                player.Initiate();
            });
        }
        
        public void Restart() {
            Cease();

            Initiate();
        }

        public void Cease() {
            DestroyBlocks();
            DisableLayers();
            DestroyBalls();

            portal.DOScale(new Vector3(15f, 15f, 15f), .5f);
        }

        void DisableLayers() {
            foreach (Transform child in portal)
                child.gameObject.SetActive(false);
        }

        void DestroyBlocks() {
            foreach (Block block in GetComponentsInChildren<Block>())
                Destroy(block.gameObject);
        }

        void DestroyBalls() {
            foreach (Ball ball in GetComponentsInChildren<Ball>())
                Destroy(ball.gameObject);
        }
    }
}
