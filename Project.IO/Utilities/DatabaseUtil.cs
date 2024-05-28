﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using FireSharp;
using Project.IO.Classes;

namespace Project.IO.Utilities
{
    internal class DatabaseUtil
    {

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "CEqjaEf9otao0GhFCJEf3vuBlNn3vBMsj8fYVapR",
            BasePath = "https://projectio-8c677-default-rtdb.europe-west1.firebasedatabase.app/"
        };

        FirebaseClient? client;

        public FirebaseClient CreateConnection()
        {

            client = new FirebaseClient(config);

            if (client != null)
            {
                System.Diagnostics.Debug.WriteLine("connection established");

                return client;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("connection not established something went wrong");

                return null;
            }

        }

    }
}