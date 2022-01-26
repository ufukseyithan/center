using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PlaySound : MonoBehaviour {
        [SerializeField] 
        AudioClip clip;

        public void Play() {
            AudioSource.PlayClipAtPoint(clip, Vector3.zero);
        }
    }
}
