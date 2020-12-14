using System.Collections.Generic;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Hosting;

namespace main_service.Services
{
    public class FcmService
    {
        public FcmService(IWebHostEnvironment environment)
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(environment.ContentRootPath + "/flutterfcm-78458-firebase-adminsdk-qpifp-9c3aa79fb7.json"),
            });
        }
        public async void SendMessage(List<string> registrationTokens, Dictionary<string, string> data)
        {
            var message = new MulticastMessage()
            {
                Tokens = registrationTokens,
                Data = data
            };
            var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
        }
    }
}