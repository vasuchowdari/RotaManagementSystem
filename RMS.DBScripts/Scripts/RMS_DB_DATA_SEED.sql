	-- System Access Roles
INSERT INTO [dbo].[SystemAccessRole]
(Name, IsActive) VALUES
(N'Admin', 1),
(N'Manager', 1),
(N'User', 1)


INSERT INTO [dbo].[Gender]
(Name, IsActive) VALUES
(N'Male', 1),
(N'Female', 1)


-- Employee Types
INSERT INTO [dbo].[EmployeeType]
(Name, IsActive) VALUES
(N'Salaried', 1),
(N'Permanent', 1),
(N'Contract', 1),
(N'Bank', 1)


-- Leave Types
INSERT INTO [dbo].[LeaveType]
(Name, IsActive) VALUES
(N'Annual', 1),
(N'Maternity', 1),
(N'Paternity', 1),
(N'Sick', 1)


-- Companies
INSERT INTO [dbo].[Company]
(ContactName, Name, Address, City, Postcode, Telephone, Fax, Email, IsActive) VALUES
(N'Bill Sykes', N'Broad Oak Manor', N'10 Somewhere', N'Hertford', N'SG18 2SD', N'01462 123 456', N'01462 654 321', N'info@nouvita.co.uk', 1),
(N'Claire Blair', N'Psycare', N'34-38 Any St.', N'Hitchin', N'SG5 3RT', N'01462 876 543', N'01462 456 123', N'info@psycare.co.uk', 1),
(N'Jenny Bush', N'Nouvita', N'4 London Road', N'Letchworth', N'SG12 8TY', N'01462 543 987', N'01462 513 935', N'info@broadoak.co.uk', 1)


-- Agencies
INSERT INTO [dbo].[Agency]
(IsApproved, Name, Address, City, Postcode, Telephone, Fax, Email, IsActive) VALUES
(1, N'Agency 1', N'101 Carlton Place', N'St. Albans', N'SA3 4FG', N'01446 123 456', N'01446 654 321', N'info@agencyone.co.uk', 1),
(1, N'Agency 2', N'24 Jury St.', N'Baldock', N'SG8 4FD', N'01436 435 678', N'01436 678 435', N'info@agencytwo.co.uk', 1),
(0, N'Agency 3', N'54 Hitchin Road', N'Ware', N'SG19 5VB', N'01463 678 263', N'01463 263 678', N'info@agencythree.co.uk', 1)


-- Users
INSERT INTO [dbo].[User]
(Login, Password, Email, Firstname, Lastname, SystemAccessRoleId, IsAccountLocked, ExternalTimeSystemId, IsActive) VALUES
(N'JohnW', N'1000:cv5tAby+JSj4DtPkqYTuD6LDSjt5ipoD:HO2BIIeGwEYLc5GqKwTHWC+Bd9qPmFk3', N'john@jwise.co.uk', N'John', N'Wise', 1, 0, null, 1),
(N'JaneD', N'1000:SwIfttFkAQKFElUAsib8kxsJ5p74RavX:m2JEH1Z+8HNMQmyFBODLDuR+89t1gbOp', N'janedoe@broadoak.co.uk', N'Jane', N'Doe', 2, 0, null, 1),




(N'SiobhanH', N'1000:SwIfttFkAQKFElUAsib8kxsJ5p74RavX:m2JEH1Z+8HNMQmyFBODLDuR+89t1gbOp', N'siobhanharrison@broadoak.co.uk', N'Siobhan', N'Harrison', 3, 0, 49, 1), 	-- REAL EMPLOYEE - EmpID: 2
(N'NathanS', N'1000:SwIfttFkAQKFElUAsib8kxsJ5p74RavX:m2JEH1Z+8HNMQmyFBODLDuR+89t1gbOp', N'nathanshava@broadoak.co.uk', N'Nathan', N'Shava', 3, 0, 45, 1), 			-- REAL EMPLOYEE - EmpID: 3
(N'MirandaT', N'1000:SwIfttFkAQKFElUAsib8kxsJ5p74RavX:m2JEH1Z+8HNMQmyFBODLDuR+89t1gbOp', N'mirandatapona@broadoak.co.uk', N'Miranda', N'Tapona', 3, 0, 529, 1), 	-- REAL EMPLOYEE - EmpID: 4
(N'IshmaelO', N'1000:SwIfttFkAQKFElUAsib8kxsJ5p74RavX:m2JEH1Z+8HNMQmyFBODLDuR+89t1gbOp', N'ishmaelodonkor@broadoak.co.uk', N'Ishmael', N'Odonkor', 3, 0, 37, 1), 	-- REAL EMPLOYEE - EmpID: 5
(N'TennysonK', N'1000:SwIfttFkAQKFElUAsib8kxsJ5p74RavX:m2JEH1Z+8HNMQmyFBODLDuR+89t1gbOp', N'tennysonkaseke@broadoak.co.uk', N'Tennyson', N'Kaseke', 3, 0, 65, 1), 	-- REAL EMPLOYEE - EmpID: 6
(N'DellaU', N'1000:SwIfttFkAQKFElUAsib8kxsJ5p74RavX:m2JEH1Z+8HNMQmyFBODLDuR+89t1gbOp', N'dellaudom@broadoak.co.uk', N'Della', N'Udom', 3, 0, 465, 1), 				-- REAL EMPLOYEE - EmpID: 7
(N'GinaW', N'1000:SwIfttFkAQKFElUAsib8kxsJ5p74RavX:m2JEH1Z+8HNMQmyFBODLDuR+89t1gbOp', N'ginawilmot@broadoak.co.uk', N'Gina', N'Wilmot', 3, 0, 460, 1), 				-- REAL EMPLOYEE - EmpID: 8
(N'KarlW', N'1000:SwIfttFkAQKFElUAsib8kxsJ5p74RavX:m2JEH1Z+8HNMQmyFBODLDuR+89t1gbOp', N'karlward@broadoak.co.uk', N'Karl', N'Ward', 3, 0, 50, 1), 					-- REAL EMPLOYEE - EmpID: 9
(N'BelsonM', N'1000:SwIfttFkAQKFElUAsib8kxsJ5p74RavX:m2JEH1Z+8HNMQmyFBODLDuR+89t1gbOp', N'belsonmatojeni@broadoak.co.uk', N'Belson', N'Matojeni', 3, 0, 34, 1), 	-- REAL EMPLOYEE - EmpID: 10




(N'SwedishC', N'1000:SwIfttFkAQKFElUAsib8kxsJ5p74RavX:m2JEH1Z+8HNMQmyFBODLDuR+89t1gbOp', N'swedishchef@broadoak.co.uk', N'Swedish', N'Chef', 3, 0, null, 1),
(N'NagaM', N'1000:SwIfttFkAQKFElUAsib8kxsJ5p74RavX:m2JEH1Z+8HNMQmyFBODLDuR+89t1gbOp', N'nagam@agency1.com', N'Naga', N'Munchetti', 3, 0, null, 1),
(N'CharlieS', N'1000:SwIfttFkAQKFElUAsib8kxsJ5p74RavX:m2JEH1Z+8HNMQmyFBODLDuR+89t1gbOp', N'charlies@agency1.com', N'Charlie', N'Stayt', 3, 0, null, 1),
(N'LouiseM', N'1000:SwIfttFkAQKFElUAsib8kxsJ5p74RavX:m2JEH1Z+8HNMQmyFBODLDuR+89t1gbOp', N'louisem@agency1.com', N'Louise', N'Minchin', 3, 0, null, 1),
(N'BillT', N'1000:SwIfttFkAQKFElUAsib8kxsJ5p74RavX:m2JEH1Z+8HNMQmyFBODLDuR+89t1gbOp', N'billt@agency1.com', N'Bill', N'Turnbull', 3, 0, null, 1),
(N'HarryK', N'1000:SwIfttFkAQKFElUAsib8kxsJ5p74RavX:m2JEH1Z+8HNMQmyFBODLDuR+89t1gbOp', N'harryk@broadoak.co.uk', N'Harry', N'Kane', 3, 0, null, 1) -- BANK EMPLOYEE


