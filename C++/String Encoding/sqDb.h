
// Author: Scott Strentzsch
//  Simple interface to get a random security question from a list of predefined questions.
//  FOR SCHOLASTIC USE ONLY.  This *IS NOT* workable in any real world scenario!
//

#pragma once
#if !defined( __sqDb_H__ )
#define __sqDb_H__

#include <iostream>
#include <string>

struct SecQuestion
{
   std::string text;    // english text value of the security question  
   int         id;      // unique identifier of security question
};


// Simple interface to get a random security question, with option to
// skip list of specific questions, id's for which are provided as parameters.
// *NOTE* - LAST ID IN LIST ***MUST*** be (-1)
SecQuestion getRandomSecurityQuestion ( int firstSkipId = -1, ... );


#endif