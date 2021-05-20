The game has 4 premade levels.
In the unity hierarchy go to 'Managers'.
Under 'Level Manager' click Load Levels.
Ready to play.

_____________________________________________________________________________________________________________________

How to create your own levels:

1. Make or edit a *.txt file.
	
	First start with the level name followed by ':'.
	Now every row of your level.
	End the last row with ';'.
	Go back to step 1 if you want to add more levels.
		
	Example:
		
		Example Level 1:
		  ###
		  #-#
		###-###
		#P--X*#
		###-###
		  #O#
		  ###;
		
		Example Level 2:
		***
		*P*
		###;
	
	
		'#' is wall.
		'-' is ground.
		'X' is box.
		'*' is point.
		'O' is solved box.
		'P' is player.
		' ' is dint.

	
		For more information take a look at the premade levels file named.

2. Load the level.

	In the unity hierarchy go to 'Managers'.
	Draw you levels.txt file in the Levels To Load bar under the Level Manager script.
	Click Load Levels.

3. Play your levels.
	
	Start the game and your first level should be loaded.


* Don't forget to Click Load Levels every time you update the levels file.