-- Employees
INSERT INTO [dbo].[Employee]
(CompanyId, EmployeeTypeId, Address, City, Postcode, Telephone, Mobile, GenderId, UserId, IsActive) VALUES
(3, 1, N'1 Somewhere Place', N'Letchworth', N'SG8 6TY', N'01462 482 195', N'07975 382 915', 1, 2, 1),

(3, 3, N'21 Sycamore Ave.', N'Ware', N'SG19 4UI', N'01463 834 943', N'07923 836 128', 0, 3, 1),
(3, 3, N'94 Sloe Lane', N'Hitchin', N'SG4 2UP', N'01462 325 698', N'07923 982 345', 0, 4, 1),
(3, 3, N'8 Acacia Rd.', N'Hertford', N'SG18 5DF', N'01462 573 836', N'07975 398 125', 0, 5, 1),
(3, 3, N'2b Fenay Lea Drive', N'Letchworth', N'SG5 5DS', N'01462 836 573', N'07975 125 398', 0, 6, 1),
(3, 3, N'24 Acacia Rd.', N'Hertford', N'SG18 5DF', N'01462 773 833', N'07973 392 124', 0, 7, 1),
(3, 3, N'108 Mill Drive', N'Stevenage', N'SG1 3TF', N'01462 174 732', N'07978 763 245', 0, 8, 1),
(3, 3, N'3 Church Lane', N'Moorden', N'SD3 9JF', N'01462 456 732', N'07978 763 453', 0, 9, 1),
(3, 3, N'41 Whinbush Road', N'Hitchin', N'SG4 7GH', N'01462 345 872', N'07923 546 242', 0, 10, 1),
(3, 3, N'71 Nelson Mandella House', N'Peckham', N'SW1 3HJ', N'01462 826 528', N'07348 456 245', 0, 11, 1),
(3, 3, N'808 The Studio', N'Huddersfield', N'HD5 8RR', N'01484 826 528', N'07976 456 245', 0, 12, 1),
(3, 4, N'748 High Road', N'Tottenham', N'N17 0AP', N'0203 123 4567', N'07972 426 248', 0, 17, 1)


-- Agency Workers
INSERT INTO [dbo].[AgencyWorker]
(IsActive, AgencyId, Address, City, Postcode, Telephone, Mobile, GenderId, UserId) VALUES
(1, 1, N'14 Salford Place', N'Manchester', N'MN3 6AS', N'0114 212 3456', N'07342 765 234', 0, 13),
(1, 1, N'23 Salford Place', N'Manchester', N'MN3 6AS', N'0114 212 3456', N'07342 765 234', 0, 14),
(1, 1, N'5 Salford Place', N'Manchester', N'MN3 6AS', N'0114 212 3456', N'07342 765 234', 0, 15),
(1, 1, N'9 Salford Place', N'Manchester', N'MN3 6AS', N'0114 212 3456', N'07342 765 234', 0, 16)


-- Personnel Leave Profiles
INSERT INTO [dbo].[PersonnelLeaveProfile]
(IsActive, EmployeeId, AgencyWorkerId, LeaveTypeId, NumberOfDaysAllocated, NumberOfDaysTaken, NumberOfDaysRemaining, Notes) VALUES
(1, 2, null, 1, 19, 1, 19, N'Test Notes'),
(1, 2, null, 4, 10, 0, 10, N'Sick Notes')


-- Leave Requests
INSERT INTO [dbo].[LeaveRequest]
(IsActive, EmployeeId, AgencyWorkerId, LeaveTypeId, StartDateTime, EndDateTime, AmountRequested, IsApproved, IsTaken, Notes) VALUES
(1, 2, null, 1, '20160330 12:00:00 AM', '20160330 11:59:59 PM', 1, 1, 0, N'Test Notes')


-- Resources
INSERT INTO [dbo].[Resource]
(Name, BaseRate, IsActive) VALUES
(N'RMN', 9.80, 1),
(N'HCA', 8.00, 1),
(N'Chef', 8.20, 1),
(N'Cleaner', 7.80, 1),
(N'Site Manager', 10.00, 1),
(N'Office Administrator', 8.50, 1)


-- Contracts
INSERT INTO [dbo].[Contract]
(ResourceId, EmployeeId, AgencyWorkerId, WeeklyHours, BaseRateOverride, OvertimeModifierOverride, IsActive) VALUES
(5, 1, null, 40, null, null, 1),
(4, 2, null, 40, null, null, 1),
(1, 3, null, 40, null, null, 1),
(1, 4, null, 40, 10.2, null, 1),
(1, 5, null, 40, null, null, 1),
(1, 6, null, 40, null, null, 1),
(1, 7, null, 40, null, null, 1),
(1, 8, null, 40, null, null, 1),
(3, 9, null, 40, null, null, 1),
(1, 10, null, 40, null, null, 1),
(3, 11, null, 40, null, null, 1),
(1, null, 1, 40, 15, null, 1),
(1, null, 2, 40, 15, null, 1),
(2, null, 3, 40, 15.5, null, 1),
(2, null, 4, 40, 15, null, 1),
(1, 12, null, 40, 12, null, 1)


-- Site Types
INSERT INTO [dbo].[SiteType]
(Name, IsActive) VALUES
(N'Hospital', 1),
(N'Home', 1)


-- SubSite Types
INSERT INTO [dbo].[SubSiteType]
(Name, IsActive) VALUES
(N'Ward', 1),
(N'Floor', 1)


-- Sites
INSERT INTO [dbo].[Site]
(Name, PayrollStartDate, SiteTypeId, CompanyId, IsActive) VALUES
(N'Hospital A', 21, 1, 1, 1),
(N'Hospital B', 21, 1, 1, 1),
(N'Hospital C', 21, 1, 1, 1),
(N'Home A', 21, 2, 1, 1),
(N'Home B', 21, 2, 1, 1),
(N'Home C', 21, 2, 1, 1),
(N'Hospital D', 21, 1, 2, 1),
(N'Hospital E', 21, 1, 2, 1),
(N'Hospital F', 21, 1, 2, 1),
(N'Home D', 21, 2, 2, 1),
(N'Home E', 21, 2, 2, 1),
(N'Home F', 21, 2, 2, 1),
(N'Baldock Manor', 21, 1, 3, 1),
(N'Hospital H', 21, 1, 3, 1),
(N'Hospital J', 21, 1, 3, 1),
(N'Home G', 21, 2, 3, 1),
(N'Home H', 21, 2, 3, 1),
(N'Home J', 21, 2, 3, 1)


