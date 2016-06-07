
#pragma once
#if !defined(__GAMESTATES_H__)
#define __GAMESTATES_H__

class GameStates
{
public:
   enum GS_Game_States
   {
      GS_DRAW = -1,
      GS_INPROG = 0,
      GS_P1_WINS = 1,
      GS_P2_WINS = 2
   };
};

#endif