/*
	Copyright © 2013, InfernoEmu Project
	All rights reserved.
	
	This file is part of InfernoEmu.

	InfernoEmu is free software: you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation, either version 3 of the License, or
	any later version.

	InfernoEmu is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with InfernoEmu.  If not, see <http://www.gnu.org/licenses/>.
*/

#region Includes

using System;

#endregion

namespace Login_Server
{
    public static class Command
    {
        /// <summary>
        /// Listens for user input in command line.
        /// </summary>
        public static void Listen()
        {
            while (Program.Running)
            {
                if (Console.ReadKey(true).Key != ConsoleKey.Enter) continue;
                Console.Write("$" + Environment.UserName.ToLower() + "@LoginServer> ");
                string input = Console.ReadLine();

                if (input != null)
                    if (input.Length > 0)
                    {
                        ProcessInput(input.Split(' '));
                    }
            }
        }

        /// <summary>
        /// Handles command line input. Arguments split by space character.
        /// </summary>
        private static void ProcessInput(string[] Args)
        {
            switch (Args[0].ToLower())
            {
                case "stop":
                case "exit":
                    Console.WriteLine("Stopping login server");
                    Program.Stop();
                    break;
                default:
                    Console.WriteLine("Okay I got it!");
                    break;
            }
        }
    }
}