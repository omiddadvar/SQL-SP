use CcRequesterSetad
go
select top(2) * from TblRequestPart
go
select top(2) * from TblMPRequestPart
go
select top(2) * from TblLPRequestPart

select top(2) * from TblLightRequestPart




 SELECT DISTINCT "TblRequest"."SubscriberName", "TblRequest"."Telephone", "Tbl_CallReason"."CallReason", "TblRequest"."ConnectDatePersian", "TblRequest"."ConnectTime", "TblRequest"."EndJobStateId", "TblBazdid"."UndoneReasonId", "Tbl_UndoneReason"."UndoneReason", "Tbl_Master"."Name", "TblBazdid"."BazdidId", "TblBazdid"."EzamDatePersian", "TblBazdid"."EzamTime", "Tbl_MPPost"."MPPostName", "Tbl_MPFeeder"."MPFeederName", "Tbl_LPPost"."LPPostName", "Tbl_LPFeeder"."LPFeederName", "TblBazdid"."Comments", "TblMPRequest"."Comments", "TblLPRequest"."Comments", "TblRequest"."RequestNumber", "ViewAllRequest"."MPRequestId", "TblRequest"."DisconnectDatePersian", "TblRequest"."DisconnectTime", "TblRequest"."IsTamir", "TblRequest"."Address", "Tbl_LPPost"."LPPostCode", "Tbl_Subscriber"."Code", "TblRequestInfo"."SubscriberCode", "TblRequest"."Comments", "TblRequest"."ExtraComments", "TblRequest"."LPRequestId"
 FROM   (((((((((((("CcRequesterSetad"."dbo"."ViewAllRequest" "ViewAllRequest" 
 RIGHT OUTER JOIN "CcRequesterSetad"."dbo"."TblRequest" "TblRequest" ON "ViewAllRequest"."RequestId"="TblRequest"."RequestId")
  LEFT OUTER JOIN "CcRequesterSetad"."dbo"."Tbl_LPPost" "Tbl_LPPost" ON "ViewAllRequest"."LPPostId"="Tbl_LPPost"."LPPostId") LEFT OUTER JOIN "CcRequesterSetad"."dbo"."Tbl_LPFeeder" "Tbl_LPFeeder" ON "ViewAllRequest"."LPFeederId"="Tbl_LPFeeder"."LPFeederId") LEFT OUTER JOIN "CcRequesterSetad"."dbo"."Tbl_MPPost" "Tbl_MPPost" ON "ViewAllRequest"."MPPostId"="Tbl_MPPost"."MPPostId") LEFT OUTER JOIN "CcRequesterSetad"."dbo"."Tbl_MPFeeder" "Tbl_MPFeeder" ON "ViewAllRequest"."MPFeederId"="Tbl_MPFeeder"."MPFeederId") LEFT OUTER JOIN "CcRequesterSetad"."dbo"."TblBazdid" "TblBazdid" ON "TblRequest"."RequestId"="TblBazdid"."RequestId") LEFT OUTER JOIN "CcRequesterSetad"."dbo"."Tbl_CallReason" "Tbl_CallReason" ON "TblRequest"."CallReasonId"="Tbl_CallReason"."CallReasonId") LEFT OUTER JOIN "CcRequesterSetad"."dbo"."TblLPRequest" "TblLPRequest" ON "TblRequest"."LPRequestId"="TblLPRequest"."LPRequestId") LEFT OUTER JOIN "CcRequesterSetad"."dbo"."TblMPRequest" "TblMPRequest" ON "TblRequest"."MPRequestId"="TblMPRequest"."MPRequestId") LEFT OUTER JOIN "CcRequesterSetad"."dbo"."Tbl_Subscriber" "Tbl_Subscriber" ON "TblRequest"."SubscriberId"="Tbl_Subscriber"."SubscriberId") LEFT OUTER JOIN "CcRequesterSetad"."dbo"."TblRequestInfo" "TblRequestInfo" ON "TblRequest"."RequestId"="TblRequestInfo"."RequestId") LEFT OUTER JOIN "CcRequesterSetad"."dbo"."Tbl_UndoneReason" "Tbl_UndoneReason" ON "TblBazdid"."UndoneReasonId"="Tbl_UndoneReason"."UndoneReasonId") LEFT OUTER JOIN "CcRequesterSetad"."dbo"."Tbl_Master" "Tbl_Master" ON "TblBazdid"."MasterId"="Tbl_Master"."MasterId"
  where TblRequest.DisconnectDatePersian >= '1400/02/01'


