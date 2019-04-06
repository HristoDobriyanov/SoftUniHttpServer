using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace SoftUniHttpServer
{
    public class SessionManager
    {
        private readonly IDictionary<string, int> sessionData = new ConcurrentDictionary<string, int>();

        void CreateSession(string sessionId)
        {
            this.sessionData.Add(sessionId, 0);
        }

        public int GetSession(string sessionId)
        {
            this.sessionData.TryGetValue(sessionId, out int value);
            return value;
        }



    }
}
