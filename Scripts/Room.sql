--✅ 1. Lấy thông tin Room theo ID
GO
CREATE PROCEDURE GetRoomById
    @RoomId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Rooms WHERE Id = @RoomId)
    BEGIN
        RAISERROR ('Error: Room with ID %d not found.', 16, 1, @RoomId);
        RETURN;
    END;

    SELECT 
        r.Id, 
        r.RoomNumber, 
        r.HotelId, 
        h.Name AS HotelName,
        r.RoomTypeId, 
		r.Price,
		r.Status,
		r.Thumbnail,
		r.Images,
        rt.Name AS RoomTypeName,
        r.CreatedAt, 
        r.UpdatedAt
    FROM Rooms r
    INNER JOIN Hotels h ON r.HotelId = h.Id
    INNER JOIN RoomTypes rt ON r.RoomTypeId = rt.Id
    WHERE r.Id = @RoomId;
END;


--✅ 2. Lấy danh sách tất cả Rooms (Phân trang)
GO
CREATE PROCEDURE GetAllRooms
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT 
        r.Id, 
        r.RoomNumber, 
        r.HotelID, 
        h.Name AS HotelName,
        r.RoomTypeId, 
        rt.Name AS RoomTypeName,
        r.Thumbnail,
        r.Images, 
        r.Price, 
        r.Status, 
        r.CreatedAt
    FROM Rooms r
    INNER JOIN Hotels h ON r.HotelID = h.Id
    INNER JOIN RoomTypes rt ON r.RoomTypeId = rt.Id
    ORDER BY r.CreatedAt DESC
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END;
