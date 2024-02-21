using System;
using System.Collections.Generic;

[Serializable]
public class LeaderBoardData 
{
    public List<UserData> users;

    public LeaderBoardData()
    {
        users = new List<UserData>();
    }

}