using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests
{
    public class HoloMuseTests
    {

        GameObject fretboard;
        bool sceneLoaded;
        bool referencesSetup;


        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene("Main", LoadSceneMode.Single);
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            sceneLoaded = true;
        }

        void SetupReferences()
        {
            if (referencesSetup)
            {
                return;
            }

            Transform[] objects = Resources.FindObjectsOfTypeAll<Transform>();
            foreach (Transform t in objects)
            {
                if (t.name == "Fretboard")
                {
                    fretboard = t.gameObject;
                }
            }

            referencesSetup = true;
        }

        // A Test behaves as an ordinary method
        [Test]
        public void HoloMuseTestsSimplePasses()
        {
            // Use the Assert class to test conditions
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator FretboardInit()
        {

            yield return new WaitWhile(() => sceneLoaded == false);
            SetupReferences();

            Assert.NotNull(fretboard);
        }

        [UnityTest]
        public IEnumerator NoteBubblesInit()
        {

            yield return new WaitWhile(() => sceneLoaded == false);
            SetupReferences();

            GameObject bubbles = fretboard.transform.Find("OpenStringBubbles").gameObject;

            //Returns false if either bubbles gameobject is null, or children have incorrect number of bubbles
            bool bubblesValid = (bubbles != null);

            foreach(Transform child in bubbles.transform)
            {
                Debug.Log("Checking " + child.name);
                if (child.childCount != NoteManager.TOTAL_FRETS)
                    bubblesValid = false;
            }

            Assert.IsTrue(bubblesValid);
        }

        [UnityTest]
        public IEnumerator ModalityInit()
        {

            yield return new WaitWhile(() => sceneLoaded == false);
            SetupReferences();

            NoteManager nm = fretboard.GetComponent<NoteManager>();

            Assert.NotNull(nm.displayModality);
        }
    }
}
