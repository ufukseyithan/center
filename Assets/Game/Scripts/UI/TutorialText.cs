using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace Game.UI {
    public class TutorialText : MonoBehaviour {
        TextMeshProUGUI text;

        Queue<string> queue = new Queue<string>();
        Coroutine coroutine;

        void Awake() {
            text = GetComponent<TextMeshProUGUI>();
        }

        void OnEnable() {
            text.text = string.Empty;

            if (coroutine != null) {
                coroutine = null;

                StartUpdateTutorialTextWithPeekCoroutine();
            }
        }

        public void Enqueue(string text) {
            queue.Enqueue(text);

            StartUpdateTutorialTextWithPeekCoroutine();
        }

        public void Clear() {
            queue.Clear();
        }

        void StartUpdateTutorialTextWithPeekCoroutine() {
            if (coroutine == null)
                if (queue.Count != 0)
                    coroutine = StartCoroutine(UpdateTutorialTextWithPeek());
        }

        IEnumerator UpdateTutorialTextWithPeek() {
            text.text = queue.Peek();
            text.DOFade(1, .33f);

            yield return new WaitForSeconds(4.66f);

            text.DOFade(0, .33f);

            yield return new WaitForSeconds(.33f);

            queue.Dequeue();

            coroutine = null;

            StartUpdateTutorialTextWithPeekCoroutine();
        }
    }
}
