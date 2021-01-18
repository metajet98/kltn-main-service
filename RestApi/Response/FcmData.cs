using System;
using System.Collections.Generic;
using FirebaseAdmin.Messaging;

namespace main_service.RestApi.Response
{
    public class FcmData
    {
        public static Dictionary<string, string> CreateFcmData(String activity, Dictionary<string, string> data)
        {
            var result = new Dictionary<string, string>
            {
                {"click_action", "FLUTTER_NOTIFICATION_CLICK"},
                {"activity", activity},
            };
            if (data != null)
            {
                foreach (var (key, value) in data)
                {
                    result.Add(key, value);
                }
            }
            return result;
        }
        
        public static Notification CreateFcmNotification(String title, String body, String imageUrl)
        {
            return new Notification
            {
                Body = body,
                Title = title,
                ImageUrl = imageUrl
            };
        }
    }
}