-- SubSites
INSERT INTO [dbo].[SubSite]
(Name, SubSiteTypeId, SiteId, IsActive) VALUES
(N'Ward A', 1, 1, 1),
(N'Ward B', 1, 1, 1),
(N'Ward C', 1, 2, 1),
(N'Ward D', 1, 2, 1),
(N'Ward E', 1, 3, 1),
(N'Ward F', 1, 3, 1),

(N'Ward G', 1, 7, 1),
(N'Ward H', 1, 7, 1),
(N'Ward J', 1, 8, 1),
(N'Ward K', 1, 8, 1),
(N'Ward L', 1, 9, 1),
(N'Ward M', 1, 9, 1),

(N'Ward N', 1, 13, 1),
(N'Ward P', 1, 13, 1),
(N'Ward Q', 1, 14, 1),
(N'Ward R', 1, 14, 1),
(N'Ward S', 1, 15, 1),
(N'Ward T', 1, 15, 1),

(N'Floor A', 2, 4, 1),
(N'Floor B', 2, 4, 1),
(N'Floor C', 2, 5, 1),
(N'Floor D', 2, 5, 1),
(N'Floor E', 2, 6, 1),
(N'Floor F', 2, 6, 1),

(N'Floor G', 2, 10, 1),
(N'Floor H', 2, 10, 1),
(N'Floor J', 2, 11, 1),
(N'Floor K', 2, 11, 1),
(N'Floor L', 2, 12, 1),
(N'Floor M', 2, 12, 1),

(N'Floor N', 2, 16, 1),
(N'Floor P', 2, 16, 1),
(N'Floor Q', 2, 17, 1),
(N'Floor R', 2, 17, 1),
(N'Floor S', 2, 18, 1),
(N'Floor T', 2, 18, 1)


-- Site Personnel Lookups
INSERT INTO [dbo].[SitePersonnelLookup]
(EmployeeId, AgencyWorkerId, SiteId, SubSiteId, IsActive) VALUES
(1, null, 13, null, 1),
(1, null, 13, 13, 1),
(1, null, 13, 14, 1),
(2, null, 13, null, 1), -- CLEANER. ONLY HAS SITE LVL ACCESS
(3, null, 13, 13, 1),
(3, null, 13, 14, 1),
(4, null, 13, 13, 1),
(4, null, 13, 14, 1),
(5, null, 13, 13, 1),
(5, null, 13, 14, 1),
(6, null, 13, 13, 1),
(6, null, 13, 14, 1),
(7, null, 13, 13, 1),
(7, null, 13, 14, 1),
(8, null, 13, 13, 1),
(8, null, 13, 14, 1),
(9, null, 13, 13, 1),
(9, null, 13, 14, 1),
(10, null, 13, 13, 1),
(10, null, 13, 14, 1),
(11, null, 13, null, 1), -- CHEF. ONLY HAS SITE LVL ACCESS
(null, 1, 13, 13, 1), -- Agency Workers
(null, 1, 13, 14, 1),
(null, 2, 13, 13, 1),
(null, 2, 13, 14, 1),
(null, 3, 13, 13, 1),
(null, 3, 13, 14, 1),
(null, 4, 13, 13, 1),
(null, 4, 13, 14, 1),
(12, null, 13, 13, 1), -- Bank Employee
(12, null, 13, 14, 1)


-- Calendars
INSERT INTO [dbo].[Calendar]
(Name, StartDate, EndDate, SiteId, SubSiteId, IsActive) VALUES
(N'Baldock Manor - Rota', '20160101 12:00:00 AM', '20161231 11:59:59 PM', 13, null, 1),
(N'Ward N - Rota', '20160101 12:00:00 AM', '20161231 11:59:59 PM', 13, 13, 1),
(N'Ward P - Rota', '20160101 12:00:00 AM', '20161231 11:59:59 PM', 13, 14, 1)


-- Shift Types
INSERT INTO [dbo].[ShiftType]
(Name, IsOvernight, IsActive) VALUES
(N'Morning', 0, 1),
(N'Day', 0, 1),
(N'Night', 1, 1),
(N'Long Morning', 0, 1),
(N'Long Day', 0, 1),
(N'Long Night', 1, 1),
(N'Bank Holiday Morning', 0, 1),
(N'Bank Holiday Day', 0, 1),
(N'Bank Holiday Night', 1, 1)


-- Resource Rate Modifiers
INSERT INTO [dbo].[ResourceRateModifier]
(Name, Value, ShiftTypeId, ResourceId, IsActive) VALUES
(N'Weekend Morning, RMN', 0.125, 1, 1, 1),
(N'Weekend Day, RMN', 0.125, 2, 1, 1),
(N'Weekend Night, RMN', 0.2, 3, 1, 1)


-- Shift Templates
INSERT INTO [dbo].[ShiftTemplate]
(Name, ShiftRate, ShiftTypeId, ResourceId, SiteId, SubSiteId, StartTime, EndTime, Duration, UnpaidBreakDuration, Mon, Tue, Wed, Thu, Fri, Sat, Sun, IsActive) VALUES
(N'Chef Weekday Morning', 8.0, 1, 3, 13, null, '20160101 06:00:00 AM', '20160101 02:00:00 PM', 8, 0.5, 1, 1, 1, 1, 1, 0, 0, 1),
(N'Chef Weekday Day', 8.0, 2, 3, 13, null, '20160101 02:00:00 PM', '20160101 10:00:00 PM', 8, 0.5, 1, 1, 1, 1, 1, 0, 0, 1),
(N'Chef Weekend Morning', 8.0, 1, 3, 13, null, '20160101 06:00:00 AM', '20160101 02:00:00 PM', 8, 0.5, 0, 0, 0, 0, 0, 1, 1, 1),
(N'Chef Weekend Day', 8.0, 2, 3, 13, null, '20160101 02:00:00 PM', '20160101 10:00:00 PM', 8, 0.5, 0, 0, 0, 0, 0, 1, 1, 1),

(N'RMN Baldock Day', 9.5, 2, 1, 13, 13, '20160101 08:00:00 AM', '20160101 08:30:00 PM', 12.5, 0.5, 1, 1, 1, 1, 1, 0, 0, 1),
(N'RMN Baldock Day (Short)', 9.5, 2, 1, 13, 13, '20160101 08:30:00 AM', '20160101 05:00:00 PM', 8.5, 0.5, 1, 1, 1, 1, 1, 0, 0, 1),
(N'RMN Baldock Night (Short)', 9.5, 2, 1, 13, 14, '20160101 09:00:00 PM', '20160101 08:30:00 AM', 8.5, 0.5, 1, 1, 1, 1, 1, 0, 0, 1),
(N'Cleaner Baldock Morning', 9.5, 1, 1, 13, null, '20160101 09:00:00 AM', '20160101 12:00:00 PM', 3, 0.5, 1, 1, 1, 1, 1, 0, 0, 1),
(N'RMN Baldock Night', 9.5, 3, 1, 13, null, '20160101 08:00:00 PM', '20160101 08:30:00 AM', 12.5, 0.5, 1, 1, 1, 1, 1, 0, 0, 1),

