using UnityEngine;
using TMPro;

namespace Game.UI {
    public class HUD : MonoBehaviour {
        [SerializeField]
        TextMeshProUGUI levelText;
        [SerializeField]
        TextMeshProUGUI ballCountText;

        public void UpdateLevelText(int level) {
            levelText.text = $"Level {level}";
        }

        public void UpdateBallCountText(int ballCount) {
            ballCountText.text = ballCount.ToString();
        }
    }
}
