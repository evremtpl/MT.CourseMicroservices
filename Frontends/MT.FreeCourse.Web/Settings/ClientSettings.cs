﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MT.FreeCourse.Web.Settings
{
    public class ClientSettings
    {
        public Client WebClient { get; set; }
        public Client WebClientForUser { get; set; }
    }

    public class Client
    {
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }
    }
}
