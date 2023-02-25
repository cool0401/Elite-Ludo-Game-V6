using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Google;

public class SignIN : MonoBehaviour
{

    public string webClientId = "";

    private GoogleSignInConfiguration configuration;
    private Authenticator authenticator;

   private void Awake()
   {
        configuration = new GoogleSignInConfiguration
        {
            WebClientId = webClientId,
            RequestIdToken = true,
            RequestEmail = true,
            RequestAuthCode = true,
            RequestProfile = true
        };

        authenticator = FindObjectOfType<Authenticator>();
        print(configuration.WebClientId);
   }

    public void OnSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
      

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
          OnAuthenticationFinished);
    }

    public void OnSignOut()
    {
    
        GoogleSignIn.DefaultInstance.SignOut();
    }

    public void OnDisconnect()
    {

        GoogleSignIn.DefaultInstance.Disconnect();
    }

    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        /*authenticator.GoogleSuccessCallback("LBludobattle69", "ludobattle69@gmail.com", 
            "", 
            "ludobattle69@gmail.com", "ludobattle69@gmail.com");*/
        if (task.IsFaulted)
        {
            Debug.Log("PEDRO - Google Sign In Faulted.");
            using (IEnumerator<System.Exception> enumerator =
                    task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error =
                            (GoogleSignIn.SignInException)enumerator.Current;
                    Debug.Log("PEDRO - Google Sign In Error: " + error.Status + " " + error.Message);
                    authenticator.GoogleErrorCallback("Failed  " + error +  " ");

                }
                else
                {
                    authenticator.GoogleErrorCallback("Fail");

                }
            }
        }
        else if (task.IsCanceled)
        {
            Debug.Log("PEDRO - Google Sign In Cancelled.");
            authenticator.GoogleErrorCallback("Cancelled  ");
        }
        else
        {
            Debug.Log("PEDRO - Google Sign In Succeeded.");
            print(task.Result.UserId);
            authenticator.GoogleSuccessCallback(task.Result.UserId, task.Result.Email, task.Result.ImageUrl.ToString(), task.Result.DisplayName, task.Result.IdToken);
        }
    }

    public void OnSignInSilently()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
  
        GoogleSignIn.DefaultInstance.SignInSilently()
              .ContinueWith(OnAuthenticationFinished);
    }


    public void OnGamesSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = true;
        GoogleSignIn.Configuration.RequestIdToken = false;

  
        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
          OnAuthenticationFinished);
    }
    public void SignInGoogle()
    {
        OnSignIn();
    }


}