(N'HCA Weekday Morning', 8.0, 1, 2, 13, 13, '20160101 07:00:00 AM', '20160101 03:30:00 PM', 8.5, 0.5, 1, 1, 1, 1, 1, 0, 0, 1),
(N'HCA Weekday Day', 8.0, 2, 2, 13, 13, '20160101 03:00:00 PM', '20160101 11:30:00 PM', 8.5, 0.5, 1, 1, 1, 1, 1, 0, 0, 1),
(N'HCA Weekday Night', 8.5, 3, 2, 13, 13, '20160101 11:00:00 PM', '20160101 07:30:00 AM', 8.5, 0.5, 1, 1, 1, 1, 1, 0, 0, 1),

(N'RMN Weekday Morning', 9.5, 1, 1, 13, 14, '20160101 06:00:00 AM', '20160101 02:30:00 PM', 8.5, 0.5, 1, 1, 1, 1, 1, 0, 0, 1),
(N'RMN Weekday Day', 9.5, 2, 1, 13, 14, '20160101 02:00:00 PM', '20160101 10:30:00 PM', 8.5, 0.5, 1, 1, 1, 1, 1, 0, 0, 1),
(N'RMN Weekday Night', 10.0, 3, 1, 13, 14, '20160101 10:00:00 PM', '20160101 06:30:00 AM', 8.5, 0.5, 1, 1, 1, 1, 1, 0, 0, 1),
(N'HCA Weekday Morning', 8.0, 1, 2, 13, 14, '20160101 07:00:00 AM', '20160101 03:30:00 PM', 8.5, 0.5, 1, 1, 1, 1, 1, 0, 0, 1),
(N'HCA Weekday Day', 8.0, 2, 2, 13, 14, '20160101 03:00:00 PM', '20160101 11:30:00 PM', 8.5, 0.5, 1, 1, 1, 1, 1, 0, 0, 1),
(N'HCA Weekday Night', 8.5, 3, 2, 13, 14, '20160101 11:00:00 PM', '20160101 07:30:00 AM', 8.5, 0.5, 1, 1, 1, 1, 1, 0, 0, 1)


-- CalendarPeriodResourceRequirement
INSERT INTO [dbo].[CalendarResourceRequirement]
(IsActive, CalendarId, ResourceId, StartDate, EndDate) VALUES
(1, 1, 3, '20160101 12:00:00 AM', '20160430 23:59:59 PM'),
(1, 1, 4, '20160101 12:00:00 AM', '20160430 23:59:59 PM'),
(1, 2, 1, '20160101 12:00:00 AM', '20160430 23:59:59 PM'),
(1, 2, 1, '20160101 12:00:00 AM', '20160430 23:59:59 PM'),
(1, 2, 1, '20160101 12:00:00 AM', '20160430 23:59:59 PM'),
(1, 2, 1, '20160101 12:00:00 AM', '20160430 23:59:59 PM'),
(1, 2, 1, '20160101 12:00:00 AM', '20160430 23:59:59 PM'),
(1, 2, 1, '20160101 12:00:00 AM', '20160430 23:59:59 PM')


-- Shift Patterns
INSERT INTO [dbo].[ShiftPattern]
(IsActive, CalendarResourceRequirementId) VALUES
(1, 1),
(1, 2),
(1, 3),
(1, 4),
(1, 5),
(1, 6),
(1, 7),
(1, 8)


-- Shifts
INSERT INTO [dbo].[Shift]
(IsActive, IsAssigned, ShiftTemplateId, ShiftPatternId, StartDate, EndDate, EmployeeId, AgencyWorkerId) VALUES
-- Siobhan Harrison (DAY)
(1, 1, 8, 2, '20160301 09:00:00 AM', '20160301 12:00:00 PM', 2, null),
(1, 1, 8, 2, '20160302 09:00:00 AM', '20160302 12:00:00 PM', 2, null),
(1, 1, 8, 2, '20160303 09:00:00 AM', '20160303 12:00:00 PM', 2, null),
(1, 1, 8, 2, '20160304 09:00:00 AM', '20160304 12:00:00 PM', 2, null),
(1, 1, 8, 2, '20160307 09:00:00 AM', '20160307 12:00:00 PM', 2, null),
(1, 1, 8, 2, '20160308 09:00:00 AM', '20160308 12:00:00 PM', 2, null),
(1, 1, 8, 2, '20160309 09:00:00 AM', '20160309 12:00:00 PM', 2, null),
(1, 1, 8, 2, '20160310 09:00:00 AM', '20160310 12:00:00 PM', 2, null),
(1, 1, 8, 2, '20160311 09:00:00 AM', '20160311 12:00:00 PM', 2, null),
(1, 1, 8, 2, '20160314 09:00:00 AM', '20160314 12:00:00 PM', 2, null),
(1, 1, 8, 2, '20160315 09:00:00 AM', '20160315 12:00:00 PM', 2, null),
(1, 1, 8, 2, '20160316 09:00:00 AM', '20160316 12:00:00 PM', 2, null),
(1, 1, 8, 2, '20160317 09:00:00 AM', '20160317 12:00:00 PM', 2, null),
(1, 1, 8, 2, '20160318 09:00:00 AM', '20160318 12:00:00 PM', 2, null),
(1, 1, 8, 2, '20160321 09:00:00 AM', '20160321 12:00:00 PM', 2, null),
(1, 1, 8, 2, '20160322 09:00:00 AM', '20160322 12:00:00 PM', 2, null),
(1, 1, 8, 2, '20160323 09:00:00 AM', '20160323 12:00:00 PM', 2, null),
(1, 1, 8, 2, '20160324 09:00:00 AM', '20160324 12:00:00 PM', 2, null),
(1, 1, 8, 2, '20160329 09:00:00 AM', '20160329 12:00:00 PM', 2, null),
(1, 1, 8, 2, '20160330 09:00:00 AM', '20160330 12:00:00 PM', 2, null),
(1, 1, 8, 2, '20160331 09:00:00 AM', '20160331 12:00:00 PM', 2, null),
(1, 1, 8, 2, '20160401 09:00:00 AM', '20160401 12:00:00 PM', 2, null),
(1, 1, 8, 2, '20160402 09:00:00 AM', '20160402 12:00:00 PM', 2, null),
(1, 1, 8, 2, '20160404 09:00:00 AM', '20160404 12:00:00 PM', 2, null),
(1, 1, 8, 2, '20160405 09:00:00 AM', '20160405 12:00:00 PM', 2, null),
(1, 1, 8, 2, '20160406 09:00:00 AM', '20160406 12:00:00 PM', 2, null),
(1, 1, 8, 2, '20160407 09:00:00 AM', '20160407 12:00:00 PM', 2, null),
(1, 1, 8, 2, '20160408 09:00:00 AM', '20160408 12:00:00 PM', 2, null),
(1, 1, 8, 2, '20160411 09:00:00 AM', '20160411 12:00:00 PM', 2, null),
(1, 1, 8, 2, '20160412 09:00:00 AM', '20160412 12:00:00 PM', 2, null),
(1, 1, 8, 2, '20160413 09:00:00 AM', '20160413 12:00:00 PM', 2, null),
(1, 1, 8, 2, '20160414 09:00:00 AM', '20160414 12:00:00 PM', 2, null),
(1, 1, 8, 2, '20160415 09:00:00 AM', '20160415 12:00:00 PM', 2, null),
(1, 1, 8, 2, '20160418 09:00:00 AM', '20160418 12:00:00 PM', 2, null),
(1, 1, 8, 2, '20160419 09:00:00 AM', '20160419 12:00:00 PM', 2, null),
(1, 1, 8, 2, '20160420 09:00:00 AM', '20160420 12:00:00 PM', 2, null),

