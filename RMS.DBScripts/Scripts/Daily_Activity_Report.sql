USE [RmsDb]
GO
/****** Object:  StoredProcedure [dbo].[DailyActivityReport]    Script Date: 06/09/2016 09:33:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<John Wise>
-- Create date: <22/06/2016>
-- Description:	<Daily Activity Report>
-- =============================================
ALTER PROCEDURE [dbo].[DailyActivityReport] 
	@Payroll_Start datetime,
	@Payroll_End datetime,

	@SiteLocationName nvarchar(50) = NULL,
	@SubSiteLocationName nvarchar(50) = NULL,
	@Firstname nvarchar(50) = NULL,
	@Lastname nvarchar(50) = NULL,
	@Role nvarchar(50) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF @SiteLocationName = N''
	SET @SiteLocationName = NULL

	IF @SubSiteLocationName = N''
	SET @SubSiteLocationName = NULL

	IF @Firstname = N''
	SET @Firstname = NULL

	IF @Lastname = N''
	SET @Lastname = NULL

	IF @Role = N''
		SET @Role = NULL
	ELSE
		SET @Role = @Role + '%'

	SELECT 
	FORMAT(@Payroll_Start, 'yyyy-MM') AS N'Period',
	ste.Name AS N'SiteLocation', 
	ss.Name AS N'SubSiteLocation',
	st.Name AS N'Role',
	u.ExternalTimeSystemId AS N'ZKTimeSystemId',
	u.PayrollReferenceNumber AS N'PayrollReferenceNumber',
	u.Firstname AS N'Firstname', 
	u.LastName AS N'Lastname',
	et.Name AS N'Type',
	FORMAT (sp.StartDateTime, 'd', 'en-gb') AS N'Date',
	CONVERT (CHAR(8), sp.StartDateTime, 108) AS N'RmsShiftStart', 
	CONVERT (CHAR(8), sp.EndDateTime, 108) AS N'RmsShiftEnd',
	CAST (((st.Duration/1000)/60)/60 AS float) AS N'ShiftHours',
	CAST ((st.UnpaidBreakDuration/1000)/60 AS int) AS N'UnpaidBreak',
	CONVERT (CHAR(8), sp.ZktStartDateTime, 108) AS N'ZKTimeClockIn', 
	CONVERT (CHAR(8), sp.ZktEndDateTime, 108) AS N'ZKTimeClockOut',
	CONVERT (CHAR(8), sp.ActualStartDateTime, 108) AS N'ActualStart', 
	CONVERT (CHAR(8), sp.ActualEndDateTime, 108) AS N'ActualEnd',
	
	CAST (
		CASE
			WHEN sp.Status = 5 THEN
				ROUND((sp.HoursWorked/1000)/60.0/60.0, 2)
			WHEN sp.Status = 6 THEN
				ROUND((sp.HoursWorked/1000)/60.0/60.0, 2)
			WHEN sp.Status = 8 THEN
				ROUND((sp.HoursWorked/1000)/60.0/60.0, 2)
			ELSE
				ROUND(((sp.HoursWorked - st.UnpaidBreakDuration)/1000)/60.0/60.0, 2)
		END
		AS decimal(36, 2)
	) AS N'ActualHours',

	CAST (
			CASE 
				WHEN sp.Status = 0 THEN N'Valid'
				WHEN sp.Status = 1 THEN N'Late In'
				WHEN sp.Status = 2 THEN N'Early Out'
				WHEN sp.Status = 3 THEN N'Early In'
				WHEN sp.Status = 4 THEN N'Late Out'
				WHEN sp.Status = 5 THEN N'Missing Clock In'
				WHEN sp.Status = 6 THEN N'Missing Clock Out'
				WHEN sp.Status = 7 THEN N'No Show'
				WHEN sp.Status = 8 THEN N'Overtime'
				ELSE N'Other'
			END AS nvarchar) AS N'ActualStatus',
	CAST (
			CASE 
				WHEN sp.IsApproved = 0 THEN N'Unapproved'
				ELSE N'Approved'
			END AS nvarchar) AS N'Approval',
	CAST (
		CASE 
			WHEN sp.IsModified = 1 THEN N'Yes' 
			ELSE N'' 
		END AS nvarchar) AS N'Modified',
	sp.Reason AS N'Reason',
	sp.Notes AS N'Notes'

	FROM [RmsDb].[dbo].[ShiftProfile] AS sp
	JOIN [RmsDb].[dbo].[Shift] AS s ON sp.ShiftId = s.Id
	JOIN [RmsDb].[dbo].[ShiftTemplate] AS st ON s.ShiftTemplateId = st.Id
	JOIN [RmsDb].[dbo].[Site] AS ste ON st.SiteId = ste.Id
	JOIN [RmsDb].[dbo].[SubSite] AS ss ON st.SubSiteId = ss.Id
	JOIN [RmsDb].[dbo].[Employee] AS e ON sp.EmployeeId = e.Id
	JOIN [RmsDb].[dbo].[EmployeeType] AS et ON e.EmployeeTypeId = et.Id
	JOIN [RmsDb].[dbo].[User] AS u ON e.UserId = u.Id

	WHERE sp.StartDateTime >= @Payroll_Start
	AND sp.StartDateTime <= @Payroll_End
	
	AND (@SiteLocationName IS NULL OR ste.Name = @SiteLocationName)
	AND (@SubSiteLocationName IS NULL OR ss.Name = @SubSiteLocationName)
	AND (@Firstname IS NULL OR u.Firstname = @Firstname)
	AND (@Lastname IS NULL OR u.Lastname = @Lastname)
	AND (@Role IS NULL OR st.Name LIKE @Role)

	ORDER BY u.Firstname, sp.StartDateTime
END
