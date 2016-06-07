
#include <stdafx.h>
#include "Bomberman\Player.h"
#include "Bomberman\Unit.h"
#include "Bomberman\GameStates.h"

//========================================================================
Player::Player()
{

}

//========================================================================
Player::~Player()
{

}

//========================================================================
bool Player::init( bool playerOne, int x, int y, Unit::Type unitType, int numUnits )
{
   myUnitCount = myMaxUnits = numUnits;

   myArrayUnits.resize(numUnits);
   
   if( playerOne )
   {
      myTargetOffset = 64;
   }
   else
   {
      myTargetOffset = -64;
   }

   isPlayerOne = playerOne;

   for(unsigned index = 0; index < myArrayUnits.size(); index++)
   {
      myArrayUnits[index].init(unitType, x, y);
      myArrayUnits[index].getSprite().collidable(true);
      x += 64; 
   }

   myAttackCursor.setScale(0.5f, 0.5f);   
   myAttackCursor.create("ATTACK-CURSOR");
   isAttacking = false;

   myArrayUnits[0].setMove();

   myDummyUnit.init(Unit::blank, 0, 0);
   myDummyUnit.killUnit();
   myDummyUnit.getImage().collidable(false);

   return true;
}

//==========Determines what Unit was Clicked=============================================================
void Player::unitClick( Point mousePos )
{
   for( unsigned i = 0; i < myArrayUnits.size(); i++ )
   {
      if( myArrayUnits[i].getImage().getCollisionArea().contains( mousePos ) )
      {
         resetUnitMove();
         myArrayUnits[i].setMove();
         mySelectedUnit = myArrayUnits[i];
      }
   }
}

//=====================================================================================
void Player::checkUnitDeaths()
{
   for(unsigned index = 0; index < myArrayUnits.size(); index++)
   {
      if( myArrayUnits[index].checkIfDead() ||
         myArrayUnits[index].getHitWaterFlag() )
      {
         myArrayUnits[index].killUnit();
         decUnitCount();
      }
   }
}

//========Unit Collision===============================================================
void Player::unitCollision()
{	
   for(unsigned index = 0; index < myArrayUnits.size(); index++)
   {
      if( myArrayUnits[index].getHitWaterFlag() == false )
      {
         D3DXVECTOR3 snPos = myArrayUnits[index].getLastPosition();
         myArrayUnits[index].setMyPosition(snPos);
         myArrayUnits[index].incMovePoints();
      }      
   }
}

//=====================================================================================
void Player::singleUnitCollision()
{	
   for(unsigned index = 0; index < myArrayUnits.size(); index++)
   {
      if( myArrayUnits[index].getCanMove() == true )
      {
         D3DXVECTOR3 snPos = myArrayUnits[index].getLastPosition();
         myArrayUnits[index].setMyPosition(snPos);
         myArrayUnits[index].incMovePoints();
      }      
   }
}

//=====Resets All Units CanMove to false==================================================================
void Player::resetUnitMove()
{
   for(unsigned index = 0; index < myArrayUnits.size(); index++)
   {
      if(myArrayUnits[index].getCanMove() == true)
      {
         myArrayUnits[index].setMove();
      }
   }
}

//========Unit Drawing===============================================================
bool Player::unitDraw(IDXSPRITE spriteInterface)
{
   for(unsigned index = 0; index < myArrayUnits.size(); index++)
   {
      myArrayUnits[index].draw(spriteInterface);
   }

   if( isAttacking == true )
   {
      myAttackCursor.draw( spriteInterface );
   }


   return true;
}

//=======Unit Updates=======================================================================
Unit Player::getUnit(int num)
{
   return myArrayUnits[num];
}

//========================================================================
bool Player::update()
{
   for ( int i = 0; i < myArrayUnits.size(); i++ )
   {
      myArrayUnits[i].update();
   }
   return true;
}

//========================================================================
bool Player::shutdown()
{
   delete[] myUnits;

   return true;
}

//========================================================================
Unit* Player::getMyUnitArr()
{
   return myUnits;
}

//========================================================================
int Player::getMyUnitCount()
{
   return myUnitCount;
}

//========================================================================
bool Player::getPlayerOneStatus()
{
   return isPlayerOne;
}

//========================================================================
bool Player::unitKilled()
{
   myUnitCount--;
   return true;
}

//==========Untested::Gets the Current selected Units or returns the Previous Selected==============================================================
Unit Player::getSelectedUnit()
{
	for(unsigned index = 0; index < myArrayUnits.size(); index++)
	{
		if(myArrayUnits[index].getCanMove() == true)
		{
			myPreviousSelectedUnit = myArrayUnits[index];
			return myArrayUnits[index];
		}	
	}
	return myPreviousSelectedUnit;
}

