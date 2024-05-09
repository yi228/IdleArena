//using GooglePlayGames.BasicApi;
//using GooglePlayGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GPGSManager : MonoBehaviour
{
    void Start()
    {
        //SignIn();
#if UNITY_EDITOR
        DBManager.instance.clientUserName = "MorrieKim";
#else
                DBManager.instance.clientUserName = "1";
#endif
        DBManager.instance.LoadUserData();
    }
    //public void SignIn()
    //{
    //    PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    //}
    //internal void ProcessAuthentication(SignInStatus status)
    //{
    //    if (status == SignInStatus.Success)
    //    {
    //        DBManager.instance.clientUserName = PlayGamesPlatform.Instance.GetUserDisplayName();
    //        DBManager.instance.LoadUserData();
    //    }
    //    else
    //    {
    //        DBManager.instance.clientUserName = "MorrieKim";
    //        DBManager.instance.LoadUserData();
    //    }
    //}
}
