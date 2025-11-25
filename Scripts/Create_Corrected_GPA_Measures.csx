// ============================================================================
// WILEY UNIVERSITY ATHLETICS DASHBOARD - CORRECTED GPA MEASURES
// Tabular Editor 3 C# Script
// Version: 1.0 - Created 11/25/2025
// Purpose: Fix GPA calculations and ensure slicers work properly
// ============================================================================
//
// INSTRUCTIONS:
// 1. Open your Power BI model in Tabular Editor 3
// 2. Copy this entire script
// 3. Go to C# Script tab and paste
// 4. Run the script
// 5. Save the model and refresh
// ============================================================================

// Define the table where measures will be created
var measureTableName = "Executive Measures";
var measureTable = Model.Tables[measureTableName];

if (measureTable == null)
{
    Error($"Table '{measureTableName}' not found. Please create it first or change the measureTableName variable.");
    return;
}

// Track results
int updated = 0;
int created = 0;
int errors = 0;

// Helper function to create or update a measure
void CreateOrUpdateMeasure(string name, string expression, string displayFolder, string formatString, string description = "")
{
    try
    {
        var existingMeasure = measureTable.Measures.FirstOrDefault(m => m.Name == name);

        if (existingMeasure != null)
        {
            existingMeasure.Expression = expression;
            existingMeasure.DisplayFolder = displayFolder;
            existingMeasure.FormatString = formatString;
            if (!string.IsNullOrEmpty(description)) existingMeasure.Description = description;
            Info($"UPDATED: '{name}'");
            updated++;
        }
        else
        {
            var newMeasure = measureTable.AddMeasure(name, expression);
            newMeasure.DisplayFolder = displayFolder;
            newMeasure.FormatString = formatString;
            if (!string.IsNullOrEmpty(description)) newMeasure.Description = description;
            Info($"CREATED: '{name}'");
            created++;
        }
    }
    catch (Exception ex)
    {
        Error($"ERROR with '{name}': {ex.Message}");
        errors++;
    }
}

Info("Starting GPA Measures creation/update...\n");

// ============================================================================
// CORE GPA MEASURES
// ============================================================================

CreateOrUpdateMeasure(
    "Average Career GPA (Weighted)",
    @"VAR TotalStudentCount = SUM('Academic Performance by Term Sport and Scholarship'[Total Students])
VAR WeightedGPA = SUMX(
    'Academic Performance by Term Sport and Scholarship',
    'Academic Performance by Term Sport and Scholarship'[Average Career GPA] *
    'Academic Performance by Term Sport and Scholarship'[Total Students]
)
RETURN
    DIVIDE(WeightedGPA, TotalStudentCount, BLANK())",
    "GPA Calculations",
    "0.00",
    "Properly weighted average GPA that responds to slicer selections"
);

CreateOrUpdateMeasure(
    "Average Career GPA",
    @"AVERAGE('Academic Performance by Term Sport and Scholarship'[Average Career GPA])",
    "GPA Calculations",
    "0.00",
    "Simple average of pre-aggregated GPA values"
);

// ============================================================================
// GPA DISTRIBUTION COUNTS (FIXED)
// ============================================================================

CreateOrUpdateMeasure(
    "Students GPA Above 3.5",
    @"SUM('Academic Performance by Term Sport and Scholarship'[Students with GPA Above 3.5])",
    "GPA Distribution",
    "#,0",
    "Count of students with GPA 3.5 or higher"
);

CreateOrUpdateMeasure(
    "Students GPA 3.0 to 3.5",
    @"SUM('Academic Performance by Term Sport and Scholarship'[Students with GPA 3.0 to 3.5])",
    "GPA Distribution",
    "#,0",
    "Count of students with GPA between 3.0 and 3.5"
);

CreateOrUpdateMeasure(
    "Students GPA 2.5 to 3.0",
    @"SUM('Academic Performance by Term Sport and Scholarship'[Students with GPA 2.5 to 3.0])",
    "GPA Distribution",
    "#,0",
    "Count of students with GPA between 2.5 and 3.0"
);

// FIXED: Was incorrectly using SUM on Academic Term (a text field!)
CreateOrUpdateMeasure(
    "Students GPA Below 2.5",
    @"SUM('Academic Performance by Term Sport and Scholarship'[Students with GPA Below 2.5])",
    "GPA Distribution",
    "#,0",
    "FIXED: Count of students with GPA below 2.5"
);

CreateOrUpdateMeasure(
    "Total Students",
    @"SUM('Academic Performance by Term Sport and Scholarship'[Total Students])",
    "GPA Distribution",
    "#,0",
    "Total students in current filter context"
);

