
// Author: Scott Strentzsch
// sqDb.cpp 
//    Security question "database" and access function(s)

#include <iostream>
#include <string>
#include <vector>
#include <algorithm>
#include <cassert>
#include <cstdarg>
#include "sqDb.h"

using namespace std;

#define CSTR2(x) #x
#define CSTR(x) CSTR2(x)

static const char* questionDatabase[] = {
   "Who was your childhood hero?",
   "Who is your favorite actor, musician, or artist?",
   "Which is your favorite web browser?",
   "What is your favorite movie?",
   "Where were you when you had your first kiss?",
   "Where were you when you first heard about 9/11?",
   "Where were you New Year's 2000?",
   "When you were young, what did you want to be when you grew up?",
   "What's your best friend's middle name?",
   "What were the last four digits of your childhood telephone number?",
   "What was your high school mascot?",
   "What was your favorite place to visit as a child?",
   "What was your childhood nickname?",
   "What was the name of your second dog/cat/goldfish/etc?",
   "What was the name of your elementary / primary school?",
   "What is the first name of the boy or girl that you first kissed?",
   "What was the name of the boy/girl you had your second kiss with?",
   "What was the make of your first car?",
   "Where were you when you had your first alcoholic drink (or cigarette)?",
   "What was the last name of your third grade teacher?",
   "What time of the day (hh:mm) were you born?",
   "What street did you grow up on?",
   "What primary school did you attend?",
   "What is your youngest brother's birthday?",
   "What is your spouse or partner's mother's maiden name?",
   "What is your oldest cousin's first and last name?",
   "Which phone number do you remember most from your childhood?",
   "What is your mother's maiden name?",
   "What is your maternal grandmother's maiden name?",
   "What is the name of your worst boss ever?",
   "What is your paternal grandmother's maiden name?",
   "Which would you join, Army, Air Force, Navy, Marines, Coast Guard, or Peace Corp?",
   "In what town or city did you meet your spouse/partner?",
   "What is your mother's hometown?",
   "What is your favorite movie?",
   "What is your father's hometown?",
   "What name do you wish your parents had given you?",
   "What is your favorite color?",
   "What is your father's middle name?",
   "What is the name of your grandmother's favorite pet?",
   "What is the name of your first school?",
   "What is the name of your first grade teacher?",
   "What is your oldest sibling's birthday (mm/dd/yy)?",
   "What is the name of your favorite pet?                           ",
   "What is the middle name of your oldest child?",
   "What is the first name of your last boyfriend or girlfriend?",
   "In what town or city did your mother and father meet?",
   "What is the last name of your first boyfriend or girlfriend?",
   "What high school did you attend?",
   "What are the last five digits of your driver's license number?",
   "What are the last five digits of your driver's licence number?",
   "What city was your first full time job?",
   "What was your first boss's name?",
   };

static const int g_numQuestions = (sizeof(questionDatabase) / sizeof(questionDatabase[0])) ;


// Little better random range function, improves upon (rand() % n)
//    Why?  (rand() % range) skews distribution to lower range.
static unsigned random ( int rMin, int rMax )
{
   size_t rn = rand();
   double multiplier = double(rn) / double(RAND_MAX);
   unsigned range = rMax - rMin + 1;
   return  int(multiplier*range) + rMin;
}


//
// Get a random security question, with provision made for skipping a list of questions
// The "skipping" provision IS NOT IMPLEMENTED, as the question list doesn't have any ID#'s
// The code below is written 
//
SecQuestion getRandomSecurityQuestion ( int firstSkipId, ... )
{
   int rn = -1;
   int skipCount = 0;
   vector<int> vSkip;
   
   if ( firstSkipId == -1 )
   {
      // simplest case: just generate one RN and return the string
      rn = random( 0, g_numQuestions-1 );
   }
   else
   {
      // Uses provided ID#s of security questions to skip, so must do some work - darn.
      va_list  vl;
      va_start( vl, firstSkipId );
      
      vSkip.push_back( firstSkipId );  // don't forget the 1st ID!!!

      int num;
      skipCount = 0;
      // Get all id's from parameters, terminated by a (-1).  Safety: Stop if more ids than questions!
      while ( (num = va_arg( vl, int )) != -1 && (skipCount++ < g_numQuestions) )
      {
         vSkip.push_back( num );
      }

      // Generate RN's until a RN not in skipped ID's list is found
      std::vector<int>::iterator iter = vSkip.end();
      skipCount = g_numQuestions * 100;   // heuristic (i.e., bug) for case Id's list contains EVERY id
      do
      {
         rn = random( 0, g_numQuestions-1 );
         iter = std::find( vSkip.begin(), vSkip.end(), rn );
      }
      while ( iter != vSkip.end() && skipCount-- >= 0);

   }

   if ( rn == -1 || skipCount < 0 )
   {
      cerr << "Oops.  Something wrong here - didn't generate a random security question\n";
      assert(false);
      std::logic_error( __FILE__ ":" __FUNCTION__ ":" CSTR(__LINE__) ": FATAL LOGIC ERROR\n" );
   }
   
   SecQuestion sc;
   sc.text = questionDatabase[rn];
   sc.id   = rn;

   return sc;
}