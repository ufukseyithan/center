using UnityEngine;
using DG.Tweening;

namespace Game.Core {
    public class Ball : MonoBehaviour {
        public Player player;

        SpriteRenderer spriteRenderer;
        TrailRenderer trailRenderer;

        Color color;

        public Color Color {
            get => spriteRenderer.color;
            set {
                spriteRenderer.color = value;
                trailRenderer.material.color = value;
            }
        }

        void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            trailRenderer = GetComponent<TrailRenderer>();
        }

        void OnTriggerEnter2D() {
            Destroy(gameObject);
            GameManager.Instance.BeatLevel();
        }

        void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.TryGetComponent<Block>(out Block block)) {
                if (spriteRenderer.color == Color.white || spriteRenderer.color == block.GetComponent<LineRenderer>().startColor)
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
