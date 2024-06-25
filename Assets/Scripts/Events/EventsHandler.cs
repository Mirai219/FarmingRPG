using System;
using System.Collections.Generic;
using UnityEngine;

public delegate void MovementDelegate
    (
    float inputX, float inputY,
    bool isWaliking, bool isRunning, bool isIdle, bool isCarrying,
    ToolEffects toolEffects,
    bool isUsingToolRight, bool isUsingToolLeft, bool isUsingToolUp, bool isUsingToolDown,
    bool isLiftingToolRight, bool isLiftingToolLeft, bool isLiftingToolUp, bool isLiftingToolDown,
    bool isPickingRight, bool isPickingLeft, bool isPickingUp, bool isPickingDown,
    bool isSwingingToolRight, bool isSwingingToolLeft, bool isSwingingToolUp, bool isSwingingToolDown,
    bool idleUp,bool idleDown,bool idleLeft,bool idleRight
    );

public static class EventsHandler {


    public static class InventoryEventsHandler
    {
        //Inventory Update event
        private static Action<InventoryLocation, List<InventoryItem>> _InventoryUpdateDelegate;

        public static event Action<InventoryLocation, List<InventoryItem>> InventoryUpdateEvent
        {
            add
            {
                _InventoryUpdateDelegate += value;
            }
            remove
            {
                _InventoryUpdateDelegate -= value;
            }
        }

        public static void CallInventoryUpdateEvent(InventoryLocation inventoryLocation, List<InventoryItem> inventoryList)
        {
            if (_InventoryUpdateDelegate != null)
            {
                _InventoryUpdateDelegate.Invoke(inventoryLocation, inventoryList);
                //Debug.Log("_InventoryUpdateDelegate !!!!");
            }
        }
    }
    
    public static class MovementEventsHandler
    {
        //create a delegate field first;
        private static MovementDelegate _MovementDelegate;

        //then allocate a event
        public static event MovementDelegate MovementEvent
        {
            add
            {
                _MovementDelegate += value;
            }
            remove
            {
                _MovementDelegate -= value;
            }
        }

        //how to invoke a event ,through CallMovementEvent,which is applied when player moves
        public static void CallMovementEvent
            (
                float inputX, float inputY,
                bool isWaliking, bool isRunning, bool isIdle, bool isCarrying,
                ToolEffects toolEffects,
                bool isUsingToolRight, bool isUsingToolLeft, bool isUsingToolUp, bool isUsingToolDown,
                bool isLiftingToolRight, bool isLiftingToolLeft, bool isLiftingToolUp, bool isLiftingToolDown,
                bool isPickingRight, bool isPickingLeft, bool isPickingUp, bool isPickingDown,
                bool isSwingingToolRight, bool isSwingingToolLeft, bool isSwingingToolUp, bool isSwingingToolDown,
                bool idleUp, bool idleDown, bool idleLeft, bool idleRight
            )
        {
            if (_MovementDelegate != null)
            {
                _MovementDelegate.Invoke(inputX, inputY,
                isWaliking, isRunning, isIdle, isCarrying,
                toolEffects,
                isUsingToolRight, isUsingToolLeft, isUsingToolUp, isUsingToolDown,
                isLiftingToolRight, isLiftingToolLeft, isLiftingToolUp, isLiftingToolDown,
                isPickingRight, isPickingLeft, isPickingUp, isPickingDown,
                isSwingingToolRight, isSwingingToolLeft, isSwingingToolUp, isSwingingToolDown,
                idleUp, idleDown, idleLeft, idleRight);
            }
        }
    }
    

    public static class TimeEventsHandler
    {
        #region Minute
        private static Action<int, Season, int, string, int, int, int> _AdvanceGameMinuteDelegate;

        public static event Action<int, Season, int, string, int, int, int> AdvanceGameMinuteEvent
        {
            add
            {
                _AdvanceGameMinuteDelegate += value;
            }
            remove
            {
                _AdvanceGameMinuteDelegate -= value;
            }
        }

        public static void CallAdvanceGameMinuteEvent(int gameYear, Season season, int gameDay,
            string gameDayofWeek, int gameHour, int gameMinute, int gameSecond)
        {
            if (_AdvanceGameMinuteDelegate != null)
            {
                _AdvanceGameMinuteDelegate.Invoke(gameYear, season, gameDay, gameDayofWeek, gameHour, gameMinute, gameSecond);
            }
        }

        #endregion Minute

        #region Hour
        private static Action<int, Season, int, string, int, int, int> _AdvanceGameHourDelegate;

        public static event Action<int, Season, int, string, int, int, int> AdvanceGameHourEvent
        {
            add
            {
                _AdvanceGameHourDelegate += value;
            }
            remove
            {
                _AdvanceGameHourDelegate -= value;
            }
        }

        public static void CallAdvanceGameHourEvent(int gameYear, Season season, int gameDay,
            string gameDayofWeek, int gameHour, int gameMinute, int gameSecond)
        {
            if (_AdvanceGameHourDelegate != null)
            {
                _AdvanceGameHourDelegate.Invoke(gameYear, season, gameDay, gameDayofWeek, gameHour, gameMinute, gameSecond);
            }
        }
        #endregion Hour

