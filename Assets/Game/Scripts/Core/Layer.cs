using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
    public class Layer : MonoBehaviour {
        public float rotationSpeed = 0;
        public AnimationCurve curve;
        public float curveSpeedDivider;
        
        float time = 0;
        
        
        
        void Update() {
            if (Time.timeScale == 0)
                return;
            
            transform.Rotate(new Vector3(0, 0, (curve.Evaluate(time) * rotationSpeed)) * Time.deltaTime);

            time += Time.deltaTime / curveSpeedDivider;
            if (time > 1)
                time = 0;
        }
    }
}
