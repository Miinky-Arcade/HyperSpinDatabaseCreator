HyperSpinDatabaseCreator
========================

The HyperSpin Database Creator creates a new database file with just the ROMs you have available. This is significant because HyperSpin by default will load the front end with every game that was ever available for a given system rather than showing just the games that you have. This results in finding a game that you are interested in playing, only to find out that you don't actaully have that game. This can be frustrating and give the impression of an incomplete product when using HyperSpin in a party like atmosphere.

## Instructions

Enter your file path to the XML database that HyperSpin installs. This database includes every game available for a particular system. This is what the database creator uses as a default.

Look in your database directory that was provided and you will see a new XML file with the suffix "_custom" on it. This is your new database file that matches only the ROMs you have in your directory.

You are now free to rename this file to match whatever HyperSpin is looking for. In many cases, this just means renaming the full XML database to be *_full.xml and renaming the custom created file to match what the full database was. This is not done automatically so that the database can be verified before you make the decision to use the database.

You can repeat these steps on each database until you have all of you database files updated.

## How does it work?

The HyperSpin Database Creator will simply load the database file provided and compare it to the ROMs that you have in the directory that you provided. If a name of a ROM matches a name in the database, we'll keep track of that and create a new file with the all the ROMs that matched both the database and the ROM. Once the new database has been constructed, it is saved to the same directory as the database path with the name *_custom.xml. If that file exists, it will be overwritten. The original, full, database that was provided will be untouched.

