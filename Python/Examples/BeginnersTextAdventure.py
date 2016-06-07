
# =============================================================
# Author:  Nathan Contreras
# Date:    06/07/2016
# Purpose: Instructional
# Details:
#   Provide a simple example of a linear text adventure
#   game with no back-tracking capabilities.  This is not an
#   extremely clean way to do a text adventure game, but is
#   a great way to easily introduce someone to the basic logic
#   of a text adventure game.
# =============================================================

# =============================================================
# Variable declarations at the top of the file, this is just
# a clean convention of keeping variables organized.
# Variables are seperated by type, and follow camelCase format.

introduction = "Here begins the adventure...\n"

mainRoom = "You look around, and see three doors."
secondRoom = "You find yourself in another room. There is nothing to do here."

leftDoor = "The door leads to your doom!!!"
middleDoor = "The door leads to much questioning"
rightDoor = "The door leads to your salvation!"

doorChoice = ""

validInput = False
isPlaying = True
isRestarting = False

# ===========================================================
# Game "loop" begins here:

# Prints the introductory text, followed by the main room text.
print introduction
print mainRoom

# This line can roughly translate into:
# "If validInput is not valid, get input from the user"
# Once the user enters input that is valid, the program
# will fall out of this while loop.
while validInput == False:

    # ===========================================================
    # We will store user input into the doorChoice variable now
    doorChoice = raw_input("Which door will you take, left, middle, or right:  ")

    # If the choice is left, print the string in leftDoor
    if doorChoice == "left":
        print("You have selected door: " + doorChoice)  # Show the user which door they picked
        validInput = True  # We set validInput to true
        print leftDoor     # Print the respective text
        raw_input("You have died, press enter to restart the game.")  # Force input from the user,
        # >>>                                                         # so the program doesn't close immediately.
        quit()  # Quit the process.

    # If the choice is right, print the string in rightDoor
    elif doorChoice == "right":
        print("You have selected door: " + doorChoice)  # Show the user which door they picked
        validInput = True  # We set validInput to true
        print rightDoor    # Print the respective text

    # If the choice is middle, print the string in middleDoor
    elif doorChoice == "middle":
        print("You have selected door: " + doorChoice)  # Show the user which door they picked
        validInput = True  # We set validInput to true
        print middleDoor   # Print the respective text

    # If no valid option entered, do NOT set validInput to true.
    else:
        validInput = False  # While not necessary to state, it is safe to explicitly say that validInput
        # >>>               # is explicitly false at this point in time.
        print "I didn't understand that, try again."  # Let the user know they entered something funky.

# When we overcome the first room, display dialogue of second room
print secondRoom

# The game will end after the above line.  This is a great place for you
# to begin moving forward and making a lengthier adventure.
