--✅ 1. Lấy thông tin RoomType theo ID
GO
CREATE PROCEDURE GetRoomTypeById
    @RoomTypeId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM RoomTypes WHERE Id = @RoomTypeId)
    BEGIN
        RAISERROR ('Error: RoomType with ID %d not found.', 16, 1, @RoomTypeId);
        RETURN;
    END;

    SELECT 
        Id, 
        Name, 
        Description, 
        Capacity, 
        PricePerNight, 
        CreatedAt, 
        UpdatedAt
    FROM RoomTypes
    WHERE Id = @RoomTypeId;
END;

--✅ 2. Lấy danh sách tất cả RoomTypes (Phân trang)
GO
CREATE PROCEDURE GetAllRoomTypes
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT 
        Id, 
        Name, 
        Description, 
        Capacity, 
        PricePerNight, 
        CreatedAt
    FROM RoomTypes
    ORDER BY CreatedAt DESC
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END;


--✅ 3. Thêm mới RoomType
GO
CREATE PROCEDURE CreateRoomType
    @Name NVARCHAR(255),
    @Description NVARCHAR(MAX),
    @Capacity INT,
    @PricePerNight DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM RoomTypes WHERE Name = @Name)
    BEGIN
        RAISERROR ('Error: RoomType "%s" already exists.', 16, 1, @Name);
        RETURN;
    END;

    INSERT INTO RoomTypes (Name, Description, Capacity, PricePerNight, CreatedAt, UpdatedAt)
    VALUES (@Name, @Description, @Capacity, @PricePerNight, GETDATE(), GETDATE());

    SELECT SCOPE_IDENTITY() AS RoomTypeId;
END;

--✅ 4. Cập nhật RoomType
GO
CREATE PROCEDURE UpdateRoomType
    @RoomTypeId INT,
    @Name NVARCHAR(255),
    @Description NVARCHAR(MAX),
    @Capacity INT,
    @PricePerNight DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM RoomTypes WHERE Id = @RoomTypeId)
    BEGIN
        RAISERROR ('Error: RoomType with ID %d not found.', 16, 1, @RoomTypeId);
        RETURN;
    END;

    UPDATE RoomTypes
    SET 
        Name = @Name,
        Description = @Description,
        Capacity = @Capacity,
        PricePerNight = @PricePerNight,
        UpdatedAt = GETDATE()
    WHERE Id = @RoomTypeId;
END;

-- 5. Xóa RoomType
GO
CREATE PROCEDURE DeleteRoomType
    @RoomTypeId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM RoomTypes WHERE Id = @RoomTypeId)
    BEGIN
        RAISERROR ('Error: RoomType with ID %d not found.', 16, 1, @RoomTypeId);
        RETURN;
    END;

    DELETE FROM RoomTypes WHERE Id = @RoomTypeId;
END;
