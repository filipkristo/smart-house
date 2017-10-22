using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
    public static class PhoneCallsStack
    {
        private const int TIMEOUT = 1;
        private static readonly List<PhoneCallData> phoneCalls = new List<PhoneCallData>();

        public static void AddPhoneCall(PhoneCallData phoneCall)
        {
            phoneCalls.Add(phoneCall);
        }

        public static bool ShouldStartWithMusic()
        {
            var oldphoneCalls = phoneCalls.Where(x => (DateTime.UtcNow - x.Started).TotalHours > TIMEOUT).ToList();

            if(oldphoneCalls.Any())
                oldphoneCalls.ForEach(x => phoneCalls.Remove(x));

            var oldestPhoneCall = phoneCalls.OrderByDescending(x => x.Started).FirstOrDefault();
            if (oldestPhoneCall != null)
                phoneCalls.Remove(oldestPhoneCall);

            return oldestPhoneCall != null && !PhoneCallActive();
        }

        public static bool PhoneCallActive()
        {
            return phoneCalls.Any(x => (DateTime.UtcNow - x.Started).TotalHours < TIMEOUT);
        }

        public static IEnumerable<PhoneCallData> AllPhoneCalls()
        {
            return phoneCalls;
        }
    }
}