        #region Day
        private static Action<int, Season, int, string, int, int, int> _AdvanceGameDayDelegate;

        public static event Action<int, Season, int, string, int, int, int> AdvanceGameDayEvent
        {
            add
            {
                _AdvanceGameDayDelegate += value;
            }
            remove
            {
                _AdvanceGameDayDelegate -= value;
            }
        }

        public static void CallAdvanceGameDayEvent(int gameYear, Season season, int gameDay,
            string gameDayofWeek, int gameHour, int gameMinute, int gameSecond)
        {
            if (_AdvanceGameDayDelegate != null)
            {
                _AdvanceGameDayDelegate.Invoke(gameYear, season, gameDay, gameDayofWeek, gameHour, gameMinute, gameSecond);
            }
        }
        #endregion Day

        #region Season
        private static Action<int, Season, int, string, int, int, int> _AdvanceGameSeasonDelegate;

        public static event Action<int, Season, int, string, int, int, int> AdvanceGameSeasonEvent
        {
            add
            {
                _AdvanceGameSeasonDelegate += value;
            }
            remove
            {
                _AdvanceGameSeasonDelegate -= value;
            }
        }

        public static void CallAdvanceGameSeasonEvent(int gameYear, Season season, int gameDay,
            string gameDayofWeek, int gameHour, int gameMinute, int gameSecond)
        {
            if (_AdvanceGameSeasonDelegate != null)
            {
                _AdvanceGameSeasonDelegate.Invoke(gameYear, season, gameDay, gameDayofWeek, gameHour, gameMinute, gameSecond);
            }
        }
        
        
        #endregion Season
        
        #region Year
        private static Action<int, Season, int, string, int, int, int> _AdvanceGameYearDelegate;

        public static event Action<int, Season, int, string, int, int, int> AdvanceGameYearEvent
        {
            add
            {
                _AdvanceGameYearDelegate += value;
            }
            remove
            {
                _AdvanceGameYearDelegate -= value;
            }
        }

        public static void CallAdvanceGameYearEvent(int gameYear, Season season, int gameDay,
            string gameDayofWeek, int gameHour, int gameMinute, int gameSecond)
        {
            if (_AdvanceGameYearDelegate != null)
            {
                _AdvanceGameYearDelegate.Invoke(gameYear, season, gameDay, gameDayofWeek, gameHour, gameMinute, gameSecond);
            }
        }
        #endregion Year
    }

    public static class SceneEventsHandler
    {
        #region  BeforeSceneUnloadFadeOut
        private static Action BeforeSceneUnloadFadeOutDelegate;

        public static event Action BeforeSceneUnloadFadeOutEvent
        {
            add
            {
                BeforeSceneUnloadFadeOutDelegate += value;
            }
            remove
            {
                BeforeSceneUnloadFadeOutDelegate -= value;
            } 
        }

        public static void CallBeforeSceneUnloadFadeOutEvent()
        {
            if( BeforeSceneUnloadFadeOutDelegate != null)
            {
                BeforeSceneUnloadFadeOutDelegate.Invoke();
            }
        }
        #endregion  BeforeSceneUnloadFadeOut

        #region BeforeSceneUnload
        private static Action BeforeSceneUnloadDelegate;

        public static event Action BeforeSceneUnloadEvent
        {
            add
            {
                BeforeSceneUnloadDelegate += value;
            }
            remove
            {
                BeforeSceneUnloadDelegate -= value;
            }
        }

        public static void CallBeforeSceneUnloadEvent()
        {
            if (BeforeSceneUnloadDelegate != null)
            {
                BeforeSceneUnloadDelegate.Invoke();
            }
        }
        #endregion BeforeSceneUnload

        #region AfterSceneLoad
        private static Action AfterSceneLoadDelegate;

        public static event Action AfterSceneLoadEvent
        {
            add
            {
                AfterSceneLoadDelegate += value;
            }
            remove
            {
                AfterSceneLoadDelegate -= value;
            }
        }

        public static void CallAfterSceneLoadEvent()
        {
            if (AfterSceneLoadDelegate != null)
            {
                AfterSceneLoadDelegate.Invoke();
            }
        }
        #endregion AfterSceneLoad

        #region AfterSceneLoadFadeIn
        private static Action AfterSceneLoadFadeInDelegate;

        public static event Action AfterSceneLoadFadeInEvent
        {
            add
            {
                AfterSceneLoadFadeInDelegate += value;
            }
            remove
            {
                AfterSceneLoadFadeInDelegate -= value;
            }
        }

        public static void CallAfterSceneLoadFadeInEvent()
        {
            if (AfterSceneLoadFadeInDelegate != null)
            {
                AfterSceneLoadFadeInDelegate.Invoke();
            }
        }
        #endregion AfterSceneLoadFadeIn
    }

    

}
