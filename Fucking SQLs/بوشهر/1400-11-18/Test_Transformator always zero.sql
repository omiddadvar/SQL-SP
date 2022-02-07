
select MPRequestId,* from TblRequest where RequestNumber = 40099263107

select MPFeederId,MPPostId,* from TblMPRequest where MPRequestId = 9900000000215466

select * from Tbl_MPFeeder where MPFeederId = 8077

select * from Tbl_MPPost where MPPostId = 1873

select IsActive,* from Tbl_LPPost where MPFeederId = 8077

select * from Tbl_Area where AreaId = 4

SELECT dbo.GetLPTransCount(8077 , 0)
