
Alter Procedure spGetFileNameAndPath @FileServerId bigint=-1,@FileId bigint=-1
AS
BEGIN
SELECT [FileName],FilePath
FROM TblFileServer
WHERE (@FileServerId<=0 or FileServerId= @FileServerId)
	AND (@FileId<=0 or FileId=@FileId)
END

