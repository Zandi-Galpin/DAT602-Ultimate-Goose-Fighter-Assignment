# DAT602-Ultimate-Goose-Fighter-Assignment
Repo for my game for my DAT602 assignment, contains sprite assets, a visual paradigm file containing the ERD, a word document (containing the game description, storyboards, ERD, CRUD table and DDL explaination), the sprite assets svg, a sql file with a single T-SQL procedure (that creates the database, all tables, constraints, and test data), another sql file (with test stored procedures that arent used anymore), another sql file which has the stored procedures for all 12 CRUD processes for the app, and the winform app prototype itself.

To test the project, first, run DAT602_Assignment_1_DDL_Script.sql against your SQL server instance to set up the database and all necessary tables.

Then run DAT602_assignment_2_Game_Play_Procedures.sql to create all the stored procedures. The test calls in it change some of the data, so after this, run DAT602_Assignment_1_DDL_Script.sql again if you want the same test data from before.

Then, open GooseFighterApp in Visual Studio. You may need to edit the App.config if you are not running your SQL server on localhost (which is what it is currently set to). Then click F5 to run and test the app.

TEST ACCOUNTS INFO:

Username:       Password:        Role:

ZandiGoose      Password123      Regular player.
AdminGoose      AdminPass123!    Administrator.
LockedGoose     wrongpass        Locked out account.

Note that entering LockedGoose's credentials wont work unless you literally type the password as "wrongpass". This is temporary just to show that lockout functions. 

More details for using the app:

Login form: log in, or fill in the email box as well and click Register to make a new account. Five wrong passwords locks an account. Admin accounts open the Administration form, everyone else opens Gameplay. There is also a Test DB Connection button.

Game Play form: move, attack, pick up/drop items, store items in your nest and take them back out, and delete your account. The board table shows every tile and what is on it. The World events buttons (spawn a random item, move or respawn the NPC) are there for the timers the game in the next stage use. The box at the bottom shows the status each action returned (bottom left).

Administration form: a Players tab (add, update, delete players, unlock accounts), a Board tab (place and delete tiles, place and remove items) and a Game tab (kill the running game).

To try combat or the kill running game button, run the app twice and log in as a different player in each one.