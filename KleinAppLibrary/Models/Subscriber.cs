using System;

namespace KleinMapLibrary.Models
{
    public class Subscriber
    {
        public int Id { get; set; }
        public string MailAddress { get; set; }
        public int IsVerify { get; set; }
        public int StationId { get; set; }
        public int IsSendVerifyCode { get; set; }
        public int LastDailyMail { get; set; }
    }
}
