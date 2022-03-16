# WhoDoneIt_MixItUp
A small project to help manage a 'Who Done It' game in a twitch chat using mix it up.

This project works by placing the netcoreapp3.0 folder from the release folder into the Mix It Up install directory under a new folder called 'ExternalApp'. The netcoreapp3.0 folder will need to be renamed to 'WhoDoneIt'.
From there commands can be made to call the application via the 'External Program' action in Mix It Up. Examples of the commands are included with the project.

The program has 3 actions it can perform:
- Adding a new guess (or updating a current guess)
- Checking the current guess
- Resetting the game for new guesses (for games that have multiple cases)

To run each of these commands a few parameters are needed. Most of the parameters are passed via the Mix It Up command but if running it manually for testing they are as follows:

For Adding:
Add "Game Title" Username "Name or description of character"

For Checking:
Check "Game Title" Username [optional: All] (include ALL if you are a mod and want to see all current guesses in chat.)

For Resetting:
Reset "Game Title" "Perpetrator name or description"
