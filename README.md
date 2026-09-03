# DAT602-Ultimate-Goose-Fighter-Assignment
Repo for my game for my DAT602 assignment, contains sprite assets, a visual paradigm file containing the ERD, a word document (containing the game description, storyboards, ERD, CRUD table and DDL explaination), the sprite assets svg, a sql file with a single T-SQL procedure (that creates the database, all tables, constraints, and test data), another sql file (with test stored procedures which are called by the winform app prototype), and the winform app prototype itself.

To test the project, first, run DAT602_Assignment_1_DDL_Script.sql against your SQL server instance to set up the database and all necessary tables.

Then run DAT602_assignment_1_test_procedures.sql to set up the test procedures that will be used with the winform app. 

Then, open GooseFighterApp in Visual Studio. You may need to edit the App.config if you are not running your SQL server on localhost (which is what it is currently set to). Then click F5 to run and test the app.

TEST ACCOUNTS INFO:

Username:       Password:        Role:

ZandiGoose      Password123      Regular player.
AdminGoose      AdminPass123!    Administrator.
LockedGoose     wrongpass        Locked out account.

Note that entering LockedGoose's credentials wont work unless you literally type the password as "wrongpass". This is temporary just to show that lockout functions. 