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
            try
            {
                var app = FirebaseApp.Create(new AppOptions
                {
                    Credential = credentialFile
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async void SendMessages(List<int> userIds, Dictionary<string, string> data)
        {
            try
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
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        
        public async void SendMessage(int userId, Dictionary<string, string> data, Notification notification)
        {
            try
            {
                var token = _fcmTokenRepository.GetToken(userId);
                var message = new Message
                {
                    Token = token,
                    Data = data,
                    Notification = notification,
                };
                var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}