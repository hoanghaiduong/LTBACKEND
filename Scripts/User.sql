GO
CREATE PROCEDURE GetUserById
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = @UserId)
    BEGIN
        RAISERROR ('Error: User with ID %d not found.', 16, 1, @UserId);
        RETURN;
    END;

    SELECT 
        Id, Email, PhoneNumber, FirstName, LastName, 
        EmailVerified, Avatar, IsDisabled, LastLogin, CreatedAt, UpdatedAt
    FROM Users
    WHERE Id = @UserId;
END;


GO
CREATE PROCEDURE GetUserRoles
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = @UserId)
    BEGIN
        RAISERROR ('Error: User with ID %d not found.', 16, 1, @UserId);
        RETURN;
    END;

    SELECT 
        ur.UserId, r.Id AS RoleId, r.Name AS RoleName, r.Description
    FROM UserRoles ur
    INNER JOIN Roles r ON ur.RoleId = r.Id
    WHERE ur.UserId = @UserId;
END;


GO
CREATE PROCEDURE GetUserBookings
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = @UserId)
    BEGIN
        RAISERROR ('Error: User with ID %d not found.', 16, 1, @UserId);
        RETURN;
    END;

    SELECT 
        b.Id AS BookingId, b.RoomId, r.Name AS RoomType, 
        b.CheckInDate, b.CheckOutDate, b.TotalPrice, b.Status, b.CreatedAt, b.UpdatedAt
    FROM Bookings b
    INNER JOIN Rooms rm ON b.RoomId = rm.Id
    INNER JOIN RoomTypes r ON rm.RoomTypeId = r.Id
    WHERE b.UserId = @UserId;
END;


GO
CREATE PROCEDURE GetUserReviews
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = @UserId)
    BEGIN
        RAISERROR ('Error: User with ID %d not found.', 16, 1, @UserId);
        RETURN;
    END;

    SELECT 
        r.Id AS ReviewId, r.BookingId, rt.Name AS RoomType, 
        r.Rating, r.Comment, r.CreatedAt, r.UpdatedAt
    FROM Reviews r
    INNER JOIN Bookings b ON r.BookingId = b.Id
    INNER JOIN Rooms rm ON b.RoomId = rm.Id
    INNER JOIN RoomTypes rt ON rm.RoomTypeId = rt.Id
    WHERE b.UserId = @UserId;
END;
