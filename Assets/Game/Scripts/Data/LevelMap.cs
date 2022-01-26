using System;
using UnityEngine;

namespace Game.Data {
    [CreateAssetMenu(fileName = "New Levels Data", menuName = "Data/Levels")]
    public class LevelMap : ScriptableObject {
        public Level[] levels;
    }
}

