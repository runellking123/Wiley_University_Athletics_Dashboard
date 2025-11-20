// WILEY UNIVERSITY ATHLETICS DASHBOARD - COLUMN RENAME SCRIPT
// Adapted from Version 11.20.2025 Live PowerBI Dashboard
// Renames database column names to user-friendly display names for all 6 athletics CSV files

// This script is designed for use with Tabular Editor 3 to clean up column names
// in the athletics dashboard data model after importing CSV files

// Define all column renames as key-value pairs
var renames = new[] {
    // IDENTIFIERS
    new { Old = "ID_NUM", New = "Student ID" },
    new { Old = "APPID", New = "App ID" },
    new { Old = "PartyAppID", New = "Party App ID" },

    // SPORTS-SPECIFIC FIELDS
    new { Old = "ATTRIB_CDE", New = "Sport Code" },
    new { Old = "ATTRIB_BEGIN_DTE", New = "Sport Begin Date" },
    new { Old = "ATTRIB_END_DATE", New = "Sport End Date" },
    new { Old = "SportCode", New = "Sport Code" },
    new { Old = "SportCodeFromAttribute", New = "Sport Code" },
    new { Old = "Sport", New = "Sport" },
    new { Old = "SportFromAttribute", New = "Sport Name" },
    new { Old = "SportFromFund", New = "Sport From Scholarship" },
    new { Old = "AttributeYear", New = "Attribute Year" },

    // TERM/PERIOD INFORMATION
    new { Old = "Term", New = "Academic Term" },
    new { Old = "POE_DESC", New = "Period Of Enrollment" },
    new { Old = "POE_ID", New = "POE ID" },
    new { Old = "AwardYear", New = "Award Year" },
    new { Old = "PF_AWARD_YR", New = "Financial Aid Award Year" },

    // STUDENT NAME INFORMATION
    new { Old = "FIRST_NAME", New = "First Name" },
    new { Old = "LAST_NAME", New = "Last Name" },
    new { Old = "MIDDLE_NAME", New = "Middle Name" },
    new { Old = "EMAIL_ADDRESS", New = "Email Address" },

    // DEMOGRAPHICS
    new { Old = "GENDER", New = "Gender" },
    new { Old = "BIRTH_DTE", New = "Birth Date" },
    new { Old = "ETHNIC_GROUP", New = "Ethnic Group" },

    // ACADEMIC INFORMATION
    new { Old = "ClassCode", New = "Class Code" },
    new { Old = "CURRENT_CLASS_CDE", New = "Current Class Code" },
    new { Old = "CLASS_CDE", New = "Class Code" },
    new { Old = "ENTRANCE_YR", New = "Entrance Year" },
    new { Old = "ENTRANCE_TRM", New = "Entrance Term" },
    new { Old = "Division", New = "Academic Division" },
    new { Old = "DIV_CDE", New = "Division Code" },

    // GPA AND HOURS
    new { Old = "CAREER_GPA", New = "Career GPA" },
    new { Old = "career_gpa", New = "Career GPA" },
    new { Old = "CAREER_HRS_EARNED", New = "Career Hours Earned" },
    new { Old = "CAREER_HRS_ATTEMPT", New = "Career Hours Attempted" },
    new { Old = "AvgCareerGPA", New = "Average Career GPA" },
    new { Old = "AvgHoursEarned", New = "Average Hours Earned" },
    new { Old = "AvgHoursAttempted", New = "Average Hours Attempted" },

    // SCHOLARSHIP INFORMATION
    new { Old = "FUND_CDE", New = "Fund Code" },
    new { Old = "FundDescription", New = "Fund Description" },
    new { Old = "TOTAL_AWARDED", New = "Total Amount Awarded" },
    new { Old = "TOTAL_EARNED", New = "Total Amount Earned" },
    new { Old = "AmountAwarded", New = "Amount Awarded" },
    new { Old = "AmountEarned", New = "Amount Earned" },
    new { Old = "TotalScholarshipAwarded", New = "Total Scholarship Awarded" },
    new { Old = "NumberOfScholarships", New = "Number Of Scholarships" },
    new { Old = "ScholarshipCount", New = "Scholarship Count" },
    new { Old = "RELEASE_STS", New = "Release Status" },
    new { Old = "ReleaseStatus", New = "Release Status" },
    new { Old = "ScholarshipStatus", New = "Scholarship Status" },

    // SCHOLARSHIP SUMMARY FIELDS
    new { Old = "TotalAthletes", New = "Total Athletes" },
    new { Old = "MaleAthletes", New = "Male Athletes" },
    new { Old = "FemaleAthletes", New = "Female Athletes" },
    new { Old = "TotalScholarshipAthletes", New = "Total Scholarship Athletes" },
    new { Old = "TotalScholarshipDollars", New = "Total Scholarship Dollars" },
    new { Old = "AvgScholarshipAmount", New = "Average Scholarship Amount" },
    new { Old = "MinScholarship", New = "Minimum Scholarship" },
    new { Old = "MaxScholarship", New = "Maximum Scholarship" },
    new { Old = "TotalStudents", New = "Total Students" },

    // ACADEMIC PERFORMANCE BREAKDOWN
    new { Old = "Students_GPA_Above_3_5", New = "Students GPA Above 3.5" },
    new { Old = "Students_GPA_3_0_to_3_5", New = "Students GPA 3.0 to 3.5" },
    new { Old = "Students_GPA_2_5_to_3_0", New = "Students GPA 2.5 to 3.0" },
    new { Old = "Students_GPA_Below_2_5", New = "Students GPA Below 2.5" },

    // SYSTEM FIELDS
    new { Old = "USER_NAME", New = "User Name" },
    new { Old = "JOB_NAME", New = "Job Name" },
    new { Old = "JOB_TIME", New = "Job Time" },
    new { Old = "APPROWVERSION", New = "App Row Version" },
    new { Old = "AppRowVersion", New = "App Row Version" },
    new { Old = "ChangeUser", New = "Change User" },
    new { Old = "ChangeJob", New = "Change Job" },
    new { Old = "ChangeTime", New = "Change Time" }
};

// Apply renames to all tables
foreach (var table in Model.Tables)
{
    foreach (var r in renames)
    {
        var column = table.Columns.FirstOrDefault(c => c.Name == r.Old);
        if (column != null)
        {
            column.Name = r.New;
        }
    }
}

// Output completion message
Info("Column rename script completed successfully for Wiley University Athletics Dashboard!");
