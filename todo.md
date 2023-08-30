TODO:
Features:
	do pretty md formatting of this todo doc
	implement pause
	implement ability to update player's info in lobby 	
	implement game configuration 
	implement drawing walls by hand (map editor)
	make polygon collision calculation on GPU?
	make self-learning bots (https://towardsdatascience.com/how-to-teach-an-ai-to-play-games-deep-reinforcement-learning-28f9b920440a)

Architecture:
	Migrate to .Net 6
	Ditch using network communication for bots - as it impacts performance significantly when there are a lot of bots
	make a Class for all the objects conversions? 
	create iMovable, iCollidable interfaces; create base class for all game objects (asteroids, jets, bullets) so that they can be gathered in a single collection 
	don't transfer all the polygons over the network - calculate them on client right before drawing
    tie drawing to real time as well. currently it draws the frame whenever it's available, and that makes game sluggish
    (hard) make client-side prediction of the game state 
    migrate to .NET 6.0
    make a JavaScript web client
    Deploy to Azure 

Visual:
	ensure health 0 when killed
	make ship prettier, add wings
	slap ship sprites on top of polygons?
	make better maps

Gameplay:
	matches (rules, respawn,  kills/deaths count, etc )
	variable game speed 
	implement trajectory prediction 
	implement ship collisions	
	implement harpoon/grapple 
	implement gravity

DONE:
	add stars in the background
	enable shooting
	enable HUD
	remove unnecessary usings
	cleanup obsolete code 
	make separate client class to decouple the logic from UI
	implement proper connection <-> user management and Game Lobby
	implement more than 2 players	
	make HUDs for all the players, not just 2
	Pull out server class from client class
	make local game great again?
	implement game with bots 
	implement kick players/bots from lobby
	implement proper game loop with timer and without thread.sleep
	make UI in WPF, beacuse GDI+ is horrible for animation
	implement recording games as movies/GIFs
	change $safeprojectname$ (appears in task manager) to Planetes
	implement braking (deceleration)
	make pretty README.MD
	implement big world outside of screen and camera movement (viewport) 

BUGS:	
	If during debugging I stay in break mode for a bit more time then signalR disconnects with exception

FIXED:
	implement ACTUAL network game, not just localhost to localhost 
	walls can't be serialized for some reason
		Cause: if class has more than one non-default constructor, jsonserializer doesn't know which one to choose
	nullreference exception in yoke if you have mouse on game window when game is starting 	