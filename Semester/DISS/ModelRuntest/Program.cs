﻿using DISS_Model_Elektrokomponenty;

class Program
{
    static void Main(string[] args)
    {
        Constants.POCET_OBSLUZNYCH_MIEST = 15;
        Constants.POCET_POKLADNI = 6;
        Core core = new Core(100, 0);
        core.RunDebug();
    }
}