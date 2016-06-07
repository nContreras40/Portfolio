
//=====================================================================================================================================================================
// Author:  Nathan Contreras
//=====================================================================================================================================================================
#include <iostream>
#include "send.h"
#include "receive.h"

//=====================================================================================================================================================================
// "Sends" a packet from the network to the requesting user.  Returning the user's answers to the security questions.
//		The returned packet will have a format as follows:
//		A question-answer pair will have a terminating zero between the pair to seperate the question/answer pair, followed by two terminating zeros to denote the end
//    of the question/answer pair.  The end of the packet will be denoted by three terminating zeros.
//    
//    Visual Example:
//			packet: [ question \0 answer \0\0 question \0 answer \0\0 ... \0\0\0 ]
//=====================================================================================================================================================================
char* sendPacket ( char* packet )
{
	return receivePacket( packet );
}