-- Nathan Shava (DAY)
(1, 1, 5, 4, '20160301 08:00:00 AM', '20160301 08:30:00 PM', 3, null),
(1, 1, 5, 4, '20160303 08:00:00 AM', '20160303 08:30:00 PM', 3, null),
(1, 1, 5, 4, '20160304 08:00:00 AM', '20160304 08:30:00 PM', 3, null),
(1, 1, 5, 4, '20160307 08:00:00 AM', '20160307 08:30:00 PM', 3, null),
(1, 1, 5, 4, '20160308 08:00:00 AM', '20160308 08:30:00 PM', 3, null),
(1, 1, 5, 4, '20160310 08:00:00 AM', '20160310 08:30:00 PM', 3, null),
(1, 1, 5, 4, '20160311 08:00:00 AM', '20160311 08:30:00 PM', 3, null),
(1, 1, 5, 4, '20160314 08:00:00 AM', '20160314 08:30:00 PM', 3, null),
(1, 1, 5, 4, '20160316 08:00:00 AM', '20160316 08:30:00 PM', 3, null),
(1, 1, 5, 4, '20160317 08:00:00 AM', '20160317 08:30:00 PM', 3, null),
(1, 1, 5, 4, '20160318 08:00:00 AM', '20160318 08:30:00 PM', 3, null),
(1, 1, 5, 4, '20160321 08:00:00 AM', '20160321 08:30:00 PM', 3, null),
(1, 1, 5, 4, '20160322 08:00:00 AM', '20160322 08:30:00 PM', 3, null),
(1, 1, 5, 4, '20160324 08:00:00 AM', '20160324 08:30:00 PM', 3, null),
(1, 1, 5, 4, '20160325 08:00:00 AM', '20160325 08:30:00 PM', 3, null),
(1, 1, 5, 4, '20160328 08:00:00 AM', '20160328 08:30:00 PM', 3, null),
(1, 1, 5, 4, '20160329 08:00:00 AM', '20160329 08:30:00 PM', 3, null),
(1, 1, 5, 4, '20160331 08:00:00 AM', '20160331 08:30:00 PM', 3, null),
(1, 1, 5, 4, '20160401 08:00:00 AM', '20160401 08:30:00 PM', 3, null),
(1, 1, 5, 4, '20160404 08:00:00 AM', '20160404 08:30:00 PM', 3, null),
(1, 1, 5, 4, '20160405 08:00:00 AM', '20160405 08:30:00 PM', 3, null),
(1, 1, 5, 4, '20160407 08:00:00 AM', '20160407 08:30:00 PM', 3, null),
(1, 1, 5, 4, '20160408 08:00:00 AM', '20160408 08:30:00 PM', 3, null),
(1, 1, 5, 4, '20160411 08:00:00 AM', '20160411 08:30:00 PM', 3, null),
(1, 1, 5, 4, '20160413 08:00:00 AM', '20160413 08:30:00 PM', 3, null),
(1, 1, 5, 4, '20160414 08:00:00 AM', '20160414 08:30:00 PM', 3, null),
(1, 1, 5, 4, '20160415 08:00:00 AM', '20160415 08:30:00 PM', 3, null),
(1, 1, 5, 4, '20160418 08:00:00 AM', '20160418 08:30:00 PM', 3, null),
(1, 1, 5, 4, '20160419 08:00:00 AM', '20160419 08:30:00 PM', 3, null),
(1, 1, 5, 4, '20160421 08:00:00 AM', '20160421 08:30:00 PM', 3, null),
(1, 1, 5, 4, '20160422 08:00:00 AM', '20160422 08:30:00 PM', 3, null),
(1, 1, 5, 4, '20160425 08:00:00 AM', '20160425 08:30:00 PM', 3, null),
(1, 1, 5, 4, '20160426 08:00:00 AM', '20160426 08:30:00 PM', 3, null),
(1, 1, 5, 4, '20160428 08:00:00 AM', '20160428 08:30:00 PM', 3, null),
(1, 1, 5, 4, '20160429 08:00:00 AM', '20160429 08:30:00 PM', 3, null),

-- Miranda Tapona (DAY)
(1, 1, 5, 3, '20160301 08:00:00 AM', '20160301 08:30:00 PM', 4, null),
(1, 1, 5, 3, '20160302 08:00:00 AM', '20160302 08:30:00 PM', 4, null),
(1, 1, 5, 3, '20160304 08:00:00 AM', '20160304 08:30:00 PM', 4, null),
(1, 1, 5, 3, '20160305 08:00:00 AM', '20160305 08:30:00 PM', 4, null),
(1, 1, 5, 3, '20160307 08:00:00 AM', '20160307 08:30:00 PM', 4, null),
(1, 1, 5, 3, '20160308 08:00:00 AM', '20160308 08:30:00 PM', 4, null),
(1, 1, 5, 3, '20160309 08:00:00 AM', '20160309 08:30:00 PM', 4, null),
(1, 1, 5, 3, '20160312 08:00:00 AM', '20160312 08:30:00 PM', 4, null),
(1, 1, 5, 3, '20160313 08:00:00 AM', '20160313 08:30:00 PM', 4, null),
(1, 1, 5, 3, '20160314 08:00:00 AM', '20160314 08:30:00 PM', 4, null),
(1, 1, 5, 3, '20160316 08:00:00 AM', '20160316 08:30:00 PM', 4, null),
(1, 1, 5, 3, '20160318 08:00:00 AM', '20160318 08:30:00 PM', 4, null),
(1, 1, 5, 3, '20160321 08:00:00 AM', '20160321 08:30:00 PM', 4, null),
(1, 1, 5, 3, '20160322 08:00:00 AM', '20160322 08:30:00 PM', 4, null),
(1, 1, 5, 3, '20160323 08:00:00 AM', '20160323 08:30:00 PM', 4, null),
(1, 1, 5, 3, '20160329 08:00:00 AM', '20160329 08:30:00 PM', 4, null),
(1, 1, 5, 3, '20160401 08:00:00 AM', '20160401 08:30:00 PM', 4, null),
(1, 1, 5, 3, '20160402 08:00:00 AM', '20160402 08:30:00 PM', 4, null),
(1, 1, 5, 3, '20160403 08:00:00 AM', '20160403 08:30:00 PM', 4, null),
(1, 1, 5, 3, '20160406 08:00:00 AM', '20160406 08:30:00 PM', 4, null),
(1, 1, 5, 3, '20160407 08:00:00 AM', '20160407 08:30:00 PM', 4, null),
(1, 1, 5, 3, '20160408 08:00:00 AM', '20160408 08:30:00 PM', 4, null),
(1, 1, 5, 3, '20160414 08:00:00 AM', '20160414 08:30:00 PM', 4, null),
(1, 1, 5, 3, '20160415 08:00:00 AM', '20160415 08:30:00 PM', 4, null),
(1, 1, 5, 3, '20160416 08:00:00 AM', '20160416 08:30:00 PM', 4, null),
(1, 1, 5, 3, '20160417 08:00:00 AM', '20160417 08:30:00 PM', 4, null),
(1, 1, 5, 3, '20160419 08:00:00 AM', '20160419 08:30:00 PM', 4, null),
(1, 1, 5, 3, '20160420 08:00:00 AM', '20160420 08:30:00 PM', 4, null),
(1, 1, 5, 3, '20160421 08:00:00 AM', '20160421 08:30:00 PM', 4, null),
(1, 1, 5, 3, '20160422 08:00:00 AM', '20160422 08:30:00 PM', 4, null),
(1, 1, 5, 3, '20160425 08:00:00 AM', '20160425 08:30:00 PM', 4, null),

