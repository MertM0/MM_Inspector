using System.Collections.Generic;
using UnityEngine;

namespace MM.Inspector.Editor
{
    public static class MMLog
    {
        private static readonly HashSet<string> Reported = new HashSet<string>();

        public static void WarnOnce(string message)
        {
            if (string.IsNullOrEmpty(message) || !Reported.Add(message))
            {
                return;
            }

            Debug.LogWarning($"[MM_Inspector] {message}");
        }
    }
}
