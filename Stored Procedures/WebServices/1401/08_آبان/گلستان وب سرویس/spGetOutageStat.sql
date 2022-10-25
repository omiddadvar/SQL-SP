CREATE PROCEDURE spGetOutageStat 
        @aFromDate AS VARCHAR(11),
        @aToDate AS VARCHAR(11),
        @aAreaIds AS VARCHAR(2000) = ''
AS
  BEGIN
    CREATE TABLE #tmpReq 
    (
        Area NVARCHAR(50),
        MPPostName NVARCHAR(30),
        MPFeederName NVARCHAR(50),
        LPPostName NVARCHAR(100),
        LPFeederName NVARCHAR(50),
        IsTotalDisconnect BIT,
        OutageType VARCHAR(20),
        Count INT,
        DisconnectInterval INT,
        DisconnectPowerPlanned FLOAT,
        DisconnectPowerNotPlanned FLOAT,
    )
    
    
    INSERT INTO #tmpReq EXEC spGetOutageStat_MPPosts @aFromDate , @aToDate, @aAreaIds
    INSERT INTO #tmpReq EXEC spGetOutageStat_MPFeeders @aFromDate , @aToDate, @aAreaIds
    INSERT INTO #tmpReq EXEC spGetOutageStat_LPPosts @aFromDate , @aToDate, @aAreaIds
    INSERT INTO #tmpReq EXEC spGetOutageStat_LPFeeders @aFromDate , @aToDate, @aAreaIds
    
    SELECT * FROM #tmpReq 
    ORDER BY Area , OutageType , IsTotalDisconnect 
    
    DROP TABLE #tmpReq  	
  END