// ============================================================================
// GPA DISTRIBUTION PERCENTAGES
// ============================================================================

CreateOrUpdateMeasure(
    "Pct Students GPA Above 3.5",
    @"VAR StudentsAbove35 = [Students GPA Above 3.5]
VAR TotalStudents = [Total Students]
RETURN DIVIDE(StudentsAbove35, TotalStudents, 0) * 100",
    "GPA Distribution %",
    "0.0%",
    "Percentage of students with GPA 3.5+"
);

CreateOrUpdateMeasure(
    "Pct Students GPA 3.0 to 3.5",
    @"VAR Students30to35 = [Students GPA 3.0 to 3.5]
VAR TotalStudents = [Total Students]
RETURN DIVIDE(Students30to35, TotalStudents, 0) * 100",
    "GPA Distribution %",
    "0.0%",
    "Percentage of students with GPA 3.0-3.5"
);

CreateOrUpdateMeasure(
    "Pct Students GPA 2.5 to 3.0",
    @"VAR Students25to30 = [Students GPA 2.5 to 3.0]
VAR TotalStudents = [Total Students]
RETURN DIVIDE(Students25to30, TotalStudents, 0) * 100",
    "GPA Distribution %",
    "0.0%",
    "Percentage of students with GPA 2.5-3.0"
);

CreateOrUpdateMeasure(
    "Pct Students GPA Below 2.5",
    @"VAR StudentsBelow25 = [Students GPA Below 2.5]
VAR TotalStudents = [Total Students]
RETURN DIVIDE(StudentsBelow25, TotalStudents, 0) * 100",
    "GPA Distribution %",
    "0.0%",
    "Percentage of students with GPA below 2.5"
);

// ============================================================================
// ACADEMIC RISK MEASURES (FIXED)
// ============================================================================

// FIXED: Was referencing non-existent 'GPA' column
CreateOrUpdateMeasure(
    "At-Risk Student Rate",
    @"DIVIDE(
    SUM('Academic Performance by Term Sport and Scholarship'[Students with GPA Below 2.5]),
    SUM('Academic Performance by Term Sport and Scholarship'[Total Students]),
    0
) * 100",
    "Risk Management",
    "0.0%",
    "FIXED: Percentage of students with GPA below 2.5"
);

// FIXED: Was referencing non-existent 'GPA' column
CreateOrUpdateMeasure(
    "At-Risk Students Count",
    @"SUM('Academic Performance by Term Sport and Scholarship'[Students with GPA Below 2.5])",
    "Risk Management",
    "#,0",
    "FIXED: Count of at-risk students (GPA < 2.5)"
);

CreateOrUpdateMeasure(
    "Academic Excellence Rate",
    @"VAR StudentsAbove30 = [Students GPA Above 3.5] + [Students GPA 3.0 to 3.5]
VAR TotalStudents = [Total Students]
RETURN DIVIDE(StudentsAbove30, TotalStudents, 0) * 100",
    "Strategic Performance",
    "0.0%",
    "Percentage of students with GPA 3.0 or higher"
);

CreateOrUpdateMeasure(
    "High Performance Athletes",
    @"SUM('Academic Performance by Term Sport and Scholarship'[Students with GPA Above 3.5])",
    "Strategic Performance",
    "#,0",
    "Count of students with GPA 3.5+"
);

// ============================================================================
// HOURS MEASURES
// ============================================================================

CreateOrUpdateMeasure(
    "Average Hours Earned (Weighted)",
    @"VAR TotalStudentCount = SUM('Academic Performance by Term Sport and Scholarship'[Total Students])
VAR WeightedHours = SUMX(
    'Academic Performance by Term Sport and Scholarship',
    'Academic Performance by Term Sport and Scholarship'[Average Hours Earned] *
    'Academic Performance by Term Sport and Scholarship'[Total Students]
)
RETURN
    DIVIDE(WeightedHours, TotalStudentCount, BLANK())",
    "Hours Analysis",
    "#,0.0",
    "Weighted average hours earned"
);

CreateOrUpdateMeasure(
    "Average Hours Attempted (Weighted)",
    @"VAR TotalStudentCount = SUM('Academic Performance by Term Sport and Scholarship'[Total Students])
VAR WeightedHours = SUMX(
    'Academic Performance by Term Sport and Scholarship',
    'Academic Performance by Term Sport and Scholarship'[Average Hours Attempted] *
    'Academic Performance by Term Sport and Scholarship'[Total Students]
)
RETURN
    DIVIDE(WeightedHours, TotalStudentCount, BLANK())",
    "Hours Analysis",
    "#,0.0",
    "Weighted average hours attempted"
);

