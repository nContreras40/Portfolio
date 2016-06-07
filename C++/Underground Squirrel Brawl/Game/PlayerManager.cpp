
#include <stdafx.h>
#include "Bomberman\PlayerManager.h"
#include "Bomberman\Player.h"
#include "Bomberman\Unit.h"
#include "Bomberman\GameStates.h"
#include "WinApplFramework\GameMessages.h"

//========================================================================
PlayerManager::PlayerManager()
{
}

//========================================================================
PlayerManager::~PlayerManager()
{
}

//========================================================================
bool PlayerManager::init( bool isPlayerOneMovingFirst, Player* ptrPOne,
                          Player* ptrPTwo )
{
   if ( isPlayerOneMovingFirst ) 
   {
      this->currentlyActingPlayer = ptrPOne;
   }
   else
   {
      this->currentlyActingPlayer = ptrPTwo;
   }

   elapsedTurns = 0;
   isPlayerOneActing = isPlayerOneMovingFirst;
   
   if ( ptrPOne == NULL ) // if default game is desired
   {
      playerOnePtr->init(true, 64, 64, Unit::largeGrey);
   }
   else // create custom players
   {
      playerOnePtr = ptrPOne;
   }

   if ( ptrPTwo == NULL ) // if default game is desired
   {
      playerTwoPtr->init(false, 256, 256, Unit::smallBrown);
   }
   else // else create custom players
   {
      playerTwoPtr = ptrPTwo;
   }

   return true;
}

//========================================================================
bool PlayerManager::update()
{
   playerOnePtr->update();
   playerTwoPtr->update();
   checkGameState(checkUnitTotals());
   return true;
}

//========================================================================
bool PlayerManager::draw( IDXSPRITE spriteObj )
{
   return ( playerOnePtr->unitDraw( spriteObj ) && playerTwoPtr->unitDraw( spriteObj ) );
}

//========================================================================
bool PlayerManager::shutdown()
{
   delete[] playerOnePtr;
   delete[] playerTwoPtr;

   return true;
}

//========================================================================
bool PlayerManager::endCurrentTurn()
{
   elapsedTurns++;

   if ( isPlayerOneActing )
   {
      isPlayerOneActing = false;
      this->currentlyActingPlayer = playerTwoPtr;
   }
   else
   {
      isPlayerOneActing = true;
      this->currentlyActingPlayer = playerOnePtr;
   }

   return true;
}

//========================================================================
void PlayerManager::toggleActivePlayerAttackPhase()
{
   if ( isPlayerOneActing )
   {
      playerOnePtr->toggleAttackState();
   }  
   else
   {
      playerTwoPtr->toggleAttackState();
   }
}

Player& PlayerManager::getCurrentActivePlayer()
{
   if ( isPlayerOneActing )
   {
      return *playerOnePtr;
   }  
   else
   {
      return *playerTwoPtr;
   }
}

Player& PlayerManager::getCurrentInactivePlayer()
{
   if ( isPlayerOneActing )
   {
      return *playerTwoPtr;
   }  
   else
   {
      return *playerOnePtr;
   }
}


//========================================================================
int PlayerManager::getNumElapsedTurns()
{
   return elapsedTurns;
}

//======   Yes = 6        No = 7     ========================================
int PlayerManager::checkUnitTotals()
{
   if ( playerOnePtr->getMyUnitCount() <= 0 &&
        playerTwoPtr->getMyUnitCount() <= 0 )
   {   
      return drawMessageBox();
   }
   else if ( playerOnePtr->getMyUnitCount() > 0 &&
             playerTwoPtr->getMyUnitCount() <= 0 )
   {
      return player1MessageBox();
   }
   else if ( playerOnePtr->getMyUnitCount() <= 0 &&
             playerTwoPtr->getMyUnitCount() > 0 )
   {
      return player2MessageBox();
   }
   else
   {
      return 0;
   }
}

//========================================================================
void PlayerManager::checkGameState(int result)
{
   if ( result == 6 ) //6 represents "YES" from the message boxes
   {
      GameMessages::signalStartNewGame();
   }
   else if ( result == 7 )  //7 represents "NO" from the message boxes
   {
      GameMessages::signalExitGame();
   }

}

//=======================================================================
int PlayerManager::player1MessageBox()
{
   string line0 = "Player 1 gets the nuts!\n\n";
   string line1 = "Would you like to play again?";

   string concatString = line0 + line1;
   
   return MessageBox(NULL, concatString.c_str(), "Player 1 Wins!", MB_YESNO | MB_ICONEXCLAMATION );
}

//=======================================================================
int PlayerManager::player2MessageBox()
{
   string line0 = "Player 2 gets the nuts!\n\n";
   string line1 = "Would you like to play again?";

   string concatString = line0 + line1;
   
   return MessageBox(NULL, concatString.c_str(), "Player 2 Wins!", MB_YESNO | MB_ICONEXCLAMATION );
}

//=======================================================================
int PlayerManager::drawMessageBox()
{
   string line0 = "All the squirrels were gone and the forest was forever empty...save for the nuts.\n\n";
   string line1 = "Care to try again?";

   string concatString = line0 + line1;
   
   return MessageBox(NULL, concatString.c_str(), "It's a draw!", MB_YESNO | MB_ICONEXCLAMATION );
}
