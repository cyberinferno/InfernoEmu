Inferno Emu A3 emulator for A3 Client version 562
==================================================

Overview of A3
---------------
A3 - Art, Alive, Attraction is a Korean MMOPRG released around 2003. English version of it was officially released in India in 2005 under the banner of Sify. Later due to some issues it was shut down. But the love for the game did not end there. Thanks to RaGEZONE Forum (http://forum.ragezone.com/f98/) some Indians were able to create A3 private server using the files released there. The released files were just executables compiled by the Chinese developers and the source of these executables were never released. 

This is an effort to create an open source emulator for A3. If you are developer with a passion for MMORPG you are welcome to help in its development.

Requirements
------------
1. Windows Operating System
2. .NET Framework 4.0
3. MSSQL server
4. Existing A3 MMORPG server with characters created. For guides creating a server visit http://forum.ragezone.com/f98/
5. Visual Studio 2010 (For making changes in the code and building it)

Running the Project
-------------------
1. Build the project in Visual Studio 
2. Edit config.ini with your configurations
3. Run the executable


Things that are working
------------------------
1. Username, password authentication of an account.
2. Duplicate login check
3. IP banning
4. Server maintenance message for players
5. Character display
6. Character diconnection from server
7. Character deletion

Known bugs
----------
Refer https://github.com/cyberinferno/InfernoEmu/issues

Things to be implemented in the near future
--------------------------------------------
1. Create a new character
2. Actually making character enter into the map its currently in


Thanks To
----------
1. getdeepz (https://www.facebook.com/DipakRNayak) for his invaluable help in packet structures and example codes!
2. Mihir (https://github.com/mihir7293) for contributing to the code base
