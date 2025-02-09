--1. Lấy thông tin chi tiết Role theo ID
GO
CREATE PROCEDURE GetRoleById
    @RoleId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Roles WHERE Id = @RoleId)
    BEGIN
        RAISERROR ('Error: Role with ID %d not found.', 16, 1, @RoleId);
        RETURN;
    END;

    SELECT Id, Name, Description, CreatedAt, UpdatedAt
    FROM Roles
    WHERE Id = @RoleId;
END;

--Lấy danh sách tất cả các Role với phân trang
GO
CREATE PROCEDURE GetAllRoles
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT Id, Name, Description, CreatedAt, UpdatedAt
    FROM Roles
    ORDER BY CreatedAt DESC
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END;



--3. Lấy danh sách người dùng theo Role
GO
CREATE PROCEDURE GetUsersByRole
    @RoleId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Roles WHERE Id = @RoleId)
    BEGIN
        RAISERROR ('Error: Role with ID %d not found.', 16, 1, @RoleId);
        RETURN;
    END;

    SELECT 
        u.Id AS UserId, 
        u.FirstName + ' ' + u.LastName AS FullName, 
        u.Email, 
        u.PhoneNumber, 
        u.Avatar, 
        u.IsDisabled, 
        u.CreatedAt, 
        u.UpdatedAt
    FROM Users u
    INNER JOIN UserRoles ur ON u.Id = ur.UserId
    INNER JOIN Roles r ON ur.RoleId = r.Id
    WHERE r.Id = @RoleId
    ORDER BY u.CreatedAt DESC;
END;


/*
GO
CREATE PROCEDURE GetRolePermissions
    @RoleId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Roles WHERE Id = @RoleId)
    BEGIN
        RAISERROR ('Error: Role with ID %d not found.', 16, 1, @RoleId);
        RETURN;
    END;

    SELECT 
        p.Id AS PermissionId, 
        p.Name AS PermissionName, 
        p.Description AS PermissionDescription
    FROM RolePermissions rp
    INNER JOIN Permissions p ON rp.PermissionId = p.Id
    WHERE rp.RoleId = @RoleId;
END;
*/