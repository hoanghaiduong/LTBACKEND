--✅ 1. Lấy thông tin Hotel theo ID
GO
CREATE PROCEDURE GetHotelById
    @HotelId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Hotels WHERE Id = @HotelId)
    BEGIN
        RAISERROR ('Error: Hotel with ID %d not found.', 16, 1, @HotelId);
        RETURN;
    END;

    SELECT 
        h.Id, 
        h.Name, 
        h.Address, 
        h.Phone, 
        h.Email, 
        h.Thumbnail, 
        h.Stars, 
        h.CheckinTime, 
        h.CheckoutTime, 
        h.CreatedAt, 
        h.UpdatedAt
    FROM Hotels h
    WHERE h.Id = @HotelId;
END;

--✅ 2. Lấy danh sách tất cả Hotels (Phân trang)
GO
CREATE PROCEDURE GetAllHotels
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT 
        h.Id, 
        h.Name, 
        h.Address, 
        h.Phone, 
        h.Email, 
        h.Thumbnail, 
        h.Stars, 
        h.CheckinTime, 
        h.CheckoutTime, 
        h.CreatedAt
    FROM Hotels h
    ORDER BY h.CreatedAt DESC
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END;

--✅ 3. Lấy danh sách Rooms theo Hotel
GO
CREATE PROCEDURE GetRoomsByHotel
    @HotelId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Hotels WHERE Id = @HotelId)
    BEGIN
        RAISERROR ('Error: Hotel with ID %d not found.', 16, 1, @HotelId);
        RETURN;
    END;

    SELECT 
        r.Id, 
        r.RoomNumber, 
        r.Price, 
        r.Status, 
        r.Thumbnail, 
        r.RoomTypeId, 
        rt.Name AS RoomType
    FROM Rooms r
    INNER JOIN RoomTypes rt ON r.RoomTypeId = rt.Id
    WHERE r.HotelID = @HotelId;
END;

--✅ 4. Lấy danh sách Users làm việc tại Hotel
GO
CREATE PROCEDURE GetUsersByHotel
    @HotelId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Hotels WHERE Id = @HotelId)
    BEGIN
        RAISERROR ('Error: Hotel with ID %d not found.', 16, 1, @HotelId);
        RETURN;
    END;

    SELECT 
        u.Id, 
        u.FirstName, 
        u.LastName, 
        u.Email, 
        u.PhoneNumber, 
        u.IsDisabled, 
        u.CreatedAt
    FROM Users u
    INNER JOIN UserHotels uh ON u.Id = uh.UserId
    WHERE uh.HotelId = @HotelId;
END;
