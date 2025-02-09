--
GO
CREATE PROCEDURE GetPaymentById
    @PaymentId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Payments WHERE Id = @PaymentId)
    BEGIN
        RAISERROR ('Error: Payment with ID %d not found.', 16, 1, @PaymentId);
        RETURN;
    END;

    SELECT 
        p.Id, 
        p.BookingId, 
        b.UserId,
        u.FirstName + ' ' + u.LastName AS CustomerName,
        p.Amount, 
        p.PaymentDate, 
        p.PaymentMethod, 
        p.CreatedAt, 
        p.UpdatedAt
    FROM Payments p
    INNER JOIN Bookings b ON p.BookingId = b.Id
    INNER JOIN Users u ON b.UserId = u.Id
    WHERE p.Id = @PaymentId;
END;

--✅ 2. Lấy danh sách tất cả Payments (Phân trang)
GO
CREATE PROCEDURE GetAllPayments
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT 
        p.Id, 
        p.BookingId, 
        b.UserId,
        u.FirstName + ' ' + u.LastName AS CustomerName,
        p.Amount, 
        p.PaymentDate, 
        p.PaymentMethod, 
        p.CreatedAt
    FROM Payments p
    INNER JOIN Bookings b ON p.BookingId = b.Id
    INNER JOIN Users u ON b.UserId = u.Id
    ORDER BY p.CreatedAt DESC
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END;
