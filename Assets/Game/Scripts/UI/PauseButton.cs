using UnityEngine;

namespace Game.UI {
    public class PauseButton : MonoBehaviour {
        public void Pause() => Time.timeScale = 0;

        public void Unpause() => Time.timeScale = 1;
    }
}