
//=====================================================================================================================================================================
// Author:  Nathan Contreras
//=====================================================================================================================================================================

#include <iostream>
#include <iomanip>
#include <string>
#include "sqDb.h"
#include "receive.h"
#include "common.h"

// Mircosoft Visual Studio warning for strcat function.
#pragma warning ( disable : 4996 )

using namespace std;

//=====================================================================================================================================================================
unsigned getNumberOfQuestions( char* packet )
{
	unsigned count = 0;

	while ( ( *packet != 0 ) && ( *(packet+1) != 0 ) )
	{
		packet++;
		if ( *packet == '?' )
		{
			count++;
		}
	}

	return count;
}

//=====================================================================================================================================================================
// Generate a sub-string of the passed cstring.
//		source is the cstring to be copied.
//    destination is the location of 
//		num is the number of characters to be copied.
//=====================================================================================================================================================================
char* n_cSubstr ( char* source, char* destination, unsigned num )
{
	unsigned index = 0;
	char* newSubStr = NULL;
	newSubStr = new char[ num + 1 ];

	while( index < num )
		newSubStr[index++] = *source++; 

	newSubStr[index] = '\0';

	return newSubStr;
}

//=====================================================================================================================================================================
// Parse the packet for the specified number of questions and store within an array of SecQuestions
//=====================================================================================================================================================================
void parseQuestionPacket( char* packet, SecQuestion* qstArr )
{
	unsigned qstCount = getNumberOfQuestions( packet );
	char* pStart = packet;
	char* pEndQst = NULL;

	for ( unsigned index = 0; index < qstCount ; index++ )
	{
		while ( *packet != '0' )
			packet++;

		pEndQst = packet;

		if ( *packet == '0' )
		{
			char* tempCStr = NULL;
			tempCStr = new char[ pEndQst - pStart + 1 ];
			tempCStr = strncpy( pStart, tempCStr, ( pEndQst - pStart ) + 1 );
			qstArr[index].text = tempCStr;
		}

		pStart = ++packet;
	}
}

//=====================================================================================================================================================================
// Receive a packet from the network, then return user's answers in a new packet.
//		The returned packet will have a format as follows:
//		A question-answer pair will have a terminating zero between the pair to seperate the question/answer pair, followed by two terminating zeros to denote the end
//    of the question/answer pair.  The end of the packet will be denoted by three terminating zeros.
//
//    Visual Example:
//			packet: [ question \0 answer \0\0 question \0 answer \0\0 ... \0\0\0 ]
//=====================================================================================================================================================================
char* receivePacket ( char* packet )
{
	unsigned questionCount = getNumberOfQuestions( packet );
	SecQuestion* pQuestions = NULL;
	pQuestions = new SecQuestion[ questionCount ];
	
	parseQuestionPacket( packet, pQuestions );	
	
	for ( unsigned i = 0; i < questionCount; i++ )
		cout << pQuestions[i].text << endl;

	//pAnswers = getUserInput;
	//char replyPacket[] = encodeReplyPacket(pQuestions, pAnswers);
	
	delete[] pQuestions;
	//delete[] pAnswers;

	
	
	// TODO: CHANGE BELOW, MEANT TO MAKE PROGRAM COMPILE, NOT RUN
	// TODO: CHANGE BELOW, MEANT TO MAKE PROGRAM COMPILE, NOT RUN
	// TODO: CHANGE BELOW, MEANT TO MAKE PROGRAM COMPILE, NOT RUN

	char* newPacket = NULL;
	return newPacket;
}