//========================================================================
void Player::setSelectedUnit( Unit& selectedUnit )
{
   mySelectedUnit = selectedUnit;
}

//========Moves the Unit to the left================================================================
void Player::left()
{
	for(unsigned index = 0; index < myArrayUnits.size(); index++)
	{
		if(myArrayUnits[index].getCanMove() == true &&
         myArrayUnits[index].getMovementPoints() > 0 )
		{
			myArrayUnits[index].left();
         myArrayUnits[index].decMovePoints();
		}
	}
}

//========Stops all the Units===============================================================
void Player::stopAllUnits()
{
	for(unsigned index = 0; index < myArrayUnits.size(); index++)
	{
		myArrayUnits[index].stop();
	}
}

//============Moves the Unit to the down============================================================
void Player::down()
{
  for(unsigned index = 0; index < myArrayUnits.size(); index++)
	{
		if(myArrayUnits[index].getCanMove() == true &&
         myArrayUnits[index].getMovementPoints() > 0)
		{
			myArrayUnits[index].down();
         myArrayUnits[index].decMovePoints();
         mySelectedUnit.getImage().setXPosition( myArrayUnits[index].getSprite().getXPosition() );
         mySelectedUnit.getImage().setYPosition( myArrayUnits[index].getSprite().getYPosition() );
		}
	}
}

//===========Moves the Unit to the up=============================================================
void Player::up()
{
	for(unsigned index = 0; index < myArrayUnits.size(); index++)
	{
		if(myArrayUnits[index].getCanMove() == true &&
         myArrayUnits[index].getMovementPoints() > 0)
		{
			myArrayUnits[index].up();
         myArrayUnits[index].decMovePoints();
         mySelectedUnit.getImage().setXPosition( myArrayUnits[index].getSprite().getXPosition() );
         mySelectedUnit.getImage().setYPosition( myArrayUnits[index].getSprite().getYPosition() );
		}
	}
}

//===========Moves the Unit to the right=============================================================
void Player::right()
{
	for(unsigned index = 0; index < myArrayUnits.size(); index++)
	{
		if(myArrayUnits[index].getCanMove() == true &&
         myArrayUnits[index].getMovementPoints() > 0)
		{
			myArrayUnits[index].right();
         myArrayUnits[index].decMovePoints();  
         mySelectedUnit.setX( myArrayUnits[index].getSprite().getXPosition() );
         mySelectedUnit.setY( myArrayUnits[index].getSprite().getYPosition() );
		}
	}
}

//===================================================================================================
void Player::checkWaterCollisions( CollisionManager& collisionManager, TiledBackground&  myLevelRef )
{
   for( int i = 0; i < myArrayUnits.size(); i++ )
   {
      if( collisionManager.waterCollisions( myArrayUnits[i].getImage(), myLevelRef ) )         
      {
         if( myArrayUnits[i].getHitWaterFlag() == false )
         {
            myArrayUnits[i].setHitWaterFlag(true);
            decUnitCount();
         }         
      }
   }
}

void Player::toggleAttackState()
{
   isAttacking = !isAttacking; 
   
   if( isAttacking )
   { 
      myAttackCursor.setPosition( mySelectedUnit.getImage().getXPosition(), mySelectedUnit.getImage().getYPosition() );    
   } 
}

void Player::setAttackCursorRight()
{
   myAttackCursor.setPosition( mySelectedUnit.getImage().getXPosition() + 64, mySelectedUnit.getImage().getYPosition() + myTargetOffset);
}

void Player::setAttackCursorLeft()
{
   myAttackCursor.setPosition( mySelectedUnit.getImage().getXPosition() - 64, mySelectedUnit.getImage().getYPosition() + myTargetOffset );
}

void Player::setAttackCursorUp()
{
   myAttackCursor.setPosition( mySelectedUnit.getImage().getXPosition(), mySelectedUnit.getImage().getYPosition() - 64 + myTargetOffset);   
}

void Player::setAttackCursorDown()
{
   myAttackCursor.setPosition( mySelectedUnit.getImage().getXPosition(), mySelectedUnit.getImage().getYPosition() + 64 + myTargetOffset);   
}

Unit& Player::findUnitReceivingDamage( DxGameSprite attackCursor )
{
   for(int i = 0; i < myArrayUnits.size(); i++)
   {
      if( attackCursor.collidesWith( myArrayUnits[i].getImage() ) )
      {
         return myArrayUnits[i];
      }
   }

   return myDummyUnit;

}

void Player::resetUnitMoves( )
{
   for(int i = 0; i < myArrayUnits.size(); i++)
   {
      myArrayUnits[i].resetMovePoints();
   }
}

void Player::resetUnits()
{
   for(int i = 0; i < myArrayUnits.size(); i++)
   {
      myArrayUnits[i].resetUnit();      
   }
   myUnitCount = myMaxUnits;
}










