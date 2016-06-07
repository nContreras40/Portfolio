
//========================================================================
// Nathan Contreras
//    PlayerManager Class
//       PlayerManager class will keep track of both players, creating them,
//       determining the winner, etc.
//========================================================================


#pragma once
#if !defined(__PLAYERMANAGER_H__)
#define __PLAYERMANAGER_H__

#include <windows.h>
#include <d3d9.h>
#include <d3dx9.h>
#include <dinput.h>
#include "DxFramework\DXFramework.h"
#include "Bomberman\Player.h"
#include "Bomberman\Unit.h"



//========================================================================
class PlayerManager
{
public:
   PlayerManager();
   ~PlayerManager();

   //-------------------------------------------------------------------------------------------
   // Init allows for the creation of two different types of players, a set of default players,
   // or a set of customized players.  If a set of customized players is desired, the user
   // will need to create two player objects to their own specifications, and pass them into
   // init as paramaters.
   bool init( bool isPlayerOneMovingFirst, Player* playerOne = NULL, Player* playerTwo = NULL );
   bool update();
   bool shutdown();
   bool draw( IDXSPRITE spriteObj );

   bool endCurrentTurn();  // signals the PlayerManager to move priority to the next player
   
   void toggleActivePlayerAttackPhase();
   Player& getCurrentActivePlayer();
   Player& getCurrentInactivePlayer();
   
   int  getNumElapsedTurns();
   int  checkUnitTotals();
   void checkGameState(int result);

   int player1MessageBox();
   int player2MessageBox();
   int drawMessageBox();


private:
   int     elapsedTurns;
   bool    isPlayerOneActing;

public:
   Player* currentlyActingPlayer;
   Player* playerOnePtr;
   Player* playerTwoPtr;

   

};


#endif