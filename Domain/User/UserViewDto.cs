using System;
using IModel = Domain.Core.IModel;

namespace Domain.User
{
    public class UserViewDto : IModel
    {
        public Guid Id { get; set; }
    }
}