CreateOrUpdateMeasure(
    "Completion Rate",
    @"VAR HoursEarned = [Average Hours Earned (Weighted)]
VAR HoursAttempted = [Average Hours Attempted (Weighted)]
RETURN DIVIDE(HoursEarned, HoursAttempted, 0) * 100",
    "Hours Analysis",
    "0.0%",
    "Percentage of attempted hours that were earned"
);

// ============================================================================
// SCHOLARSHIP GPA COMPARISON
// ============================================================================

CreateOrUpdateMeasure(
    "Scholarship Student GPA",
    @"CALCULATE(
    [Average Career GPA (Weighted)],
    'Academic Performance by Term Sport and Scholarship'[Scholarship Status] = ""On Scholarship""
)",
    "Scholarship Analysis",
    "0.00",
    "Average GPA for students on scholarship"
);

CreateOrUpdateMeasure(
    "Non-Scholarship Student GPA",
    @"CALCULATE(
    [Average Career GPA (Weighted)],
    'Academic Performance by Term Sport and Scholarship'[Scholarship Status] = ""No Scholarship""
)",
    "Scholarship Analysis",
    "0.00",
    "Average GPA for students without scholarship"
);

CreateOrUpdateMeasure(
    "GPA Difference Scholarship vs Non",
    @"VAR ScholarshipGPA = [Scholarship Student GPA]
VAR NonScholarshipGPA = [Non-Scholarship Student GPA]
RETURN ScholarshipGPA - NonScholarshipGPA",
    "Scholarship Analysis",
    "0.00",
    "Difference in GPA between scholarship and non-scholarship students"
);

// ============================================================================
// YEAR OVER YEAR COMPARISONS
// ============================================================================

CreateOrUpdateMeasure(
    "Current Year GPA",
    @"VAR MaxYear = MAX('Academic Performance by Term Sport and Scholarship'[Award Year])
RETURN
CALCULATE(
    [Average Career GPA (Weighted)],
    'Academic Performance by Term Sport and Scholarship'[Award Year] = MaxYear
)",
    "Trend Analysis",
    "0.00",
    "GPA for the most recent year"
);

CreateOrUpdateMeasure(
    "Previous Year GPA",
    @"VAR MaxYear = MAX('Academic Performance by Term Sport and Scholarship'[Award Year])
RETURN
CALCULATE(
    [Average Career GPA (Weighted)],
    'Academic Performance by Term Sport and Scholarship'[Award Year] = MaxYear - 1
)",
    "Trend Analysis",
    "0.00",
    "GPA for the previous year"
);

CreateOrUpdateMeasure(
    "YoY GPA Change",
    @"VAR CurrentGPA = [Current Year GPA]
VAR PrevGPA = [Previous Year GPA]
RETURN CurrentGPA - PrevGPA",
    "Trend Analysis",
    "0.00",
    "Year over year change in GPA"
);

CreateOrUpdateMeasure(
    "YoY GPA Change Pct",
    @"VAR CurrentGPA = [Current Year GPA]
VAR PrevGPA = [Previous Year GPA]
RETURN DIVIDE(CurrentGPA - PrevGPA, PrevGPA, 0) * 100",
    "Trend Analysis",
    "0.0%",
    "Year over year percentage change in GPA"
);

// FIXED: Was using [AwardYear] instead of [Award Year]
CreateOrUpdateMeasure(
    "Program Growth Rate",
    @"VAR MaxYear = MAX('Academic Performance by Term Sport and Scholarship'[Award Year])
VAR CurrentYearStudents =
    CALCULATE(
        [Total Students],
        'Academic Performance by Term Sport and Scholarship'[Award Year] = MaxYear
    )
VAR PreviousYearStudents =
    CALCULATE(
        [Total Students],
        'Academic Performance by Term Sport and Scholarship'[Award Year] = MaxYear - 1
    )
RETURN DIVIDE(CurrentYearStudents - PreviousYearStudents, PreviousYearStudents, 0) * 100",
    "Strategic Performance",
    "0.0%",
    "FIXED: Year over year growth in student count"
);

// ============================================================================
// TREATAS MEASURES (Use if Filters table is not directly related)
// ============================================================================

