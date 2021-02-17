using Domain.Push;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FirebaseCoreSDK;
using FirebaseCoreSDK.Firebase.CloudMessaging.Models;

namespace Infrastructure.Push
{
    public class FirebasePushSender : IPushSender
    {
        private readonly ILogger _logger;
        private readonly FirebaseClient _firebaseClient;
        
        public FirebasePushSender(FirebaseClient firebaseClient, ILogger<FirebasePushSender> logger)
        {
            _firebaseClient = firebaseClient;
            _logger = logger;
        }

        public async Task<PushMessageResponse> PushToDevice(
            string pushToken, 
            NotificationDataContainer properties = null
        )
        {
            return await sendPush(PushTypes.Device, pushToken, null, null, false, properties);
        }

        public async Task<PushMessageResponse> PushToDevice(
            string pushToken, 
            string messageTitle, 
            string messageBody,
            NotificationDataContainer properties = null
        )
        {
            return await sendPush(PushTypes.Device, pushToken, messageTitle, messageBody, true,
                properties);
        }

        public async Task<PushMessageResponse> PushToTopic(
            string topicName,
            NotificationDataContainer properties = null
        )
        {
            return await sendPush(PushTypes.Topic, topicName, null, null, false, properties);
        }

        public async Task<PushMessageResponse> PushToTopic(
            string topicName, 
            string messageTitle, 
            string messageBody, 
            NotificationDataContainer properties = null
        )
        {
            return await sendPush(PushTypes.Topic, topicName, messageTitle, messageBody, true, properties);
        }

        private async Task<PushMessageResponse> sendPush(
            PushTypes pushType,
            string recipientId,
            string messageTitle,
            string messageBody,
            bool isPopUp,
            NotificationDataContainer properties
        )
        {
            
            //Сделано пока что только для Device
            try
            {
                await _firebaseClient.Auth.AuthenticateAsync();
                FirebasePushMessage message = new FirebasePushMessage()
                {
                    Token = recipientId,
                    Android = new Android()
                    {
                        Priority = Priority.High
                    },
                    Notification = new Notification()
                    {
                        Body = messageTitle,
                        Title = messageBody
                    },
                    Data = properties.GetDictionary()
                };
                return await _firebaseClient.CloudMessaging.SendCloudMessageAsync(message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown in Firebase Push Sender: {ex.Message}, {ex.StackTrace}");
            }

            return null;
        }
    }
}