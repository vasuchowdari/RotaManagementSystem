using System;
using System.Collections.Generic;
using RMS.AppServiceLayer.ShiftTypes.Dto;

namespace RMS.AppServiceLayer.ShiftTypes.Interfaces
{
    public interface IShiftTypeAppService : IDisposable
    {
        // Service Methods


        // Repo Methods
        ShiftTypeDto GetById(long id);
        ICollection<ShiftTypeDto> GetAll();

        // CRUD
        void Create(ShiftTypeDto shiftTypeDto, long userId);
        void Update(ShiftTypeDto shiftTypeDto, long userId);
        void Delete(long id, long userId);
    }
}
