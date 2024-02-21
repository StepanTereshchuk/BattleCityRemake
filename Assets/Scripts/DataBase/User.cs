using Firebase.Auth;
using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class UserData
{
    public string userId;
    public string email;
    public string name;
    public int maxStage;
    public int highScore;
    public int tanksDestroyed;
    [DoNotSerialize]
    public Texture2D userIconTexture;
    public UserData()
    {
        //userIconTexture = new Texture2D(100, 100);
    }

    public UserData(FirebaseUser firebaseUser)
    {
        userId = firebaseUser.UserId;
        email = firebaseUser.Email;
        name = firebaseUser.DisplayName;
        maxStage = 0;
        tanksDestroyed = 0;
        highScore = 0;
        userIconTexture = new Texture2D(100, 100);
    }

    public void SetTextureNativeSize()
    {
        userIconTexture = new Texture2D(100, 100);
    }

    public string UserToString()
    {
        return "Email : "+ email+", Name : " + name+ ", MaxStage :"+maxStage;
    }
}
