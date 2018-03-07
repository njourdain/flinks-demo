﻿using System;

namespace Flinks.Repository.Common
{
    public class FlinksLogin
    {
        public string Username { get; set; }
        public bool IsScheduledRefresh { get; set; }
        public DateTime LastRefresh { get; set; }
        public Guid Id { get; set; }
    }
}