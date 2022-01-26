using UnityEngine;

namespace Game.Data {
    [System.Serializable]
    public class Block {
        [Tooltip("Leaving hitpoint as 0 means the block is unbreakable.")]
        public int hitpoint;
        public Color color;
        public float startAngle;
        public float angle;

        public Block(int hitpoint, Color color, float startAngle, float angle) {
            this.hitpoint = hitpoint;
            this.color = color;
            this.startAngle = startAngle;
            this.angle = angle;
        }
    }
}