﻿using System;

namespace RewriteMe.WebApi.Models
{
    public class UpdateAdministratorModel
    {
        public Guid Id { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}