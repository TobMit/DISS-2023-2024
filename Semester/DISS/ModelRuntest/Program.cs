﻿using DISS_Model_Elektrokomponenty;

class Program
{
    static void Main(string[] args)
    {
        Core core = new Core(25_000, 0, 13,4);
        core.SlowDown = false;
        core.SlowDownSpeed = 1;
        core.RunDebug();
    }
}