using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LTBACKEND.Entities;
using LTBACKEND.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LTBACKEND.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomTypeController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly SQLHelper _sqlHelper;

        public RoomTypeController(SQLHelper sqlHelper, ApplicationDbContext dbContext)
        {
            _sqlHelper = sqlHelper;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IResult> GetRoomTypes()
        {
            try
            {

                var roomTypes = await _sqlHelper.ExecQueryAsync<RoomType>("SELECT * FROM RoomTypes");

                return Results.Ok(new
                {
                    message = "Success",
                    data = roomTypes
                });

            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }

          [HttpGet("Test2")]
        public async Task<IResult> GetRoomTypes2()
        {
            try
            {

                var roomTypes = await _dbContext.RoomTypes.ToListAsync();

                return Results.Ok(new
                {
                    message = "Success",
                    data = roomTypes
                });

            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }
    }
}