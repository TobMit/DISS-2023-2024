﻿using DISS_Model_Elektrokomponenty;

class Program
{
    static void Main(string[] args)
    {
        Core core = new Core(50_000, 0, 20,3);
        core.SlowDown = false;
        core.SlowDownSpeed = 1;
        core.RunDebug();
    }
}