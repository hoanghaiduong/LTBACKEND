using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LTBACKEND.Entities;
using LTBACKEND.Services;
using LTBACKEND.Utils;
using Microsoft.AspNetCore.Mvc;

namespace LTBACKEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomTypeController : ControllerBase
    {
        private readonly RoomTypeService _roomTypeService;
        private readonly ILogger<RoomTypeController> _logger;

        public RoomTypeController(RoomTypeService roomTypeService, ILogger<RoomTypeController> logger)
        {
            _roomTypeService = roomTypeService;
            _logger = logger;
        }

        // ✅ Tạo RoomType (201 Created)
        [HttpPost]
        public async Task<IResult> CreateRoomType([FromBody] RoomType roomType)
        {
            try
            {
                var id = await _roomTypeService.CreateRoomType(roomType);
                if (id == 0)
                {
                    return Results.Problem("Failed to create room type.", statusCode: 500);
                }
                return Results.Created($"/api/roomtype/{id}", roomType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating RoomType");
                return Results.Problem("An unexpected error occurred.", statusCode: 500);
            }
        }

        // ✅ Lấy RoomType theo ID
        [HttpGet("{id:int}")]
        public async Task<IResult> GetRoomTypeById(int id)
        {
            var roomType = await _roomTypeService.GetRoomTypeById(id);
            return roomType == null
                ? Results.NotFound(new { message = "RoomType not found" })
                : Results.Ok(roomType);
        }

        // ✅ Lấy danh sách RoomType (Có phân trang)
        [HttpGet]
        public async Task<IResult> GetAllRoomTypes([FromQuery] PaginationModel paginationModel)
        {
            var result = await _roomTypeService.GetAllRoomTypes(paginationModel);
            return Results.Ok(result);
        }

        // ✅ Cập nhật RoomType (204 No Content)
        [HttpPut("{id:int}")]
        public async Task<IResult> UpdateRoomType(int id, [FromBody] RoomType roomType)
        {
            roomType.Id = id;
            var success = await _roomTypeService.UpdateRoomType(roomType);
            return success
                ? Results.NoContent()
                : Results.NotFound(new { message = "RoomType not found" });
        }

        // ✅ Xóa RoomType (204 No Content)
        [HttpDelete("{id:int}")]
        public async Task<IResult> DeleteRoomType(int id)
        {
            var success = await _roomTypeService.DeleteRoomType(id);
            return success
                ? Results.NoContent()
                : Results.NotFound(new { message = "RoomType not found" });
        }
    }
}
