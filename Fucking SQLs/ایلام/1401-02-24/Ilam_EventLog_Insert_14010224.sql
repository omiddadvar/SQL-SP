INSERT INTO [CCRequesterSetad].[dbo].[Tbl_EventLogCenter]
           ([TableName]
           ,[TableNameId]
           ,[PrimaryKeyId]
           ,[Operation]
           ,[AreaId]
           ,[WorkingAreaId]
           ,[DataEntryDT]
           )
     SELECT 
           'Tbl_Subscriber',
           13,
           SubscriberId,
           2,
           2,
           99,
           GetDate()
        FROM Tbl_Subscriber
        Where AreaId = 2
     



