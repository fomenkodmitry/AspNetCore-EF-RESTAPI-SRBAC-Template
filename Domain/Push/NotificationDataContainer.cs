using System;
using System.Collections.Generic;

namespace Domain.Push
{
    public class NotificationDataContainer
    {
        private Dictionary<string, string> _dictionary = new Dictionary<string, string>();

        public NotificationDataContainer(NotificationTypes notificationType, ObjectType objectType, string id)
        {
            _dictionary.Add("ObjectType", notificationType.ToString());
            _dictionary.Add("NotificationTypes", objectType.ToString());
            _dictionary.Add("Id", id);
        }

        public Dictionary<string, string> GetDictionary()
        {
            return _dictionary;
        }
    }
}