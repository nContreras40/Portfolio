
//========================================================================
// Nathan Contreras
//    Player Class
//       Player class will be in control of all of their units, and be
//       responsible for making actions with their units.
//========================================================================


#pragma once
#if !defined(__PLAYER_H__)
#define __PLAYER_H__

#include <windows.h>
#include <d3d9.h>
#include <d3dx9.h>
#include <dinput.h>
#include "DxFramework\DXFramework.h"
#include "Bomberman\Unit.h"
#include "Bomberman\GameStates.h"
#include "Utilities/Point.h"
#include "Bomberman\CollisionManager.h"

//========================================================================
class Player
{
// public methods/data
public:
   Player();   //Ctor
  ~Player();   //Dtor

  bool init( bool playerOne, int x, int y, Unit::Type unitType, int numUnits = 4 );
   bool update();
   bool shutdown();   

   // Getters
   Unit* getMyUnitArr();
   int   getMyUnitCount();
   void  decUnitCount(){ myUnitCount--; }
   void  incUnitCount(){ myUnitCount++; }
   bool  getPlayerOneStatus();
   
   // Signals
   bool unitKilled();
   bool playerLoses();
   Unit getUnit(int num);
   bool unitUpdate();
   bool unitDraw(IDXSPRITE spriteInterface);

   void unitClick( Point mousePos );
   void checkUnitDeaths();

   Unit getSelectedUnit();
   void setSelectedUnit( Unit& selectedUnit );

   void left();
   void down();
   void up();
   void right();
   void resetUnitMove();
   void stopAllUnits();
   void unitCollision();
   void singleUnitCollision();

   std::vector<Unit> getArrayUnits() { return myArrayUnits; }
   void checkWaterCollisions( CollisionManager& collisionManager, TiledBackground& levelRef );

   void toggleAttackState();

   void setAttackCursorRight();
   void setAttackCursorLeft();
   void setAttackCursorUp();
   void setAttackCursorDown();

   Unit& findUnitReceivingDamage( DxGameSprite attackCursor );

   void resetUnitMoves( );
   void resetUnits();

// private methods/data
private:
   Unit* myUnits;// The units that belong to this player

   int   myUnitCount;  // The current number of units that belong to this player
   int   myMaxUnits;   // The total number of units that initally belonged to this player
   bool  isPlayerOne;  // Whether or not this Player is player one

   vector<Unit> myArrayUnits;

   Unit         myDummyUnit;

public:
   bool  isAttacking;
   int   myTargetOffset;
   Unit  myPreviousSelectedUnit;
   Unit  mySelectedUnit;

   DxGameSprite   myAttackCursor;
};

#endif