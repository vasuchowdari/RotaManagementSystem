using System;
using System.Collections.Generic;
using AutoMapper;
using RMS.AppServiceLayer.AuditLogs.Dto;
using RMS.AppServiceLayer.AuditLogs.Interfaces;
using RMS.Core;
using RMS.Infrastructure.EF;

namespace RMS.AppServiceLayer.AuditLogs.Services
{
    public class AuditLogAppService : IAuditLogAppService
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        
        private long _userId;
        private DateTime _entryDateTime;
        private string _actionType;
        private string _tableName;
        private long _recordId;


        public AuditLogAppService(AuditLogDto auditLogDto)
        {
            _userId = auditLogDto.UserId;
            _entryDateTime = auditLogDto.EntryDateTime;
            _actionType = auditLogDto.ActionType;
            _tableName = auditLogDto.TableName;
            _recordId = auditLogDto.RecordId;
        }

        // Service Methods
        public void Audit(string actionType, string tableName, long userId, long recordId)
        {
            var auditLogDto = new AuditLogDto
            {
                Id = Guid.NewGuid(),
                ActionType = actionType,
                EntryDateTime = DateTime.Now,
                TableName = tableName,
                UserId = userId,
                RecordId = recordId
            };

            Create(auditLogDto);
        }

        // Repo Methods
        public ICollection<AuditLogDto> GetAllAuditLogs()
        {
            throw new NotImplementedException();
        }


        // CRUD
        public void Create(AuditLogDto auditLogDto)
        {
            var auditLog = Mapper.Map<AuditLog>(auditLogDto);

            _unitOfWork.AuditLogRepository.Create(auditLog);
            _unitOfWork.Save();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
