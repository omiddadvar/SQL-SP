Create PROCEDURE [dbo].[spGetMPPosts_Multiple] @aAreaIds varchar(4000),
	@IsAll BIT,
	@aWhereClause VARCHAR(4000),
	@aWhereClauseOR VARCHAR(4000)
AS
DECLARE @lSQL NVARCHAR(4000)

SET @lSQL = 'SELECT * FROM Tbl_MPPost WHERE ( (' + Cast(@IsAll AS NVARCHAR) + '=1 or IsActive=1) AND MPPostId IN ( SELECT MPPostId FROM Tbl_MPPost WHERE ( (AreaId In (' + @aAreaIds + ') OR ' + +''''+@aAreaIds+''''+  ' = '''') OR (MPPostId IN (SELECT DISTINCT MPPostId FROM Tbl_MPFeeder WHERE AreaId In (' + @aAreaIds + ') and (' + Cast(@IsAll AS NVARCHAR) + '=1 or IsActive=1)) ) OR (MPPostId IN (SELECT DISTINCT Tbl_MPFeeder.MPPostId FROM Tbl_MPFeeder INNER JOIN Tbl_LPPost ON Tbl_MPFeeder.MPFeederId = Tbl_LPPost.MPFeederId WHERE Tbl_LPPost.AreaId In (' + @aAreaIds + ') and (' + Cast(@IsAll AS NVARCHAR) + '=1 or Tbl_LPPost.IsActive=1)) ) OR (MPPostId IN (SELECT Tbl_MPCommonPost.MPPostId FROM Tbl_MPCommonPost INNER JOIN Tbl_MPPost ON Tbl_MPCommonPost.MPPostId = Tbl_MPPost.MPPostId WHERE Tbl_MPCommonPost.AreaId In (' + @aAreaIds + ') and (' + Cast(@IsAll AS NVARCHAR) + 
	'=1 or Tbl_MPPost.IsActive=1)) ) OR (MPPostId IN (SELECT Tbl_MPFeeder.MPPostId FROM Tbl_MPCommonFeeder INNER JOIN Tbl_MPFeeder ON Tbl_MPCommonFeeder.MPFeederId = Tbl_MPFeeder.MPFeederId WHERE Tbl_MPCommonFeeder.AreaId In (' + @aAreaIds + ') and (' + Cast(@IsAll AS NVARCHAR) + '=1 or Tbl_MPFeeder.IsActive=1)) ) ))'

IF @aWhereClause <> ''
	SET @lSQL = @lSQL + ' AND ( ' + @aWhereClause + ' )'
SET @lSQL = @lSQL + ')'

IF @aWhereClauseOR <> ''
	SET @lSQL = @lSQL + ' OR ( ' + @aWhereClauseOR + ' )'
SET @lSQL = @lSQL + ' UNION SELECT * FROM Tbl_MPPost WHERE ( (' + Cast(@IsAll AS NVARCHAR) + '=1 or IsActive=1) AND (MPPostId IN (SELECT Tbl_MPFeeder.MPPostId FROM Tbl_MPFeeder WHERE ( MPFeederId IN (SELECT MPFeederId FROM Tbl_LPPost WHERE (LPPostId IN (SELECT LPPostId FROM Tbl_LPFeeder WHERE AreaId In (' + @aAreaIds + ') and (' + Cast(@IsAll AS NVARCHAR) + '=1 or IsActive=1)) ) OR (LPPostId IN (SELECT Tbl_LPFeeder.LPPostId FROM Tbl_LPCommonFeeder INNER JOIN Tbl_LPFeeder ON Tbl_LPCommonFeeder.LPFeederId = Tbl_LPFeeder.LPFeederId WHERE Tbl_LPCommonFeeder.AreaId In (' + @aAreaIds + ') and (' + Cast(@IsAll AS NVARCHAR) + '=1 or IsActive=1)) ) )) )) '

IF @aWhereClause <> ''
	SET @lSQL = @lSQL + ' AND ( ' + @aWhereClause + ' )'
SET @lSQL = @lSQL + ')'

IF @aWhereClauseOR <> ''
	SET @lSQL = @lSQL + ' OR ( ' + @aWhereClauseOR + ' )'
print @lSQL
EXEC (@lSQL)
