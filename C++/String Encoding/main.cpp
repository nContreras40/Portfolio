
//=====================================================================================================================================================================
// Author:  Nathan Contreras
//    Program will encode a buffer of security questions
//=====================================================================================================================================================================

#include <iostream>
#include <iomanip>
#include "sqDb.h"
#include "send.h"
#include "receive.h"
#include "common.h"

using namespace std;

//---------------------------------------------------------------------------------------------------------------------------------------------------------------------
int main ( int argc, char* argv[], char* envstr[] )
{
	// Number of security questions to send.
	unsigned qNumber = 3;

	// Generate a new variable size buffer to store SecQuestion type.
	SecQuestion* pSecBuf = NULL;
   pSecBuf = new SecQuestion[ qNumber ];
	getSecQuestions( pSecBuf, qNumber );

	// Get the number of characters within the SecQuestion buffer.
	unsigned sizeOfQstBuf = getSizeOfQuestions( pSecBuf, qNumber );

	// Generate a new variable size buffer to store char.
	char* pBuffer = NULL;
	pBuffer = new char[ sizeOfQstBuf + qNumber + 1 ];
	pBuffer[0] = NULL;

	// Encode the packet
	encodePacket( pBuffer, pSecBuf, qNumber );
	delete[] pSecBuf;
	
	sendPacket( pBuffer );
	delete[] pBuffer;
}