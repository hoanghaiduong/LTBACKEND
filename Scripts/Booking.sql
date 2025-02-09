--✅ 1. Lấy thông tin Booking theo ID
GO
CREATE PROCEDURE GetBookingById
    @BookingId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Bookings WHERE Id = @BookingId)
    BEGIN
        RAISERROR ('Error: Booking with ID %d not found.', 16, 1, @BookingId);
        RETURN;
    END;

    SELECT 
        b.Id, 
        u.FirstName + ' ' + u.LastName AS UserName,
        h.Name AS HotelName,
        r.RoomNumber,
        b.CheckInDate, 
        b.CheckOutDate, 
        b.Status, 
        b.TotalPrice, 
        b.CreatedAt, 
        b.UpdatedAt
    FROM Bookings b
    INNER JOIN Users u ON b.UserId = u.Id
    INNER JOIN Rooms r ON b.RoomId = r.Id
    INNER JOIN Hotels h ON r.HotelID = h.Id
    WHERE b.Id = @BookingId;
END;


--✅ 2. Lấy danh sách Booking của một User cụ thể
GO
CREATE PROCEDURE GetBookingsByUser
    @UserId INT,
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = @UserId)
    BEGIN
        RAISERROR ('Error: User with ID %d not found.', 16, 1, @UserId);
        RETURN;
    END;

    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT 
        b.Id, 
        h.Name AS HotelName,
        r.RoomNumber,
        b.CheckInDate, 
        b.CheckOutDate, 
        b.Status, 
        b.TotalPrice, 
        b.CreatedAt
    FROM Bookings b
    INNER JOIN Rooms r ON b.RoomId = r.Id
    INNER JOIN Hotels h ON r.HotelID = h.Id
    WHERE b.UserId = @UserId
    ORDER BY b.CreatedAt DESC
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END;


--✅ 3. Lấy danh sách Booking của một Hotel cụ thể
GO
CREATE PROCEDURE GetBookingsByHotel
    @HotelId INT,
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Hotels WHERE Id = @HotelId)
    BEGIN
        RAISERROR ('Error: Hotel with ID %d not found.', 16, 1, @HotelId);
        RETURN;
    END;

    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT 
        b.Id, 
        u.FirstName + ' ' + u.LastName AS UserName, 
        r.RoomNumber, 
        b.CheckInDate, 
        b.CheckOutDate, 
        b.Status, 
        b.TotalPrice, 
        b.CreatedAt
    FROM Bookings b
    INNER JOIN Users u ON b.UserId = u.Id
    INNER JOIN Rooms r ON b.RoomId = r.Id
    WHERE r.HotelID = @HotelId
    ORDER BY b.CreatedAt DESC
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END;

