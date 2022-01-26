using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI {
    public class InGame : MonoBehaviour {
        [SerializeField] 
        HUD hud;
        [SerializeField] 
        GameObject pauseScreen;
        [SerializeField] 
        GameObject gameOverScreen;
        [SerializeField] 
        GameObject watchAdButton;

        public void ShowGameOverScreen() {
            HideScreens();
            
            gameOverScreen.SetActive(true);

            Time.timeScale = 0;
        }

        public void EnableWatchAdButton() => watchAdButton.SetActive(true);
        
        void HideScreens() {
            hud.gameObject.SetActive(false);
            pauseScreen.SetActive(false);
            gameOverScreen.SetActive(false);
        }
    }
}
