using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Dapper;
using LTBACKEND.Entities;
using LTBACKEND.Utils;
using Microsoft.AspNetCore.Mvc;

namespace LTBACKEND.Services
{
    public class RoomTypeService
    {
        private readonly DbConnection _connection;
        private readonly ApplicationDbContext _context;
        private readonly SQLHelper _sqlHelper;
        private readonly ILogger<RoomTypeService> _logger;

        public RoomTypeService(ApplicationDbContext context, SQLHelper sqlHelper, ILogger<RoomTypeService> logger, DbConnection connection)
        {
            _context = context;
            _sqlHelper = sqlHelper;
            _logger = logger;
            _connection = connection;
        }

        // ✅ Thêm RoomType (EF Core)
        public async Task<int> CreateRoomType(RoomType roomType)
        {
            try
            {
                _context.RoomTypes.Add(roomType);
                await _context.SaveChangesAsync();
                return roomType.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating RoomType");
                throw;
            }
        }

        // ✅ Cập nhật RoomType (EF Core)
        public async Task<bool> UpdateRoomType(RoomType roomType)
        {
            try
            {
                var existing = await _context.RoomTypes.FindAsync(roomType.Id);
                if (existing == null)
                    return false;

                existing.Name = roomType.Name;
                existing.Description = roomType.Description;
                existing.PricePerNight = roomType.PricePerNight;
                existing.Capacity = roomType.Capacity;

                _context.RoomTypes.Update(existing);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating RoomType");
                throw;
            }
        }

        // ✅ Xóa RoomType (EF Core)
        public async Task<bool> DeleteRoomType(int id)
        {
            try
            {
                var roomType = await _context.RoomTypes.FindAsync(id);
                if (roomType == null)
                    return false;

                _context.RoomTypes.Remove(roomType);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting RoomType");
                throw;
            }
        }

        // ✅ Lấy RoomType theo ID (Dapper - Stored Procedure)
        public async Task<RoomType?> GetRoomTypeById(int id)
        {
            var parameters = new { RoomTypeId = id };
            var result = await _sqlHelper.ExecProcedureAsync<RoomType>("GetRoomTypeById", parameters);
            return result.FirstOrDefault();
        }

        public async Task<PaginatedResult<RoomType>> GetAllRoomTypes(PaginationModel paginationModel)
        {
            var parameters = new
            {
                PageNumber = paginationModel.PageNumber,
                PageSize = paginationModel.PageSize,
                Search = paginationModel.Search
            };

            using var multi = await _sqlHelper.ExecProcedureMultipleAsync("GetAllRoomTypes", parameters);

            // ✅ 1️⃣ Đọc danh sách RoomTypes phân trang
            var roomTypes = (await multi.ReadAsync<RoomType>()).ToList();
            var roomTypeDict = roomTypes.ToDictionary(rt => rt.Id, rt => rt);

            // ✅ 2️⃣ Đọc danh sách Rooms và gán vào RoomType tương ứng
            var rooms = await multi.ReadAsync<Room>();
            foreach (var room in rooms)
            {
                if (roomTypeDict.TryGetValue(room.RoomTypeId, out var roomType))
                {
                    roomType.Rooms ??= new List<Room>();
                    roomType.Rooms.Add(room);
                }
            }

            // ✅ 3️⃣ Đọc tổng số bản ghi
            var totalCount = await multi.ReadFirstAsync<int>();

            return new PaginatedResult<RoomType>(roomTypes, totalCount, paginationModel.PageSize, paginationModel.PageNumber);
        }

    }
}