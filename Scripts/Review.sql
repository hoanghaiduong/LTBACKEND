--kiểm tra sự tồn tại của khách sạn, đồng thời đảm bảo thông tin được hiển thị đầy đủ và chính xác.
GO
CREATE PROCEDURE GetReviewsByHotel
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
        r.Id AS ReviewId, 
        u.FirstName + ' ' + u.LastName AS Reviewer, 
        u.Avatar AS ReviewerAvatar,
        h.Name AS Hotel, 
        r.Rating, 
        r.Comment, 
        r.CreatedAt
    FROM Reviews r
    INNER JOIN Bookings b ON r.BookingId = b.Id
    INNER JOIN Users u ON b.UserId = u.Id
    INNER JOIN Rooms ro ON b.RoomId = ro.Id
    INNER JOIN Hotels h ON ro.HotelID = h.Id
    WHERE h.Id = @HotelId
    ORDER BY r.CreatedAt DESC
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END;

--2. Lấy trung bình đánh giá của khách sạn
GO
CREATE PROCEDURE GetAverageRatingByHotel
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
        h.Id AS HotelId,
        h.Name AS HotelName,
        COUNT(r.Id) AS TotalReviews,
        ISNULL(AVG(r.Rating), 0) AS AverageRating
    FROM Hotels h
    LEFT JOIN Rooms ro ON h.Id = ro.HotelID
    LEFT JOIN Bookings b ON ro.Id = b.RoomId
    LEFT JOIN Reviews r ON b.Id = r.BookingId
    WHERE h.Id = @HotelId
    GROUP BY h.Id, h.Name;
END;


--3. Lấy chi tiết một review theo ID
GO
CREATE PROCEDURE GetReviewDetails
    @ReviewId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Reviews WHERE Id = @ReviewId)
    BEGIN
        RAISERROR ('Error: Review with ID %d not found.', 16, 1, @ReviewId);
        RETURN;
    END;

    SELECT 
        r.Id AS ReviewId, 
        u.FirstName + ' ' + u.LastName AS Reviewer, 
        u.Avatar AS ReviewerAvatar,
        h.Name AS Hotel, 
        rt.Name AS RoomType,
        r.Rating, 
        r.Comment, 
        r.CreatedAt
    FROM Reviews r
    INNER JOIN Bookings b ON r.BookingId = b.Id
    INNER JOIN Users u ON b.UserId = u.Id
    INNER JOIN Rooms ro ON b.RoomId = ro.Id
    INNER JOIN Hotels h ON ro.HotelID = h.Id
    INNER JOIN RoomTypes rt ON ro.RoomTypeId = rt.Id
    WHERE r.Id = @ReviewId;
END;

--4. Lấy danh sách đánh giá của một người dùng tại một khách sạn
GO
CREATE PROCEDURE GetUserReviewsByHotel
    @UserId INT,
    @HotelId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = @UserId)
    BEGIN
        RAISERROR ('Error: User with ID %d not found.', 16, 1, @UserId);
        RETURN;
    END;

    IF NOT EXISTS (SELECT 1 FROM Hotels WHERE Id = @HotelId)
    BEGIN
        RAISERROR ('Error: Hotel with ID %d not found.', 16, 1, @HotelId);
        RETURN;
    END;

    SELECT 
        r.Id AS ReviewId, 
        r.Rating, 
        r.Comment, 
        r.CreatedAt
    FROM Reviews r
    INNER JOIN Bookings b ON r.BookingId = b.Id
    INNER JOIN Rooms ro ON b.RoomId = ro.Id
    INNER JOIN Hotels h ON ro.HotelID = h.Id
    WHERE b.UserId = @UserId AND h.Id = @HotelId
    ORDER BY r.CreatedAt DESC;
END;
