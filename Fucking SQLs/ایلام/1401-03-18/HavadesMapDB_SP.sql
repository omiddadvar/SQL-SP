USE [HavadesMapDB]
GO

/****** Object:  StoredProcedure [dbo].[spAddTileContent]    Script Date: 06/08/2022 17:01:15 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spAddTileContent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spAddTileContent]
GO

/****** Object:  StoredProcedure [dbo].[spGetTileContent]    Script Date: 06/08/2022 17:01:15 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spGetTileContent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spGetTileContent]
GO

/****** Object:  StoredProcedure [dbo].[spGetTileStatistics]    Script Date: 06/08/2022 17:01:15 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spGetTileStatistics]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spGetTileStatistics]
GO

USE [HavadesMapDB]
GO

/****** Object:  StoredProcedure [dbo].[spAddTileContent]    Script Date: 06/08/2022 17:01:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spAddTileContent](@URL as nvarchar(500),@ContentType as nvarchar(50),@ContentEncoding as nvarchar(50),@Content as varbinary(max))
AS
BEGIN
	if LEN(@Content) <=0 
		return
	if @ContentType like '%text/html%'
		return
	
	set @URL=LEFT(@URL,200)
	declare @TileId as int
	declare @LastUpdateDT as datetime
	select @TileId= TileId from dbo.[TblTile] where [URL]=@URL 	
	if not @TileId is null
	begin
		update dbo.[TblTile]
		set 
		ContentType=@ContentType,
		ContentEncoding=@ContentEncoding,
		RequestAfterUpdate=0,
		Content=@Content,
		ContentLen=LEN(@Content),
		LastUpdateDT=GETDATE()
		where  TileId =@TileId
	end
	else
	begin
		INSERT INTO dbo.[TblTile]
           ([URL]
           ,[LastUpdateDT]
           ,[RequestCount]
           ,[RequestAfterUpdate]
           ,[ContentLen]
           ,[ContentType]
           ,[ContentEncoding]
           ,[Content])
     VALUES
           (@URL
           ,GETDATE()
           ,0
           ,0
           ,LEN(@Content)
           ,@ContentType
           ,@ContentEncoding
           ,@Content
           )
	end		
	/*delete from dbo.[TblTile] where ContentType like '%text/html%'*/
END

GO

/****** Object:  StoredProcedure [dbo].[spGetTileContent]    Script Date: 06/08/2022 17:01:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spGetTileContent](@URL as nvarchar(500))
AS
BEGIN
	declare @dt as datetime=dateadd(day,-2000,getdate())
	set @URL=LEFT(@URL,200)
	declare @TileId as int
	
	select @TileId= TileId from dbo.[TblTile] where [URL]=@URL and LastUpdateDT >@dt
	if not @TileId is null
	begin
		update dbo.[TblTile]
		set RequestCount=RequestCount+1,
		RequestAfterUpdate=RequestAfterUpdate+1
		where  TileId =@TileId
	end
	set @TileId=ISNULL(@TileId,-1)
	select [Content],[ContentLen],ContentType,ContentEncoding from dbo.[TblTile] where TileId =@TileId
	and ContentLen > 0
	and not ContentType like '%text/html%'
	
END

GO

/****** Object:  StoredProcedure [dbo].[spGetTileStatistics]    Script Date: 06/08/2022 17:01:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


Create PROCEDURE [dbo].[spGetTileStatistics]
AS
begin
	SELECT COUNT(*) as TileCount,SUM(RequestCount) as RequestCount,avg(RequestCount) as AvgRequestCount,cast(round(SUM(cast(ContentLen as float))/(1024*1024),1) as nvarchar(100))+' MB' as  DBSize,cast(round(sum(cast(ContentLen*RequestCount as float)) /(1024*1024),1) as nvarchar(100))+' MB' as UsedCache
	FROM dbo.[TblTile]
end	
GO


