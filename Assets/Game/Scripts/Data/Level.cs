using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Data {
    [CreateAssetMenu(fileName = "New Level Data", menuName = "Data/Level")]
    public class Level : ScriptableObject {
        public int ballCount;

        public Layer[] layers;

        [TextArea]
        public string[] tutorialTexts;
        
        [Header("Randomizing Parameters")] 
        [SerializeField]
        int layerCount;
        [SerializeField] 
        int grayBlockCount;
        [SerializeField] 
        Color[] colors;

        [ContextMenu("Randomize")]
        void Randomize() {
            Array.Clear(layers, 0, layers.Length);

            var layersToBeAdded = new List<Layer>();
            var totalHitpoint = 0;
            for (var i = 0; i < layerCount; i++) {
                var blocks = new List<Block>();
                var totalBlocks = Random.Range(1, 10);
                var angleSpace = 360;
                for (var j = 0; j < totalBlocks; j++) {
                    var hitpoint = Random.Range(1, 11);
                    var angleDivider = Random.Range(1, totalBlocks - j);
                    Debug.Log($"totalBlocks: {totalBlocks} | angleDivider: {angleDivider}");
                    var angle = angleSpace / angleDivider;
                    
                    blocks.Add(new Block(hitpoint, colors.RandomElement(), (360 - angleSpace), angle));

                    totalHitpoint += hitpoint;
                    angleSpace -= angle;

                    if (angleSpace <= 0)
                        break;
                }

                layersToBeAdded.Add(new Layer(Random.Range(-500, 500), AnimationCurve.Constant(0, 1, 1), Random.Range(1, 15), blocks.ToArray()));
            }

            ballCount = totalHitpoint + 1;

            layers = layersToBeAdded.ToArray();
        }
    }
}

