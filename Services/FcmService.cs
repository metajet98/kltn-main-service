using System;
using System.Collections.Generic;
using System.IO;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using main_service.Repositories;
using Microsoft.AspNetCore.Hosting;

namespace main_service.Services
{
    public class FcmService
    {
        private readonly FcmTokenRepository _fcmTokenRepository;
        public FcmService(IWebHostEnvironment environment, FcmTokenRepository fcmTokenRepository)
        {
            _fcmTokenRepository = fcmTokenRepository;
            var credentialFile =
                GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, 
                    "flutterfcm-78458-firebase-adminsdk-qpifp-9c3aa79fb7.json"));
            if (FirebaseApp.DefaultInstance != null) return;
            var app = FirebaseApp.Create(new AppOptions
            {
                Credential = credentialFile
            });
        }

        public async void SendMessage(List<int> userIds, Dictionary<string, string> data)
        {
            var tokens = _fcmTokenRepository.GetTokens(userIds);
            var messages = new MulticastMessage
            {
                Tokens = tokens,
                Data = data,
                Notification = null,
            };
            var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(messages);
        }
    }
}