-- Ishmael Odonkor (NIGHT)
(1, 1, 9, 4, '20160304 08:00:00 PM', '20160305 08:30:00 AM', 5, null),
(1, 1, 9, 4, '20160305 08:00:00 PM', '20160306 08:30:00 AM', 5, null),
(1, 1, 9, 4, '20160306 08:00:00 PM', '20160307 08:30:00 AM', 5, null),
(1, 1, 9, 4, '20160312 08:00:00 PM', '20160313 08:30:00 AM', 5, null),
(1, 1, 9, 4, '20160313 08:00:00 PM', '20160314 08:30:00 AM', 5, null),
(1, 1, 9, 4, '20160319 08:00:00 PM', '20160320 08:30:00 AM', 5, null),
(1, 1, 9, 4, '20160320 08:00:00 PM', '20160321 08:30:00 AM', 5, null),
(1, 1, 9, 4, '20160401 08:00:00 PM', '20160402 08:30:00 AM', 5, null),
(1, 1, 9, 4, '20160402 08:00:00 PM', '20160403 08:30:00 AM', 5, null),
(1, 1, 9, 4, '20160403 08:00:00 PM', '20160404 08:30:00 AM', 5, null),
(1, 1, 9, 4, '20160408 08:00:00 PM', '20160409 08:30:00 AM', 5, null),
(1, 1, 9, 4, '20160409 08:00:00 PM', '20160410 08:30:00 AM', 5, null),
(1, 1, 9, 4, '20160410 08:00:00 PM', '20160411 08:30:00 AM', 5, null),
(1, 1, 9, 4, '20160415 08:00:00 PM', '20160415 08:30:00 AM', 5, null),
(1, 1, 9, 4, '20160416 08:00:00 PM', '20160416 08:30:00 AM', 5, null),
(1, 1, 9, 4, '20160417 08:00:00 PM', '20160417 08:30:00 AM', 5, null),
(1, 1, 9, 4, '20160422 08:00:00 PM', '20160423 08:30:00 AM', 5, null),
(1, 1, 9, 4, '20160423 08:00:00 PM', '20160424 08:30:00 AM', 5, null),
(1, 1, 9, 4, '20160424 08:00:00 PM', '20160425 08:30:00 AM', 5, null),


-- Tennyson Kaseke (NIGHT & DAY MIX)
(1, 1, 9, 6, '20160309 08:00:00 PM', '20160310 08:30:00 AM', 6, null),
(1, 1, 9, 6, '20160313 08:00:00 PM', '20160314 08:30:00 AM', 6, null),
(1, 1, 5, 6, '20160324 08:00:00 AM', '20160324 08:30:00 PM', 6, null),
(1, 1, 5, 6, '20160401 08:00:00 AM', '20160401 08:30:00 PM', 6, null),
(1, 1, 5, 6, '20160407 08:00:00 AM', '20160407 08:30:00 PM', 6, null),
(1, 1, 9, 6, '20160410 08:00:00 PM', '20160410 08:30:00 AM', 6, null),
(1, 1, 5, 6, '20160414 08:00:00 AM', '20160414 08:30:00 PM', 6, null),
(1, 1, 9, 6, '20160415 08:00:00 PM', '20160416 08:30:00 AM', 6, null),
(1, 1, 5, 6, '20160417 08:00:00 AM', '20160417 08:30:00 PM', 6, null),
(1, 1, 5, 6, '20160428 08:00:00 AM', '20160428 08:30:00 PM', 6, null),
(1, 1, 9, 6, '20160429 08:00:00 PM', '20160430 08:30:00 AM', 6, null),


-- Della Udom (NIGHT)
(1, 1, 9, 7, '20160301 08:00:00 PM', '20160302 08:30:00 AM', 7, null),
(1, 1, 9, 7, '20160302 08:00:00 PM', '20160303 08:30:00 AM', 7, null),
(1, 1, 9, 7, '20160304 08:00:00 PM', '20160305 08:30:00 AM', 7, null),
(1, 1, 9, 7, '20160305 08:00:00 PM', '20160306 08:30:00 AM', 7, null),
(1, 1, 9, 7, '20160306 08:00:00 PM', '20160307 08:30:00 AM', 7, null),
(1, 1, 9, 7, '20160308 08:00:00 PM', '20160309 08:30:00 AM', 7, null),
(1, 1, 9, 7, '20160312 08:00:00 PM', '20160313 08:30:00 AM', 7, null),
(1, 1, 9, 7, '20160313 08:00:00 PM', '20160314 08:30:00 AM', 7, null),
(1, 1, 9, 7, '20160315 08:00:00 PM', '20160316 08:30:00 AM', 7, null),
(1, 1, 9, 7, '20160316 08:00:00 PM', '20160317 08:30:00 AM', 7, null),
(1, 1, 9, 7, '20160318 08:00:00 PM', '20160319 08:30:00 AM', 7, null),
(1, 1, 9, 7, '20160319 08:00:00 PM', '20160320 08:30:00 AM', 7, null),
(1, 1, 9, 7, '20160320 08:00:00 PM', '20160321 08:30:00 AM', 7, null),
(1, 1, 9, 7, '20160322 08:00:00 PM', '20160323 08:30:00 AM', 7, null),
(1, 1, 9, 7, '20160323 08:00:00 PM', '20160324 08:30:00 AM', 7, null),
(1, 1, 9, 7, '20160325 08:00:00 PM', '20160326 08:30:00 AM', 7, null),
(1, 1, 9, 7, '20160326 08:00:00 PM', '20160327 08:30:00 AM', 7, null),
(1, 1, 9, 7, '20160327 08:00:00 PM', '20160328 08:30:00 AM', 7, null),
(1, 1, 9, 7, '20160329 08:00:00 PM', '20160330 08:30:00 AM', 7, null),
(1, 1, 9, 7, '20160330 08:00:00 PM', '20160331 08:30:00 AM', 7, null),
(1, 1, 9, 7, '20160401 08:00:00 PM', '20160402 08:30:00 AM', 7, null),
(1, 1, 9, 7, '20160402 08:00:00 PM', '20160403 08:30:00 AM', 7, null),
(1, 1, 9, 7, '20160403 08:00:00 PM', '20160404 08:30:00 AM', 7, null),
(1, 1, 9, 7, '20160405 08:00:00 PM', '20160406 08:30:00 AM', 7, null),
(1, 1, 9, 7, '20160406 08:00:00 PM', '20160407 08:30:00 AM', 7, null),
(1, 1, 9, 7, '20160407 08:00:00 PM', '20160408 08:30:00 AM', 7, null),
(1, 1, 9, 7, '20160408 08:00:00 PM', '20160409 08:30:00 AM', 7, null),
(1, 1, 9, 7, '20160410 08:00:00 PM', '20160411 08:30:00 AM', 7, null),
(1, 1, 9, 7, '20160413 08:00:00 PM', '20160414 08:30:00 AM', 7, null),
(1, 1, 9, 7, '20160415 08:00:00 PM', '20160416 08:30:00 AM', 7, null),
(1, 1, 9, 7, '20160416 08:00:00 PM', '20160417 08:30:00 AM', 7, null),
(1, 1, 9, 7, '20160417 08:00:00 PM', '20160418 08:30:00 AM', 7, null),
(1, 1, 9, 7, '20160419 08:00:00 PM', '20160420 08:30:00 AM', 7, null),
(1, 1, 9, 7, '20160420 08:00:00 PM', '20160421 08:30:00 AM', 7, null),
(1, 1, 9, 7, '20160423 08:00:00 PM', '20160424 08:30:00 AM', 7, null),
(1, 1, 9, 7, '20160424 08:00:00 PM', '20160425 08:30:00 AM', 7, null),
(1, 1, 9, 7, '20160426 08:00:00 PM', '20160427 08:30:00 AM', 7, null),
(1, 1, 9, 7, '20160427 08:00:00 PM', '20160428 08:30:00 AM', 7, null),
(1, 1, 9, 7, '20160429 08:00:00 PM', '20160430 08:30:00 AM', 7, null),
(1, 1, 9, 7, '20160430 08:00:00 PM', '20160501 08:30:00 AM', 7, null),


