using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using DG.Tweening;
using Game.UI;

namespace Game.Core {
    public class Player : MonoBehaviour {
        [Header("Required References")]
        public HUD hud;

        [SerializeField]
        GameObject ballPrefab = null;
        [SerializeField]
        Transform ballLocation = null;
        
        [Header("Settings")]
        [SerializeField]
        float ballThrowSpeed = 10;
        [Tooltip("The scale of the spare ball.")]
        [SerializeField]
        Vector3 ballScale = new Vector3(.1f, .1f, .1f);
        [Tooltip("The scale of the held ball.")]
        [SerializeField]
        Vector3 finalBallScale = new Vector3(.2f, .2f, .2f);

        [SerializeField]
        int ballCount;

        System.Random random;
        GameObject heldBallObject;
        GameObject spareBallObject;
        Coroutine coroutine;

        public int BallCount {
            get => ballCount;
            set {
                hud.UpdateBallCountText(value);

                ballCount = value;
            }
        }

        void Start() {
            random = new System.Random();
        }

#region Input Handlers
        public void HandleThrowInput(InputAction.CallbackContext context) {
            if (context.performed)
                if (!EventSystem.current.IsPointerOverGameObject())
                    ThrowBall();
        }
#endregion

        public void StartCheckCurrentBallColorsCoroutine() {
            StartCoroutine(CheckCurrentBallColors());
        }

        public void Initiate() {
            CreateSpareBall();
            HoldSpareBall();
        }

        void CreateSpareBall() {
            if (spareBallObject || BallCount <= 1)
                return;

            spareBallObject = Instantiate(ballPrefab, ballLocation);
            spareBallObject.transform.localScale = Vector3.zero;
            spareBallObject.transform.DOScale(new Vector3(ballScale.x, ballScale.y, ballScale.z), .33f);

            Ball spareBall = spareBallObject.GetComponent<Ball>();

            spareBall.Color = GetRandomBallColor();
            spareBall.player = this;
        }

        void HoldSpareBall() {
            if (!spareBallObject || heldBallObject)
                return;

            spareBallObject.transform.DOKill(true);
            spareBallObject.transform.DOMoveX(0, .33f);
            spareBallObject.transform.DOScale(new Vector3(finalBallScale.x, finalBallScale.y, finalBallScale.z), .33f);

            heldBallObject = spareBallObject;

            spareBallObject = null;

            CreateSpareBall();
        }

        void ThrowBall() {
            if (!heldBallObject)
                return;

            heldBallObject.transform.DOKill(true);

            Rigidbody ballRigidbody = heldBallObject.GetComponent<Rigidbody>();

            ballRigidbody.AddForce(Vector2.up * ballThrowSpeed, ForceMode.Impulse);
            heldBallObject.transform.SetParent(transform.parent);

            BallCount--;

            heldBallObject = null;

            HoldSpareBall();

            if (coroutine != null)
                StopCoroutine(coroutine);

            coroutine = StartCoroutine(CheckGameIsOver());
        }
        
        public void AdWatched() {
            BallCount += GameManager.Instance.CurrentLevelData.ballCount / 3;
            
            Initiate();
        }

        IEnumerator CheckGameIsOver() {
            yield return new WaitForSeconds(1);

            if (BallCount <= 0)
                if (GameManager.Instance.State == State.InGame)
                    GameManager.Instance.LoseLevel();
        }

        IEnumerator CheckCurrentBallColors() {
            yield return null;

            var availableColors = GetAvailableColors();

            foreach (var ball in GetComponentsInChildren<Ball>()) {
                var isAppropriate = false;

                foreach (var availableColor in availableColors)
                    if (ball.Color == availableColor)
                        isAppropriate = true;

                if (!isAppropriate)
                    ball.Color = availableColors.Count > 0 ? availableColors.ElementAt(random.Next(availableColors.Count)) : Color.white;
            }
        }

        Color GetRandomBallColor() {
            var availableColors = GetAvailableColors();

            return availableColors.Count > 0 ? availableColors.ElementAt(random.Next(availableColors.Count)) : Color.white;
        }

        HashSet<Color> GetAvailableColors() {
            var layers = transform.parent.GetComponentsInChildren<Layer>();
            layers = layers.Reverse().ToArray();

            var availableColors = new HashSet<Color>();

            float lastLayerRotationSpeed = 0;
            float lastTotalAngle = 0;
            foreach (var layer in layers) {
                var blocks = layer.GetComponentsInChildren<Block>();

                if (blocks.Length == 0)
                    continue;

                var colorsToBeAdded = new HashSet<Color>();
                float totalAngle = 0;
                foreach (var block in blocks) {
                    var color = block.GetComponent<LineRenderer>().startColor;
                
                    totalAngle += block.angle;
                    
                    // Exclude gray (unbreakable) blocks
                    if (color.IsEqualTo(Color.gray))
                        continue;

                    colorsToBeAdded.Add(color);
                }
                
                if (lastTotalAngle == totalAngle && lastLayerRotationSpeed == layer.rotationSpeed)
                    continue;
                
                availableColors.UnionWith(colorsToBeAdded);
                
                if (totalAngle >= 360)
                    break;
                
                lastLayerRotationSpeed = layer.rotationSpeed;
                lastTotalAngle = totalAngle;
            }

            return availableColors;
        }
    }
}
