using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Unity.VisualScripting;

namespace Damath
{
    public class TimerManager : MonoBehaviour
    {
        public Ruleset Rules { get; private set; }
        public Timer GlobalTimer;
        public Timer BlueTimer;
        public Timer OrangeTimer;
        [SerializeField] GameObject BlueChip;
        [SerializeField] GameObject OrangeChip;
        [SerializeField] GameObject timerPrefab;

        void Awake()
        {
            
        }

        void OnEnable()
        {
            Game.Events.OnRulesetCreate += ReceiveRuleset;
            Game.Events.OnMatchBegin += Init;
            Game.Events.OnChangeTurn += SwapTurnTimer;
        }

        void OnDisable()
        {
            Game.Events.OnRulesetCreate -= ReceiveRuleset;
            Game.Events.OnMatchBegin -= Init;
            Game.Events.OnChangeTurn -= SwapTurnTimer;
        }

        void Update()
        {

        }

        void ReceiveRuleset(Ruleset rules)
        {
            Rules = rules;
        }
        
        public void Init()
        {
            if (Rules.EnableTimer) 
            {
                if (Rules.EnableGlobalTimer)
                {
                    GlobalTimer.SetFormat(Format.MM_SS);
                    GlobalTimer.SetTime(Rules.GlobalTimerSeconds);
                    GlobalTimer.Begin();
                }
                if (Rules.EnableTurnTimer)
                {
                    BlueTimer.SetFormat(Format.SS);
                    BlueTimer.SetTime(Rules.TurnTimerSeconds);
                    
                    OrangeTimer.SetFormat(Format.SS);
                    OrangeTimer.SetTime(Rules.TurnTimerSeconds);

                    if (Rules.FirstTurn == Side.Bot)
                    {
                        BlueTimer.Begin();
                        OrangeChip.SetActive(false);
                    } else if (Rules.FirstTurn == Side.Top)
                    {
                        OrangeTimer.Begin();
                        BlueChip.SetActive(false);
                    }
                }
            }
        }

        public void Init(MatchController match)
        {
            Init();
        }

        public void SwapTurnTimer(Side turnOf)
        {
            if (turnOf == Side.Bot)
            {
                OrangeTimer.Stop();
                OrangeChip.SetActive(false);

                BlueChip.SetActive(true);
                BlueTimer.Reset(true);
            } else
            {
                BlueTimer.Stop();
                BlueChip.SetActive(false);

                OrangeChip.SetActive(true);
                OrangeTimer.Reset(true);
            }
        }

        // public Timer CreateTimer(float startingTimeInSeconds, string name="New Timer", TextMeshProUGUI textComponent=null)
        // {
        //     var newTimer = Instantiate(timerPrefab);
        //     newTimer.name = name;
        //     newTimer.transform.SetParent(transform);
        //     Timer c_timer = newTimer.GetComponent<Timer>();

        //     if (textComponent != null)
        //     {
        //         c_timer.SetText(textComponent);
        //     }

        //     c_timer.Init(startingTimeInSeconds);
        //     inactiveTimers.Add(c_timer);
        //     return c_timer;
        // }
    }
}