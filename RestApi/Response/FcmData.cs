using System;
using System.Collections.Generic;

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
            foreach (var (key, value) in data)
            {
                result.Add(key, value);
            }
            return result;
        }
    }
}