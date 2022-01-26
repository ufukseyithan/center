using System;
using UnityEngine;
using TMPro;

namespace Game.Core {
    public class Block : MonoBehaviour {
        [Header("Required References")]
        public TextMeshPro text;

        [Header("Settings")]
        public int hitpoint;

        [HideInInspector]
        public float angle;

        Shaker shaker;
        LineRenderer lineRenderer;

        bool unbreakable;
        Quaternion textOriginalRotation;

        void Awake() {
            shaker = GetComponent<Shaker>();
            lineRenderer = GetComponent<LineRenderer>();

            textOriginalRotation = Quaternion.identity;
        }

        void LateUpdate() {
            text.transform.rotation = textOriginalRotation;
        }

        public void Instantiate() {
            text.text = hitpoint.ToString();
            
            if (hitpoint == 0)
                SetUnbreakable();
        }

        /// <returns>true if the block has broken, false otherwise.</returns>
        public bool Damage() {
            if (unbreakable)
                return false;;

            hitpoint--;

            text.text = hitpoint.ToString();

            shaker.Begin();

            if (hitpoint <= 0) {
                Break();

                return true;
            }

            return false;
        }

        void Break() {
            Destroy(gameObject);
        }

        void SetUnbreakable() {
            unbreakable = true;
            lineRenderer.startColor = Color.gray;
            lineRenderer.endColor = Color.gray;
            text.text = String.Empty;
        }
    }
}
