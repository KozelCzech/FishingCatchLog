using FishingCatchLog;

int choice = 1;

while (true)
{
    choice = DisplayMenu.MainMenu();

    switch (choice)
    {
        case 1:
            DisplayMenu.LogCatchMenu();
            break;
        case 2:
            DisplayMenu.SearchCatchesMenu();
            break;
        case 3:
            DisplayMenu.DisplayAllCatchesMenu();
            break;
        case 0:
            Environment.Exit(0);
            break;
        default:
            break;
    }
}
