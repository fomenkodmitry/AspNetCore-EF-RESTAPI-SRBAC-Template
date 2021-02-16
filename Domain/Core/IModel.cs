using System;

namespace Domain.Core
{
    public interface IModel
    {
        Guid Id { get; set; }
    }
}