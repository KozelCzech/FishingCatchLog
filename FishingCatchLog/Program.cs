using FishingCatchLog;

int choice = 1;

// figure out other additional features
// TODO: Remake app in winforms once all the basic features are in place
//consider using MAUI instead to make it mobile compatible, or learn android developement

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
        case 4:
            DisplayMenu.DisplayBucketListMenu();
            break;
        case 5:
            DisplayMenu.TargetingMenu();
            break;
        case 0:
            return 0;
            break;
        default:
            break;
    }
}
