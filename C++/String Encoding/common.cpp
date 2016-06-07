
#include <iostream>
#include <string>
#include "sqDb.h"

// Mircosoft Visual Studio warning for strcat function.
#pragma warning ( disable : 4996 )

using namespace std;

//---------------------------------------------------------------------------------------------------------------------------------------------------------------------
void getSecQuestions ( SecQuestion* qst, unsigned numQst )
{
	SecQuestion arrSecQst[5];
	arrSecQst[0] = getRandomSecurityQuestion( -1 ); 
	arrSecQst[1] = getRandomSecurityQuestion( arrSecQst[0].id ,-1 ); 
	arrSecQst[2] = getRandomSecurityQuestion( arrSecQst[0].id , arrSecQst[1].id, -1 ); 
	arrSecQst[3] = getRandomSecurityQuestion( arrSecQst[0].id , arrSecQst[1].id, arrSecQst[2].id, -1 ); 
	arrSecQst[4] = getRandomSecurityQuestion( arrSecQst[0].id , arrSecQst[1].id, arrSecQst[2].id, arrSecQst[3].id, -1 ); 

	for ( unsigned index = 0; index < numQst || index > 5; index++ )
	{
		qst[index] = arrSecQst[index];		
	}
}

//---------------------------------------------------------------------------------------------------------------------------------------------------------------------
void bufStrCat( char* dst, char* src ){
	while ( *dst != 0 )
		*dst++ = *src++;
}

//---------------------------------------------------------------------------------------------------------------------------------------------------------------------

void encodePacket ( char* packet, SecQuestion* questions, unsigned qstNum )
{
	char* endStr = packet;
	char* pStart = packet;
	int	pLen	 = 0;

	for ( unsigned index = 0; index < qstNum; index++ )
	{
		const char* temp = questions[index].text.c_str();
		
		bufStrCat( pStart, (char*)temp );

		pLen = strlen(temp) + 1;
		endStr += pLen;
		pStart = endStr + 1;
	}
	*(endStr+1) = 0;
}

//---------------------------------------------------------------------------------------------------------------------------------------------------------------------
unsigned getSizeOfQuestions ( SecQuestion* qstBuf, unsigned numQst )
{
	unsigned count = 0;
	for ( unsigned index = 0; index < numQst; index++ )
		count += strlen( qstBuf[index].text.c_str() );
	return count;
}