-- Gina Wilmot
(1, 1, 5, 1, '20160302 08:00:00 AM', '20160302 08:30:00 PM', 8, null),
(1, 1, 5, 1, '20160303 08:00:00 AM', '20160303 08:30:00 PM', 8, null),
(1, 1, 5, 1, '20160304 08:00:00 AM', '20160304 08:30:00 PM', 8, null),
(1, 1, 5, 1, '20160308 08:00:00 AM', '20160308 08:30:00 PM', 8, null),
(1, 1, 5, 1, '20160309 08:00:00 AM', '20160309 08:30:00 PM', 8, null),
(1, 1, 5, 1, '20160310 08:00:00 AM', '20160310 08:30:00 PM', 8, null),
(1, 1, 5, 1, '20160311 08:00:00 AM', '20160311 08:30:00 PM', 8, null),
(1, 1, 5, 1, '20160317 08:00:00 AM', '20160317 08:30:00 PM', 8, null),
(1, 1, 5, 1, '20160318 08:00:00 AM', '20160318 08:30:00 PM', 8, null),
(1, 1, 5, 1, '20160319 08:00:00 AM', '20160319 08:30:00 PM', 8, null),
(1, 1, 5, 1, '20160320 08:00:00 AM', '20160320 08:30:00 PM', 8, null),
(1, 1, 5, 1, '20160322 08:00:00 AM', '20160322 08:30:00 PM', 8, null),
(1, 1, 5, 1, '20160323 08:00:00 AM', '20160323 08:30:00 PM', 8, null),
(1, 1, 5, 1, '20160324 08:00:00 AM', '20160324 08:30:00 PM', 8, null),
(1, 1, 5, 1, '20160325 08:00:00 AM', '20160325 08:30:00 PM', 8, null),
(1, 1, 5, 1, '20160326 08:00:00 AM', '20160326 08:30:00 PM', 8, null),
(1, 1, 5, 1, '20160327 08:00:00 AM', '20160327 08:30:00 PM', 8, null),
(1, 1, 5, 1, '20160329 08:00:00 AM', '20160329 08:30:00 PM', 8, null),
(1, 1, 5, 1, '20160330 08:00:00 AM', '20160330 08:30:00 PM', 8, null),
(1, 1, 5, 1, '20160331 08:00:00 AM', '20160331 08:30:00 PM', 8, null),
(1, 1, 5, 1, '20160401 08:00:00 AM', '20160401 08:30:00 PM', 8, null),
(1, 1, 5, 1, '20160406 08:00:00 AM', '20160406 08:30:00 PM', 8, null),
(1, 1, 5, 1, '20160407 08:00:00 AM', '20160407 08:30:00 PM', 8, null),
(1, 1, 5, 1, '20160408 08:00:00 AM', '20160408 08:30:00 PM', 8, null),
(1, 1, 5, 1, '20160409 08:00:00 AM', '20160409 08:30:00 PM', 8, null),
(1, 1, 5, 1, '20160410 08:00:00 AM', '20160410 08:30:00 PM', 8, null),
(1, 1, 5, 1, '20160411 08:00:00 AM', '20160411 08:30:00 PM', 8, null),
(1, 1, 5, 1, '20160412 08:00:00 AM', '20160412 08:30:00 PM', 8, null),
(1, 1, 5, 1, '20160413 08:00:00 AM', '20160413 08:30:00 PM', 8, null),
(1, 1, 5, 1, '20160414 08:00:00 AM', '20160414 08:30:00 PM', 8, null),
(1, 1, 5, 1, '20160415 08:00:00 AM', '20160415 08:30:00 PM', 8, null),
(1, 1, 5, 1, '20160418 08:00:00 AM', '20160418 08:30:00 PM', 8, null),
(1, 1, 5, 1, '20160420 08:00:00 AM', '20160420 08:30:00 PM', 8, null),
(1, 1, 5, 1, '20160423 08:00:00 AM', '20160423 08:30:00 PM', 8, null),
(1, 1, 5, 1, '20160424 08:00:00 AM', '20160424 08:30:00 PM', 8, null),
(1, 1, 5, 1, '20160427 08:00:00 AM', '20160427 08:30:00 PM', 8, null),
(1, 1, 5, 1, '20160428 08:00:00 AM', '20160428 08:30:00 PM', 8, null),
(1, 1, 5, 1, '20160429 08:00:00 AM', '20160429 08:30:00 PM', 8, null),
(1, 1, 5, 1, '20160430 08:00:00 AM', '20160430 08:30:00 PM', 8, null),


