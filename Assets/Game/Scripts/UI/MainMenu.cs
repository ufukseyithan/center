using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace Game.UI {
    public class MainMenu : MonoBehaviour {
        [Header("Required References")]
        [SerializeField]
        GameObject mainScreen; 
        [SerializeField]
        GameObject nextLevelScreen;
        [SerializeField]
        TextMeshProUGUI levelText;

        [SerializeField]
        TextMeshProUGUI nextLevelScreenLevelText;
        [SerializeField]
        TextMeshProUGUI levelSelectMenuAvailableLevelsText;
        [SerializeField]
        TutorialText tutorialText;

        [SerializeField]
        ScrollRect levelSelectMenuScrollRect;
        [SerializeField]
        HorizontalScrollSnap levelSelectMenuHorizontalScrollSnap;

        [SerializeField]
        GameObject levelSelectMenuItemPrefab;

        public void UpdateLevelText(int level) {
            levelText.text = $"Level {level}";
            nextLevelScreenLevelText.text = $"Tap to play the next level {level}";
        }

        public void UpdateAvailableLevelsText(int availableLevels) {
            levelSelectMenuAvailableLevelsText.text = $"{availableLevels} levels available";

            for (int i = 0; i < levelSelectMenuScrollRect.content.childCount; i++) 
                levelSelectMenuScrollRect.content.GetChild(i).GetComponent<TextMeshProUGUI>().color = availableLevels - 1 < i ? Color.gray : Color.black;

            levelSelectMenuHorizontalScrollSnap.SetContentPositionToItem(availableLevels - 1);
        }

        public void EnqueueTutorialText(string text) {
            tutorialText.Enqueue(text);
        }

        public void ClearTutorialTextQueue() {
            tutorialText.Clear();
        }

        public void ShowMainScreen() {
            HideScreens();

            mainScreen.SetActive(true);
        }

        public void ShowNextLevelScreen() {
            HideScreens();

            nextLevelScreen.SetActive(true);
        }

        public void HideScreens () {
            foreach (Transform child in transform)
                child.gameObject.SetActive(false);
        }

        public void CreateLevelSelectMenuItems(int itemCount) {
            for (int i = 0; i < itemCount; i++) {
                GameObject itemObject = Instantiate(levelSelectMenuItemPrefab, levelSelectMenuScrollRect.content);

                TextMeshProUGUI text = itemObject.GetComponent<TextMeshProUGUI>();
                text.text = (i + 1).ToString();
            } 
        }
    }
}
    
