using NetBlaze.Domain.DatabaseObjects.CommonInterfaces;


namespace NetBlaze.Domain.DatabaseObjects.Views.Attendance
{
    internal class CreatingEmployeeCheckInviolationsView : IDatabaseView
    {
        public string CreateOrReplaceCommand => @"CREATE OR REPLACE VIEW vw_employeecheckinviolations AS
                                                    SELECT 
                                                        ea.Id                AS AttendanceId,
                                                        ea.AttendDate        AS AttendDate,
                                                        ea.UserId            AS UserId,
                                                        u.UserName           AS UserName,
                                                        p.Id                 AS PolicyId,
                                                        p.PolicyName         AS PolicyName,
                                                        p.PolicyCode         AS PolicyCode,
                                                        p.PolicyType         AS PolicyType,
                                                        CONCAT(
                                                            'Arrived at ',
                                                            DATE_FORMAT(ea.AttendTime, '%h:%i %p')
                                                        ) AS Clarification,
                                                        p.ActionValue        AS ViolationValue,

                                                        apa.IsApplied        AS IsApplied,

                                                        CASE
                                                            WHEN apa.Id IS NULL THEN 'Pending'
                                                            WHEN apa.IsApplied = 1 THEN 'Applied'
                                                            WHEN apa.IsApplied = 0 THEN 'Rejected'
                                                        END AS ViolationStatus

                                                    FROM employeeattendences ea

                                                    JOIN (
                                                        SELECT 
                                                            UserId,
                                                            AttendDate,
                                                            MIN(AttendTime) AS FirstCheckIn
                                                        FROM employeeattendences
                                                        GROUP BY UserId, AttendDate
                                                    ) firstcheck
                                                        ON firstcheck.UserId = ea.UserId
                                                       AND firstcheck.AttendDate = ea.AttendDate
                                                       AND firstcheck.FirstCheckIn = ea.AttendTime

                                                    JOIN users u 
                                                        ON u.Id = ea.UserId

                                                    JOIN policies p  ON p.PolicyCode = 'CHECKIN'
                                                    LEFT JOIN attendencepolicyactions apa
                                                        ON apa.AttendenceId = ea.Id
                                                       AND apa.PolicyId = p.Id

                                                    WHERE ea.AttendTime NOT BETWEEN p.WorkStartTime AND p.WorkEndTime;
                                                    ";
    }
}
