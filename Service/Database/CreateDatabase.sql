-- =============================================
-- 数据库：player
-- 表：records、table1
-- 一键创建库 + 建表
-- =============================================
-- 强制删除旧的 player 库
DROP DATABASE IF EXISTS player;
GO

-- 如果不存在 player 数据库，则创建
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'player')
CREATE DATABASE player;
GO

-- 使用该数据库
USE player;
GO

-- 创建 records 表（带默认时间）
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'records')
CREATE TABLE [dbo].[records] (
    [sqlname] VARCHAR (50) NULL,
    [before]  INT          NULL,
    [change]  INT          NULL,
    [present] INT          NULL,
    [sqltime] DATETIME     CONSTRAINT [DF_records_sqltime] DEFAULT (getdate()) NULL
);
GO

-- 创建 table1 表（用户信息表）
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'table1')
CREATE TABLE [dbo].[table1] (
    [sqlname] VARCHAR (50) NULL,
    [sqlpw]   VARCHAR (50) NULL,
    [grade]   INT          NULL
);
GO

-- 执行完成！
PRINT 'success';

SELECT 
    name AS 逻辑文件名,
    physical_name AS 真实物理路径
FROM 
    sys.master_files
WHERE 
    database_id = DB_ID('player');