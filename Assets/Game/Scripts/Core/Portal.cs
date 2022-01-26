using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
    public class Portal : MonoBehaviour {
        [SerializeField]
        float speed = 0;

        void Update() {
            transform.Rotate(new Vector3(0, 0, speed) * Time.deltaTime);
        }
    }
}
