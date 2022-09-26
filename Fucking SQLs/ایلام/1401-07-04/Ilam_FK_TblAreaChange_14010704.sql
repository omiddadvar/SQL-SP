----------------------------------------
ErrorCode=281   Date=1401/07/04 14:47:29   EventId=-100   Server=Local   TableName=TblRequestAreaChange   Action=2
ErrorDesc=The INSERT statement conflicted with the FOREIGN KEY constraint "FK_TblRequestAreaChange_TblRequest". The conflict occurred in database "CcRequesterDepartment", table "dbo.TblRequest", column 'RequestId'
SQL=if not exists(select * from TblRequestAreaChange Where RequestAreaChangeId=19909548) 
insert into TblRequestAreaChange (RequestAreaChangeId,RequestId,AreaUserId,FromAreaId,ToAreaId,RequestEndJobStateId,ChangeDT,ChangeDatePersian,ChangeTime) values (19909548,100000000836256,990777803,12,10,4,'2022/09/23 02:40:39',N'1401/07/01',N'02:40') else update TblRequestAreaChange set RequestId=100000000836256,AreaUserId=990777803,FromAreaId=12,ToAreaId=10,RequestEndJobStateId=4,ChangeDT='2022/09/23 02:40:39',ChangeDatePersian=N'1401/07/01',ChangeTime=N'02:40' where RequestAreaChangeId=19909548



Select top 10 * From TblRequestAreaChange

Select * From Tbl_AreaInfo Where AreaId in (1,10)

select * from TblRequest where RequestId = 100000000836256

/*-----Solution--------*/
1. Delete TblRequestAreaChange record left by mistake.
--On 121
Delete from TblRequestAreaChange Where RequestAreaChangeId=19909548

2. FK_TblRequestAreaChange_TblRequest Enforce was "No" !!!
Should be "Yes" and "Cascade" on delete.