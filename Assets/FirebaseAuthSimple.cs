using UnityEngine;
using TMPro;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Database;
using UnityEngine.SceneManagement;

// Firebase Authentication and Score Persistence
// Handles user login, registration, logout and saving scores to Firebase Realtime Database

public class FirebaseAuthSimple : MonoBehaviour
{
    FirebaseAuth auth;

    public TMP_InputField loginEmail;
    public TMP_InputField loginPassword;

    public TMP_InputField signupEmail;
    public TMP_InputField signupPassword;

    private DatabaseReference dbRef;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void Login()
    {
        auth.SignInWithEmailAndPasswordAsync(
            loginEmail.text.Trim(),
            loginPassword.text.Trim()
        ).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Login Failed");
                return;
            }
            Debug.Log("Login Success: " + task.Result.User.Email);
            SceneManager.LoadScene("SampleScene");
        });
    }

    public void SignUp()
    {
        auth.CreateUserWithEmailAndPasswordAsync(
            signupEmail.text.Trim(),
            signupPassword.text.Trim()
        ).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Signup Failed");
                return;
            }
            Debug.Log("Signup Success: " + task.Result.User.Email);
            SceneManager.LoadScene("SampleScene");
        });
    }
    public void Logout()
    {
        auth.SignOut();
        Debug.Log("Logged Out");
        SceneManager.LoadScene("Login");
    }
    
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    public void SaveScore(int score)
    {
        string userId = auth.CurrentUser.UserId;
        dbRef.Child("users").Child(userId).Child("highscore")
            .SetValueAsync(score);
        Debug.Log("Score saved: " + score);
    }
    
    public void LoadScore()
    {
        string userId = auth.CurrentUser.UserId;
        dbRef.Child("users").Child(userId).Child("highscore")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    Debug.Log("High Score: " + snapshot.Value);
                }
            });
    }
}