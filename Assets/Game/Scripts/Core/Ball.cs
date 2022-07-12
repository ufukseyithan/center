using UnityEngine;
using DG.Tweening;

namespace Game.Core {
    public class Ball : MonoBehaviour {
        public Player player;

        MeshRenderer meshRenderer;
        TrailRenderer trailRenderer;

        Color color;

        public Color Color {
            get => meshRenderer.material.color;
            set {
                meshRenderer.material.color = value;
                trailRenderer.material.color = value;
            }
        }

        void Awake() {
            meshRenderer = GetComponent<MeshRenderer>();
            trailRenderer = GetComponent<TrailRenderer>();
        }

        
        void OnTriggerEnter() {
            Destroy(gameObject);
            GameManager.Instance.BeatLevel();
        }

        void OnCollisionEnter(Collision collision) {
            if (collision.gameObject.TryGetComponent<Block>(out Block block)) {
                if ( meshRenderer.material.color == Color.white ||  meshRenderer.material.color == block.GetComponent<LineRenderer>().startColor)
                    if (block.Damage())
                        player.StartCheckCurrentBallColorsCoroutine();

                Destroy(gameObject);
            }
        }

        void OnDestroy() {
            DOTween.Kill(transform);
        }
    }
}
