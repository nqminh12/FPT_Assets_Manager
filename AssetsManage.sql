-- 1. Tạo cơ sở dữ liệu AssetManager
--CREATE DATABASE AssetManager;
GO

USE AssetManager;
GO

-- 2. Bảng Projects: Lưu thông tin các dự án Unity
CREATE TABLE Projects (
    ProjectID INT PRIMARY KEY IDENTITY(1,1),
    ProjectName NVARCHAR(100) NOT NULL,
    Path NVARCHAR(255) NOT NULL,
    CreatedDate DATETIME DEFAULT GETDATE()
);

-- 3. Bảng Folders: Lưu thông tin các thư mục trong dự án, mỗi thư mục chứa nhiều tài nguyên
CREATE TABLE Folders (
    FolderID INT PRIMARY KEY IDENTITY(1,1),
    FolderName NVARCHAR(100) NOT NULL,
    CreatedDate DATETIME DEFAULT GETDATE(),
    AssetType NVARCHAR(50) -- Ví dụ: "Images", "Audio", "3D Models"
);

-- 4. Bảng ProjectFolders: Liên kết nhiều-nhiều giữa Projects và Folders
CREATE TABLE ProjectFolders (
    ProjectID INT FOREIGN KEY REFERENCES Projects(ProjectID) ON DELETE CASCADE,
    FolderID INT FOREIGN KEY REFERENCES Folders(FolderID) ON DELETE CASCADE,
    PRIMARY KEY (ProjectID, FolderID)
);

-- 5. Bảng Categories: Phân loại các loại tài nguyên (ảnh, âm thanh, 3D,...)
CREATE TABLE Categories (
    CategoryID INT PRIMARY KEY IDENTITY(1,1),
    CategoryName NVARCHAR(50) NOT NULL
);

-- 6. Bảng CloudStorage: Lưu thông tin các tài khoản đám mây
CREATE TABLE CloudStorage (
    CloudID INT PRIMARY KEY IDENTITY(1,1),
    Provider NVARCHAR(50) NOT NULL,
    AccessToken NVARCHAR(255) NOT NULL,
    RefreshToken NVARCHAR(255),
    ExpirationDate DATETIME
);

-- 7. Bảng AssetType: Phân loại các định dạng tài nguyên (ví dụ: .png, .mp3, .fbx)
CREATE TABLE AssetType (
    TypeID INT PRIMARY KEY IDENTITY(1,1),
    TypeDescription NVARCHAR(10) NOT NULL
);

-- 8. Bảng Assets: Lưu thông tin về các tài nguyên của Unity
CREATE TABLE Assets (
    AssetID INT PRIMARY KEY IDENTITY(1,1),
    AssetName NVARCHAR(100) NOT NULL,
    FilePath NVARCHAR(255) NOT NULL,
    SizeKB INT NOT NULL,
    TypeID INT FOREIGN KEY REFERENCES AssetType(TypeID),
    CategoryID INT FOREIGN KEY REFERENCES Categories(CategoryID) NOT NULL,
    CloudID INT FOREIGN KEY REFERENCES CloudStorage(CloudID) ON DELETE SET NULL,
    FolderID INT FOREIGN KEY REFERENCES Folders(FolderID) ON DELETE CASCADE,
    ImportedDate DATETIME DEFAULT GETDATE()
);

-- 9. Bảng AssetLogs: Lưu lịch sử hoạt động trên các tài nguyên
CREATE TABLE AssetLogs (
    LogID INT PRIMARY KEY IDENTITY(1,1),
    AssetID INT FOREIGN KEY REFERENCES Assets(AssetID) ON DELETE CASCADE,
    Action NVARCHAR(50) NOT NULL,
    ActionDetails NVARCHAR(255),
    OldPath NVARCHAR(255),
    NewPath NVARCHAR(255),
    Timestamp DATETIME DEFAULT GETDATE()
);

-- 10. Bảng UserRoles: Quản lý vai trò người dùng
CREATE TABLE UserRoles (
    RoleID INT PRIMARY KEY IDENTITY(1,1),
    RoleName NVARCHAR(50) NOT NULL
);

-- 11. Bảng AssetPermissions: Quản lý quyền truy cập vào từng tài nguyên
CREATE TABLE AssetPermissions (
    PermissionID INT PRIMARY KEY IDENTITY(1,1),
    RoleID INT FOREIGN KEY REFERENCES UserRoles(RoleID),
    AssetID INT FOREIGN KEY REFERENCES Assets(AssetID),
    CanView BIT DEFAULT 1,
    CanEdit BIT DEFAULT 0,
    CanDelete BIT DEFAULT 0
);

-- 12. Trigger để ghi log tự động khi thêm mới tài nguyên vào bảng Assets
CREATE TRIGGER trg_AfterAssetInsert 
ON Assets
AFTER INSERT
AS
BEGIN
    INSERT INTO AssetLogs (AssetID, Action, NewPath, Timestamp)
    SELECT AssetID, 'Imported', FilePath, GETDATE()
    FROM inserted;
