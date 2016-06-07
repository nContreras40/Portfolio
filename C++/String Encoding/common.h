
//=====================================================================================================================================================================
// Author:  Nathan Contreras
//=====================================================================================================================================================================

#pragma once
#if !defined( __common_h__ )
#define __common_h__

//=====================================================================================================================================================================
// Get number security questions from the database, and store them into question buffer.
//=====================================================================================================================================================================
void getSecQuestions ( SecQuestion* qst, unsigned numQst );

//=====================================================================================================================================================================
// Encode security question packet.
//=====================================================================================================================================================================
void encodePacket ( char* packet, SecQuestion* questions, unsigned qstNum );

//=====================================================================================================================================================================
// Get number of characters in the question array.
//=====================================================================================================================================================================
unsigned getSizeOfQuestions ( SecQuestion* qstBuf, unsigned index );

#endif