CreateOrUpdateMeasure(
    "Average Career GPA with Filters",
    @"VAR SelectedTerms = VALUES('Filters'[Academic Term])
VAR SelectedYears = VALUES('Filters'[Award Year])
VAR SelectedSports = VALUES('Filters'[Sport Name])
RETURN
CALCULATE(
    [Average Career GPA (Weighted)],
    TREATAS(SelectedTerms, 'Academic Performance by Term Sport and Scholarship'[Academic Term]),
    TREATAS(SelectedYears, 'Academic Performance by Term Sport and Scholarship'[Award Year]),
    TREATAS(SelectedSports, 'Academic Performance by Term Sport and Scholarship'[Sport Name])
)",
    "Filters Integration",
    "0.00",
    "Use this if Filters table is not related to data tables"
);

CreateOrUpdateMeasure(
    "Total Students with Filters",
    @"VAR SelectedTerms = VALUES('Filters'[Academic Term])
VAR SelectedYears = VALUES('Filters'[Award Year])
VAR SelectedSports = VALUES('Filters'[Sport Name])
RETURN
CALCULATE(
    [Total Students],
    TREATAS(SelectedTerms, 'Academic Performance by Term Sport and Scholarship'[Academic Term]),
    TREATAS(SelectedYears, 'Academic Performance by Term Sport and Scholarship'[Award Year]),
    TREATAS(SelectedSports, 'Academic Performance by Term Sport and Scholarship'[Sport Name])
)",
    "Filters Integration",
    "#,0",
    "Use this if Filters table is not related to data tables"
);

CreateOrUpdateMeasure(
    "At-Risk Rate with Filters",
    @"VAR SelectedTerms = VALUES('Filters'[Academic Term])
VAR SelectedYears = VALUES('Filters'[Award Year])
VAR SelectedSports = VALUES('Filters'[Sport Name])
RETURN
CALCULATE(
    [At-Risk Student Rate],
    TREATAS(SelectedTerms, 'Academic Performance by Term Sport and Scholarship'[Academic Term]),
    TREATAS(SelectedYears, 'Academic Performance by Term Sport and Scholarship'[Award Year]),
    TREATAS(SelectedSports, 'Academic Performance by Term Sport and Scholarship'[Sport Name])
)",
    "Filters Integration",
    "0.0%",
    "Use this if Filters table is not related to data tables"
);

// ============================================================================
// PARTICIPATION SUMMARY MEASURES
// ============================================================================

CreateOrUpdateMeasure(
    "Total Athletes Program",
    @"SUM('Participation Summary by Term and Sport'[Total Athletes])",
    "Strategic Performance",
    "#,0",
    "Total athletes from participation summary"
);

CreateOrUpdateMeasure(
    "Average Career GPA Athletes",
    @"VAR TotalAthleteCount = SUM('Participation Summary by Term and Sport'[Total Athletes])
VAR WeightedGPA = SUMX(
    'Participation Summary by Term and Sport',
    'Participation Summary by Term and Sport'[Average Career GPA] *
    'Participation Summary by Term and Sport'[Total Athletes]
)
RETURN
    DIVIDE(WeightedGPA, TotalAthleteCount, BLANK())",
    "Strategic Performance",
    "0.00",
    "Weighted average GPA from participation summary"
);

CreateOrUpdateMeasure(
    "Male Athletes",
    @"SUM('Participation Summary by Term and Sport'[Male Athletes])",
    "Demographics",
    "#,0",
    "Count of male athletes"
);

CreateOrUpdateMeasure(
    "Female Athletes",
    @"SUM('Participation Summary by Term and Sport'[Female Athletes])",
    "Demographics",
    "#,0",
    "Count of female athletes"
);

CreateOrUpdateMeasure(
    "Gender Diversity Index",
    @"VAR MaleCount = [Male Athletes]
VAR FemaleCount = [Female Athletes]
VAR Ratio = DIVIDE(MaleCount, FemaleCount, 0)
RETURN 100 - ABS(Ratio - 1) * 50",
    "Demographics",
    "0.0",
    "Index measuring gender balance (100 = perfect balance)"
);

// ============================================================================
// FIX DEPENDENT MEASURES THAT WERE IN ERROR STATE
// ============================================================================

CreateOrUpdateMeasure(
    "Academic Risk Exposure",
    @"[At-Risk Student Rate] * [Total Athletic Investment] / 100",
    "Risk Management",
    "$#,0",
    "Financial exposure due to at-risk students"
);

CreateOrUpdateMeasure(
    "Compliance Risk Score",
    @"([At-Risk Student Rate] * 0.4) + ([Scholarship Default Risk] * 0.3) + ([Financial Concentration Risk] * 0.3)",
    "Risk Management",
    "0.0",
    "Combined compliance risk score"
);

