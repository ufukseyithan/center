using UnityEngine;

namespace Game.Data {
    [System.Serializable]
    public class Layer {
        public float rotationSpeed;
        public AnimationCurve curve = AnimationCurve.Linear(1, 1, 1, 1);
        public float curveSpeedDivider = 10;
        public Block[] blocks;

        public Layer(float rotationSpeed, AnimationCurve curve, float curveSpeedDivider, Block[] blocks) {
            this.rotationSpeed = rotationSpeed;
            this.curve = curve;
            this.curveSpeedDivider = curveSpeedDivider;
            this.blocks = blocks;
        }
    }
}