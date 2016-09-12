using System;
using System.Collections.Generic;
using RMS.AppServiceLayer.TimeFormAdjustments.Dto;

namespace RMS.AppServiceLayer.TimeFormAdjustments.Interfaces
{
    public interface ITimeAdjustmentFormAppService : IDisposable
    {
        // Service Methods
        ICollection<TimeAdjustmentFormDto> GetForIds(List<long> staffIds);
        ICollection<TimeAdjustmentFormDto> GetAllUnapproved(); // Repo Methods
        TimeAdjustmentFormDto GetById(long id);
        ICollection<TimeAdjustmentFormDto> GetAll();
        ICollection<TimeAdjustmentFormDto> GetForEmployee(long id);
        ICollection<TimeAdjustmentFormDto> GetForUserId(long id);
        ICollection<TimeAdjustmentFormDto> GetForSite(long id);
        ICollection<TimeAdjustmentFormDto> GetForSubSite(long id); 


        // CRUD
        void Create(TimeAdjustmentFormDto timeAdjustmentFormDto, long userId);
        void Update(TimeAdjustmentFormDto timeAdjustmentFormDto, long userId);
    }
}
