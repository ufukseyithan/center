using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.UI {
    public class BeginButton : MonoBehaviour {
        public void StartSelectedLevel() {
            Core.GameManager.Instance.StartLevel();
        }
    }
}

