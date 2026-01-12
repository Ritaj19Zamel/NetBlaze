

using NetBlaze.Domain.DatabaseObjects.CommonInterfaces;

namespace NetBlaze.Domain.DatabaseObjects.Views.Attendance
{
    public class CreatingAttendanceDailyReportView : IDatabaseView
    {
        public string CreateOrReplaceCommand => @"CREATE OR REPLACE VIEW
                                    vw_attendancedailyreport AS
                                        SELECT 
                                            a.UserId AS UserId,
                                            u.DisplayName AS DisplayName,
                                            a.AttendDate AS AttendDate,
                                            MIN(a.AttendTime) AS CheckIn,
                                            (CASE
                                                WHEN (COUNT(0) > 1) THEN MAX(a.AttendTime)
                                                ELSE NULL
                                            END) AS CheckOut
                                        FROM
                                            (employeeattendences a
                                            JOIN users u ON ((u.Id = a.UserId)))
                                        GROUP BY a.UserId , u.DisplayName , a.AttendDate";
    }
}
