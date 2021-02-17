using Domain.User;
using System;
using System.Threading.Tasks;
using FirebaseCoreSDK.Firebase.CloudMessaging.Models;

namespace Domain.Push
{
    public interface IPushSender
    {
        /// <summary>
        /// Send Data PUSH to device - won't be prompted to user
        /// </summary>
        /// <param name="pushToken">User device PUSH token</param>
        /// <param name="properties">Object describing what's happening</param>
        /// <returns>Guid of sent message</returns>
        Task<PushMessageResponse> PushToDevice(
            string pushToken, 
            NotificationDataContainer properties
        );

        /// <summary>
        /// Send Pop-Up PUSH to device - will be prompted to user
        /// </summary>
        /// <param name="pushToken">User device PUSH token</param>
        /// <param name="messageTitle">User-friendly title for pop-up</param>
        /// <param name="messageBody">User-friendly body text for pop-up</param>
        /// <param name="properties">Object describing what's happening</param>
        /// <returns>Guid of sent message</returns>
        Task<PushMessageResponse> PushToDevice(
            string pushToken, 
            string messageTitle, 
            string messageBody, 
            NotificationDataContainer properties = null
        );


        /// <summary>
        /// Send Pop-up PUSH to topic e.g. Chat
        /// </summary>
        /// <param name="topicName">Inner name of the topic - to be disclosed to Client App for Subscribe</param>
        /// <param name="properties">Object describing what's happening</param>
        /// <returns>Guid of sent message</returns>
        Task<PushMessageResponse> PushToTopic(
            string topicName, 
            NotificationDataContainer properties = null
        );

        /// <summary>
        /// Send Pop-up PUSH to topic e.g. Chat
        /// </summary>
        /// <param name="topicName">Inner name of the topic - to be disclosed to Client App for Subscribe</param>
        /// <param name="messageTitle">User-friendly title for pop-up</param>
        /// <param name="messageBody">User-friendly body text for pop-up</param>
        /// <param name="properties">Object describing what's happening</param>
        /// <returns>Guid of sent message</returns>
        Task<PushMessageResponse> PushToTopic(
            string topicName, 
            string messageTitle, 
            string messageBody, 
            NotificationDataContainer properties = null
        );
    }
}