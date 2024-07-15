﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Response.Participant
{
    public class GetSingleParticipantResponse
    {
        public int id { get; set; }
        public int eventId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public DateTime registeredDate { get; set; }
    }
}