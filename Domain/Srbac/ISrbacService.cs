﻿using System.Threading.Tasks;

namespace Domain.Srbac
{
    public interface ISrbacService
    {
        public bool CheckPermission(SrbacRoles role, SrbacPermissions permission);
    }
}