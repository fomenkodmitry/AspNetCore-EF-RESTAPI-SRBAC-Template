using System.ComponentModel.DataAnnotations;

namespace Domain.Authenticate
{
    /// <summary>
    /// Save push-token
    /// </summary>
    public class SavePushTokenDto
    {
        /// <summary>
        /// push-token 
        /// </summary>
        [Required]
        public string PushToken { get; set; }
    }
}