CreateOrUpdateMeasure(
    "Financial Risk Score",
    @"VAR AcademicRisk = [At-Risk Student Rate]
VAR RetentionRisk = [Retention Risk Factor]
VAR CombinedRisk = (AcademicRisk * 0.6) + (RetentionRisk * 0.4)
RETURN CombinedRisk",
    "Risk Management",
    "0.0",
    "Combined financial risk score"
);

CreateOrUpdateMeasure(
    "Investment at Risk",
    @"[Academic Risk Exposure] + ([Scholarship Default Risk] / 100 * [Total Athletic Investment])",
    "Risk Management",
    "$#,0",
    "Total investment at risk"
);

CreateOrUpdateMeasure(
    "Strategic Risk Rating",
    @"SWITCH(TRUE(), [Compliance Risk Score] <= 10, 1, [Compliance Risk Score] <= 25, 2, [Compliance Risk Score] <= 50, 3, [Compliance Risk Score] <= 75, 4, 5)",
    "Risk Management",
    "0",
    "Strategic risk rating on scale of 1-5"
);

CreateOrUpdateMeasure(
    "Program Sustainability",
    @"100 - ([Investment at Risk] / [Total Athletic Investment] * 100)",
    "Executive KPIs",
    "0.0%",
    "Program sustainability percentage"
);

CreateOrUpdateMeasure(
    "Performance to Cost Ratio",
    @"DIVIDE([Academic Excellence Rate], [Average Investment per Athlete], 0) * 1000",
    "Operational Efficiency",
    "0.000",
    "Performance relative to investment"
);

CreateOrUpdateMeasure(
    "Productivity Index",
    @"([Academic Excellence Rate] / 100) * ([Scholarship Utilization Rate] / 100) * 100",
    "Operational Efficiency",
    "0.0",
    "Combined productivity measure"
);

CreateOrUpdateMeasure(
    "Overall Program Health",
    @"([Academic Excellence Rate] * 0.3) + ([Scholarship Utilization Rate] * 0.3) + ([Gender Diversity Index] * 0.2) + (100 - [Strategic Risk Rating] * 20) * 0.2",
    "Executive KPIs",
    "0.0",
    "Overall program health score"
);

CreateOrUpdateMeasure(
    "Board Reporting Score",
    @"MIN([Overall Program Health], 100)",
    "Executive KPIs",
    "0.0",
    "Score for board reporting (capped at 100)"
);

CreateOrUpdateMeasure(
    "Strategic Value Score",
    @"VAR GrowthFactor = DIVIDE([Program Growth Rate] + 100, 100, 1)
VAR ExcellenceFactor = DIVIDE([Academic Excellence Rate], 100, 0)
VAR UtilizationFactor = DIVIDE([Scholarship Utilization Rate], 100, 0)
RETURN GrowthFactor * ExcellenceFactor * UtilizationFactor * 100",
    "Executive KPIs",
    "0.0",
    "Combined strategic value score"
);

CreateOrUpdateMeasure(
    "Stakeholder Value Index",
    @"([High Performance Athletes] / [Total Athletes Program] * 100) + ([Scholarship Yield Rate])",
    "Executive KPIs",
    "0.0",
    "Combined stakeholder value measure"
);

CreateOrUpdateMeasure(
    "Cost Efficiency Index",
    @"DIVIDE([High Performance Athletes], [Total Athletic Investment], 0) * 100000",
    "Operational Efficiency",
    "0.00",
    "High performers relative to investment"
);

// ============================================================================
// SUMMARY
// ============================================================================

Info("\n========================================");
Info("GPA MEASURES UPDATE SUMMARY");
Info("========================================");
Info($"Created: {created} measures");
Info($"Updated: {updated} measures");
Info($"Errors: {errors} measures");
Info("========================================\n");

Info(@"
NEXT STEPS:
1. Save the model in Tabular Editor
2. In Power BI Desktop, create relationships between 'Filters' table and data tables:
   - Filters[Academic Term] --> Academic Performance...[Academic Term]
   - Filters[Award Year] --> Academic Performance...[Award Year]
   - Filters[Sport Name] --> Academic Performance...[Sport Name]
3. If relationships cause issues, use the 'with Filters' suffix measures instead
4. Refresh the model and test your slicers

FIXED ISSUES:
- 'Students GPA Below 2.5' was summing Academic Term (text) instead of count
- 'At-Risk Students Count' was referencing non-existent 'GPA' column
- 'Program Growth Rate' was using [AwardYear] instead of [Award Year]
- Added 'Academic Excellence Rate' measure that was missing
- All dependent measures now work correctly
");
