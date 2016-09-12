﻿using System;

namespace RMS.Core
{
    public class LeaveRequest : BaseEntity
    {
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public DateTime ActualStartDateTime { get; set; }
        public DateTime ActualEndDateTime { get; set; }

        public DateTime ZktStartDateTime { get; set; }
        public DateTime ZktEndDateTime { get; set; }

        public long AmountRequested { get; set; }
        public long? EmployeeId { get; set; }
        public long LeaveTypeId { get; set; }
        public string Notes { get; set; }
        public bool IsApproved { get; set; }
        public bool IsTaken { get; set; }
        public int Status { get; set; }
    }
}