END;

-- 13. Trigger để ghi log tự động khi cập nhật đường dẫn tài nguyên trong bảng Assets
CREATE TRIGGER trg_AfterAssetUpdate
ON Assets
AFTER UPDATE
AS
BEGIN
    INSERT INTO AssetLogs (AssetID, Action, OldPath, NewPath, Timestamp)
    SELECT d.AssetID, 'Updated', d.FilePath, i.FilePath, GETDATE()
    FROM deleted d
    JOIN inserted i ON d.AssetID = i.AssetID
    WHERE d.FilePath <> i.FilePath;
END;

-- 14. Dữ liệu mẫu trong bảng AssetType
INSERT INTO AssetType (TypeDescription)
VALUES ('.png'), ('.mp3'), ('.fbx'), ('.cs');

-- 15. Dữ liệu mẫu trong bảng Projects
INSERT INTO Projects (ProjectName, Path) 
VALUES 
('RPG Adventure', 'C:\\UnityProjects\\RPG_Adventure\\Assets'),
('Sci-Fi Shooter', 'C:\\UnityProjects\\SciFi_Shooter\\Assets');

-- 16. Dữ liệu mẫu trong bảng Categories
INSERT INTO Categories (CategoryName) 
VALUES 
('Animations'), 
('Sprites'), 
('Level Designs/Scenes'), 
('Images'), 
('Audio'), 
('3D Models'), 
('Scripts');

-- 17. Dữ liệu mẫu trong bảng CloudStorage
INSERT INTO CloudStorage (Provider, AccessToken, RefreshToken, ExpirationDate)
VALUES 
('Google Drive', 'sampleAccessToken1', 'sampleRefreshToken1', DATEADD(DAY, 30, GETDATE())),
('OneDrive', 'sampleAccessToken2', 'sampleRefreshToken2', DATEADD(DAY, 30, GETDATE()));

-- 18. Dữ liệu mẫu trong bảng Folders
INSERT INTO Folders (FolderName, AssetType)
VALUES 
('Character Assets', 'Images'),
('Sound Effects', 'Audio'),
('3D Models', '3D Models'),
('Scripts', 'Scripts');

-- 19. Dữ liệu mẫu trong bảng ProjectFolders
INSERT INTO ProjectFolders (ProjectID, FolderID)
VALUES 
(1, 1), -- RPG Adventure sử dụng Character Assets
(1, 2), -- RPG Adventure sử dụng Sound Effects
(2, 3), -- Sci-Fi Shooter sử dụng 3D Models
(1, 4); -- RPG Adventure sử dụng Scripts

-- 20. Dữ liệu mẫu trong bảng Assets
INSERT INTO Assets (AssetName, FilePath, SizeKB, TypeID, CategoryID, CloudID, FolderID) 
VALUES 
('Character.png', 'C:\\UnityProjects\\RPG_Adventure\\Assets\\Character.png', 512, 1, 1, 1, 1),
('BackgroundMusic.mp3', 'C:\\UnityProjects\\RPG_Adventure\\Assets\\BackgroundMusic.mp3', 2048, 2, 5, 2, 2),
('Spaceship.fbx', 'C:\\UnityProjects\\SciFi_Shooter\\Assets\\Spaceship.fbx', 10240, 3, 6, NULL, 3),
('PlayerController.cs', 'C:\\UnityProjects\\RPG_Adventure\\Assets\\PlayerController.cs', 32, 4, 7, NULL, 4);

-- 21. Dữ liệu mẫu trong bảng AssetLogs
INSERT INTO AssetLogs (AssetID, Action, ActionDetails, OldPath, NewPath, Timestamp) 
VALUES 
(1, 'Imported', NULL, NULL, 'C:\\UnityProjects\\RPG_Adventure\\Assets\\Character.png', GETDATE()),
(2, 'Imported', NULL, NULL, 'C:\\UnityProjects\\RPG_Adventure\\Assets\\BackgroundMusic.mp3', GETDATE()),
(3, 'Imported', NULL, NULL, 'C:\\UnityProjects\\SciFi_Shooter\\Assets\\Spaceship.fbx', GETDATE()),
(4, 'Imported', NULL, NULL, 'C:\\UnityProjects\\RPG_Adventure\\Assets\\PlayerController.cs', GETDATE());

-- 22. Thêm dữ liệu mẫu vào bảng UserRoles
INSERT INTO UserRoles (RoleName)
VALUES ('Admin'), ('Editor'), ('Viewer');

-- 23. Thêm dữ liệu mẫu vào bảng AssetPermissions
INSERT INTO AssetPermissions (RoleID, AssetID, CanView, CanEdit, CanDelete)
VALUES 
(1, 1, 1, 1, 1), -- Admin có toàn quyền trên Character.png
(2, 2, 1, 1, 0), -- Editor có thể xem và chỉnh sửa BackgroundMusic.mp3 nhưng không thể xóa
(3, 3, 1, 0, 0); -- Viewer chỉ có quyền xem Spaceship.fbx
