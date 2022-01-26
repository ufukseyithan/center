using System.Linq;
using UnityEngine;
using DG.Tweening;

namespace Game.Core {
    public enum State {
        InMenu,
        InGame
    }

    public class GameManager : MonoBehaviour {
        [Header("Required References")]
        [SerializeField]
        UI.MainMenu mainMenu;
        [SerializeField]
        UI.HUD hud;
        public UI.InGame inGame;
        [SerializeField]
        Level level;

        [Header("Settings")]
        [SerializeField]
        Data.LevelMap levelMapData;
        [SerializeField]
        int selectedLevel = 1;
        [SerializeField]
        int unlockedLevels = 1;
        [SerializeField]
        [TextArea]
        string[] universalTutorialTexts;

        public static GameManager Instance { get; private set; }

        public State State { get; private set; }

        public Data.Level CurrentLevelData => level.data;
        
        public int SelectedLevel {
            get => selectedLevel;
            set {
                Data.Level selectedLevelData = CheckLevelData(value - 1);

                if (selectedLevelData != null) {
                    if (value > UnlockedLevels) {
                        Debug.LogError("Selected level is not yet unlocked!");

                        return;
                    }

                    level.data = selectedLevelData;

                    mainMenu.UpdateLevelText(value);
                    hud.UpdateLevelText(value);

                    selectedLevel = value;
                } else {
                    Debug.LogError("Selected level does not exists!");
                }
            }
        }

        public int UnlockedLevels {
            get => unlockedLevels;
            set {
                if (CheckLevelData(value - 1) != null) {
                    mainMenu.UpdateAvailableLevelsText(value);

                    var saveData = new SaveData {level = value};
                    FileManager.WriteToFile("SaveData.dat", saveData.ToJson());

                    unlockedLevels = value;
                }
            }
        }

        void Awake() {
            if (Instance == null)
                Instance = this;
        }

        void Start() {
            Application.targetFrameRate = Screen.currentResolution.refreshRate;
            
            int totalLevels = levelMapData.levels.Length;

            mainMenu.CreateLevelSelectMenuItems(totalLevels);
            mainMenu.UpdateAvailableLevelsText(totalLevels);
            
            FileManager.LoadFromFile("SaveData.dat", out var json);
            
            var saveData = new SaveData();
            if (json != null)
                saveData.LoadFromJson(json);
            
            UnlockedLevels = saveData.level;
            SelectedLevel = saveData.level;

            mainMenu.ShowMainScreen();
        }

        public void QuitToMainMenu(bool getToMainScreen) {
            State = State.InMenu;

            inGame.gameObject.SetActive(false);

            mainMenu.gameObject.SetActive(true);

            if (getToMainScreen)
                mainMenu.ShowMainScreen();

            level.Cease();

            mainMenu.ClearTutorialTextQueue();
        }

        public void StartLevel() {
            State = State.InGame;

            mainMenu.gameObject.SetActive(false);
            inGame.gameObject.SetActive(true);
            

            level.Initiate();

            for (var i = 0; i < universalTutorialTexts.Length; i++) {
                var playerPrefKey = "universalTutorialText" + i;

                if (!PlayerPrefs.HasKey(playerPrefKey)) {
                    mainMenu.EnqueueTutorialText(universalTutorialTexts[i]);
                    
                    PlayerPrefs.SetInt(playerPrefKey, 1);
                }
            }

            for (var i = 0; i < level.data.tutorialTexts.Length; i++) {
                var playerPrefKey = "level" + SelectedLevel + "TutorialText" + i;

                if (!PlayerPrefs.HasKey(playerPrefKey)) {
                    mainMenu.EnqueueTutorialText(level.data.tutorialTexts[i]);
                    
                    PlayerPrefs.SetInt(playerPrefKey, 1);
                }
            }

            PlayerPrefs.Save();
        }

        public void LoseLevel() {
            inGame.ShowGameOverScreen();
        }
        
        public void BeatLevel() {
            bool gameFinished = false;

            if (UnlockedLevels == SelectedLevel) {
                int temp = UnlockedLevels;

                UnlockedLevels++;
                SelectedLevel = UnlockedLevels;

                gameFinished = temp == UnlockedLevels;
            } else {
                SelectedLevel++;
            }

            QuitToMainMenu(gameFinished);

            if (!gameFinished)
                mainMenu.ShowNextLevelScreen();;
        }

        public void StartGame() => StartLevel();

        public void SelectPreviousLevelData() => SelectedLevel--;

        public void SetNextLevelData() => SelectedLevel++;

        Data.Level CheckLevelData(int index) => levelMapData.levels.ElementAtOrDefault(index);
    }
}

public class SaveData {
    public int level = 1;
    
    public string ToJson() => JsonUtility.ToJson(this);

    public void LoadFromJson(string json) => JsonUtility.FromJsonOverwrite(json, this);
}