-- Karl Ward
(1, 1, 6, 8, '20160301 08:30:00 AM', '20160301 05:00:00 PM', 9, null),
(1, 1, 6, 8, '20160302 08:30:00 AM', '20160302 05:00:00 PM', 9, null),
(1, 1, 6, 8, '20160303 08:30:00 AM', '20160303 05:00:00 PM', 9, null),
(1, 1, 6, 8, '20160304 08:30:00 AM', '20160304 05:00:00 PM', 9, null),
(1, 1, 6, 8, '20160307 08:30:00 AM', '20160307 05:00:00 PM', 9, null),
(1, 1, 6, 8, '20160309 08:30:00 AM', '20160309 05:00:00 PM', 9, null),
(1, 1, 6, 8, '20160310 08:30:00 AM', '20160310 05:00:00 PM', 9, null),
(1, 1, 6, 8, '20160311 08:30:00 AM', '20160311 05:00:00 PM', 9, null),
(1, 1, 6, 8, '20160315 08:30:00 AM', '20160315 05:00:00 PM', 9, null),
(1, 1, 6, 8, '20160316 08:30:00 AM', '20160316 05:00:00 PM', 9, null),
(1, 1, 7, 8, '20160316 09:00:00 PM', '20160317 08:30:00 AM', 9, null),
(1, 1, 6, 8, '20160317 08:30:00 AM', '20160317 05:00:00 PM', 9, null),
(1, 1, 6, 8, '20160318 08:30:00 AM', '20160318 05:00:00 PM', 9, null),
(1, 1, 6, 8, '20160321 08:30:00 AM', '20160321 05:00:00 PM', 9, null),
(1, 1, 6, 8, '20160322 08:30:00 AM', '20160322 05:00:00 PM', 9, null),
(1, 1, 6, 8, '20160323 08:30:00 AM', '20160323 05:00:00 PM', 9, null),
(1, 1, 6, 8, '20160324 08:30:00 AM', '20160324 05:00:00 PM', 9, null),
(1, 1, 6, 8, '20160329 08:30:00 AM', '20160329 05:00:00 PM', 9, null),
(1, 1, 6, 8, '20160330 08:30:00 AM', '20160330 05:00:00 PM', 9, null),
(1, 1, 6, 8, '20160331 08:30:00 AM', '20160331 05:00:00 PM', 9, null),
(1, 1, 6, 8, '20160401 08:30:00 AM', '20160401 05:00:00 PM', 9, null),
(1, 1, 6, 8, '20160404 08:30:00 AM', '20160404 05:00:00 PM', 9, null),
(1, 1, 6, 8, '20160405 08:30:00 AM', '20160405 05:00:00 PM', 9, null),
(1, 1, 6, 8, '20160406 08:30:00 AM', '20160406 05:00:00 PM', 9, null),
(1, 1, 6, 8, '20160407 08:30:00 AM', '20160407 05:00:00 PM', 9, null),
(1, 1, 6, 8, '20160408 08:30:00 AM', '20160408 05:00:00 PM', 9, null),
(1, 1, 6, 8, '20160411 08:30:00 AM', '20160411 05:00:00 PM', 9, null),
(1, 1, 6, 8, '20160412 08:30:00 AM', '20160412 05:00:00 PM', 9, null),
(1, 1, 6, 8, '20160413 08:30:00 AM', '20160413 05:00:00 PM', 9, null),
(1, 1, 6, 8, '20160414 08:30:00 AM', '20160414 05:00:00 PM', 9, null),
(1, 1, 6, 8, '20160415 08:30:00 AM', '20160415 05:00:00 PM', 9, null),


-- Belson Matojeni (NIGHT)
(1, 1, 9, 3, '20160301 08:00:00 PM', '20160302 08:30:00 AM', 10, null),
(1, 1, 9, 3, '20160303 08:00:00 PM', '20160304 08:30:00 AM', 10, null),
(1, 1, 9, 3, '20160304 08:00:00 PM', '20160305 08:30:00 AM', 10, null),
(1, 1, 9, 3, '20160305 08:00:00 PM', '20160306 08:30:00 AM', 10, null),
(1, 1, 9, 3, '20160307 08:00:00 PM', '20160308 08:30:00 AM', 10, null),
(1, 1, 9, 3, '20160308 08:00:00 PM', '20160309 08:30:00 AM', 10, null),
(1, 1, 9, 3, '20160309 08:00:00 PM', '20160310 08:30:00 AM', 10, null),
(1, 1, 9, 3, '20160310 08:00:00 PM', '20160311 08:30:00 AM', 10, null),
(1, 1, 9, 3, '20160311 08:00:00 PM', '20160312 08:30:00 AM', 10, null),
(1, 1, 9, 3, '20160313 08:00:00 PM', '20160314 08:30:00 AM', 10, null),
(1, 1, 9, 3, '20160314 08:00:00 PM', '20160315 08:30:00 AM', 10, null),
(1, 1, 9, 3, '20160315 08:00:00 PM', '20160316 08:30:00 AM', 10, null),
(1, 1, 9, 3, '20160316 08:00:00 PM', '20160317 08:30:00 AM', 10, null),
(1, 1, 9, 3, '20160317 08:00:00 PM', '20160318 08:30:00 AM', 10, null),
(1, 1, 9, 3, '20160319 08:00:00 PM', '20160320 08:30:00 AM', 10, null),
(1, 1, 9, 3, '20160320 08:00:00 PM', '20160321 08:30:00 AM', 10, null),
(1, 1, 9, 3, '20160321 08:00:00 PM', '20160322 08:30:00 AM', 10, null),
(1, 1, 9, 3, '20160322 08:00:00 PM', '20160323 08:30:00 AM', 10, null),
(1, 1, 9, 3, '20160323 08:00:00 PM', '20160324 08:30:00 AM', 10, null),
(1, 1, 9, 3, '20160325 08:00:00 PM', '20160326 08:30:00 AM', 10, null),
(1, 1, 9, 3, '20160328 08:00:00 PM', '20160329 08:30:00 AM', 10, null),
(1, 1, 9, 3, '20160329 08:00:00 PM', '20160330 08:30:00 AM', 10, null),
(1, 1, 9, 3, '20160330 08:00:00 PM', '20160331 08:30:00 AM', 10, null),
(1, 1, 9, 3, '20160405 08:00:00 PM', '20160406 08:30:00 AM', 10, null),
(1, 1, 9, 3, '20160406 08:00:00 PM', '20160407 08:30:00 AM', 10, null),
(1, 1, 9, 3, '20160407 08:00:00 PM', '20160408 08:30:00 AM', 10, null),
(1, 1, 9, 3, '20160408 08:00:00 PM', '20160409 08:30:00 AM', 10, null),
(1, 1, 9, 3, '20160410 08:00:00 PM', '20160411 08:30:00 AM', 10, null),
(1, 1, 9, 3, '20160411 08:00:00 PM', '20160412 08:30:00 AM', 10, null),
(1, 1, 9, 3, '20160412 08:00:00 PM', '20160413 08:30:00 AM', 10, null),
(1, 1, 9, 3, '20160413 08:00:00 PM', '20160414 08:30:00 AM', 10, null),
(1, 1, 9, 3, '20160416 08:00:00 PM', '20160417 08:30:00 AM', 10, null),
(1, 1, 9, 3, '20160417 08:00:00 PM', '20160418 08:30:00 AM', 10, null),
(1, 1, 9, 3, '20160418 08:00:00 PM', '20160419 08:30:00 AM', 10, null),
(1, 1, 9, 3, '20160419 08:00:00 PM', '20160420 08:30:00 AM', 10, null),
(1, 1, 9, 3, '20160420 08:00:00 PM', '20160421 08:30:00 AM', 10, null),
(1, 1, 9, 3, '20160421 08:00:00 PM', '20160422 08:30:00 AM', 10, null),
(1, 1, 9, 3, '20160424 08:00:00 PM', '20160425 08:30:00 AM', 10, null)


-- Budget
INSERT INTO [dbo].[Budget]
(StartDate, EndDate, ForecastTotal, ActualTotal, SiteId, SubSiteId, IsActive) VALUES
('20160101 00:00:00 AM', '20161231 23:59:59 PM', 0, 0, 13, null, 1)


-- Budget Period
INSERT INTO [dbo].[BudgetPeriod]
(IsActive, StartDate, EndDate, Amount, ForecastTotal, ActualSpendTotal, BudgetId) VALUES
(1, '20160103 12:00:00 AM', '20160130 11:59:59 PM', 1000000, 0, 0, 1),
(1, '20160131 12:00:00 AM', '20160227 11:59:59 PM', 1000000, 0, 0, 1),
(1, '20160228 12:00:00 AM', '20160326 11:59:59 PM', 1000000, 0, 0, 1)