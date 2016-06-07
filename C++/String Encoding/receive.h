
//=====================================================================================================================================================================
// Author:  Nathan Contreras
//=====================================================================================================================================================================

#pragma once
#if !defined( __receive_h__ )
#define __receive_h__

//=====================================================================================================================================================================
// Receive a packet from the network, then return user's answers in a new packet.
//		The returned packet will have a format as follows:
//		A question-answer pair will have a terminating zero between the pair to seperate the question/answer pair, followed by two terminating zeros to denote the end
//    of the question/answer pair.  The end of the packet will be denoted by three terminating zeros.
//    
//    Visual Example:
//			packet: [ question \0 answer \0\0 question \0 answer \0\0 ... \0\0\0 ]
//=====================================================================================================================================================================
char* receivePacket ( char* packet );

#endif