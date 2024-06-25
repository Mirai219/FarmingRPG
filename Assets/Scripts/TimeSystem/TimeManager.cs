using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager: SingletonMonobehaviour<TimeManager>
{
    private int gameYear = 1;
    private Season gameSeason = Season.Spring;
    private int gameDay = 1;
    private int gameHour = 6;
    private int gameMinute = 30;
    private int gameSecond = 0;
    private string gameDayofWeek = "Mon";

    private bool gameClockPaused = false;

    private float gameTick = 0f;

    private void Start()
    {
        EventsHandler.TimeEventsHandler.CallAdvanceGameMinuteEvent(gameYear,gameSeason,gameDay
            ,gameDayofWeek, gameHour, gameMinute, gameSecond);
    }

    private void Update()
    {
        if (!gameClockPaused)
        {
            GameTick();
        }
    }

    private void GameTick()
    {
        gameTick += Time.deltaTime;

        if (gameTick >= Settings.secondsPerGameSeconds)
        {
            gameTick -= Settings.secondsPerGameSeconds;

            UpdateGameSeconds();
        }
    }

    private void UpdateGameSeconds()
    {
        gameSecond++;

        if(gameSecond > 59)
        {
            gameSecond -= 60;
            gameMinute++;

            if(gameMinute > 59)
            {
                gameMinute -= 60;
                gameHour++;

                if( gameHour > 23)
                {
                    gameHour -= 24;
                    gameDay++;

                    if(gameDay > 30)
                    {
                        gameDay = 1;


                        int _gameSeason = (int)gameSeason;
                        _gameSeason++;

                        gameSeason = (Season) _gameSeason;

                        if(_gameSeason > 3)
                        {
                            _gameSeason = 0;
                            gameSeason = (Season) _gameSeason;

                            gameYear++;

                            EventsHandler.TimeEventsHandler.CallAdvanceGameYearEvent(gameYear, gameSeason, gameDay, gameDayofWeek, gameHour, gameMinute, gameSecond);
                        }

                        EventsHandler.TimeEventsHandler.CallAdvanceGameSeasonEvent(gameYear, gameSeason, gameDay, gameDayofWeek, gameHour, gameMinute, gameSecond);
                    }
                    gameDayofWeek = GetGameDayofWeek();
                    EventsHandler.TimeEventsHandler.CallAdvanceGameDayEvent(gameYear, gameSeason, gameDay,gameDayofWeek, gameHour, gameMinute, gameSecond);
                }

                EventsHandler.TimeEventsHandler.CallAdvanceGameHourEvent(gameYear, gameSeason, gameDay, gameDayofWeek, gameHour, gameMinute, gameSecond);
            }

            EventsHandler.TimeEventsHandler.CallAdvanceGameMinuteEvent(gameYear, gameSeason, gameDay, gameDayofWeek, gameHour, gameMinute, gameSecond);
            
        }
    }

    private string GetGameDayofWeek()
    {
        int totalDay = (((int)gameSeason) * 30) + gameDay;
        int dayofWeek = totalDay % 7;

        switch (dayofWeek)
        {
            case 1:
                return "Mon";
            case 2:
                return "Tue";
            case 3:
                return "Wed";
            case 4:
                return "Thu";
            case 5:
                return "Fri";
            case 6:
                return "Sat";
            case 0:
                return "Sun";
            default:
                return "";
        }
    }

    internal void TestAdvanceGameMinute()
    {
        for (int i = 0; i < 60; i++)
        {
            gameSecond += 10;      
        }

    }

    internal void TestAdvanceGameDay()
    {
        for (int m = 0; m < 24; ++m)
        {
            for (int i = 0; i < 60; i++)
            {
                for (int j = 0; j < 60; j++)
                {
                    gameSecond += 1;
                }
            }
        }
    }  
}
