using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Damath
{
    public class ScoreboardUI : MonoBehaviour
    {
        public GameObject scoreBlue;
        public GameObject scoreOrange;
        public GameObject turnTimerBlue;
        public GameObject turnTimerOrange;

        void Awake()
        {
            scoreBlue = transform.Find("Score Blue").gameObject;
            scoreOrange = transform.Find("Score Orange").gameObject;

            turnTimerBlue = transform.Find("Turn Timer Blue").gameObject;
            turnTimerOrange = transform.Find("Turn Timer Orange").gameObject;
        }
    }
}
