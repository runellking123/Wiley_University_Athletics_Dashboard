// ============================================================================
// TABULAR EDITOR 3 C# SCRIPT - CREATE EXECUTIVE DASHBOARD MEASURES
// ============================================================================
// Instructions:
//   1. Open your Power BI model in Tabular Editor 3
//   2. Go to File > Open > From DB or open your .bim file
//   3. Press Ctrl+Shift+P or go to Tools > Execute Script
//   4. Paste this script and run (F5)
//   5. Save changes to your model
// ============================================================================

using System.Collections.Generic;

// Configuration - Set your target table name for measures
// Change this if you want measures in a different table
string measuresTableName = "Measures";

// Track results
int measuresCreated = 0;
int measuresUpdated = 0;
int errors = 0;
var errorMessages = new List<string>();

// Helper function to create or update a measure
Action<string, string, string, string> CreateMeasure = (name, expression, displayFolder, formatString) =>
{
    try
    {
        Measure m;

        // Check if measure already exists in any table
        m = Model.AllMeasures.FirstOrDefault(x => x.Name == name);

        if (m == null)
        {
            // Find or create the measures table
            Table targetTable = Model.Tables.FirstOrDefault(t => t.Name == measuresTableName);

            if (targetTable == null)
            {
                // If measures table doesn't exist, use the first table in the model
                targetTable = Model.Tables.FirstOrDefault();
                if (targetTable == null)
                {
                    errorMessages.Add("No tables found in model. Please ensure data is loaded.");
                    errors++;
                    return;
                }
            }

            m = targetTable.AddMeasure(name);
            measuresCreated++;
        }
        else
        {
            measuresUpdated++;
        }

        m.Expression = expression;
        m.DisplayFolder = displayFolder;

        if (!string.IsNullOrEmpty(formatString))
        {
            m.FormatString = formatString;
        }
    }
    catch (Exception ex)
    {
        errorMessages.Add($"Error creating measure '{name}': {ex.Message}");
        errors++;
    }
};

// ============================================================================
// SECTION 1: SCHOLARSHIP METRICS
// ============================================================================

CreateMeasure(
    "Total Scholarship Dollars",
    @"SUM(Scholarship_Summary_By_Term_Sport[TotalScholarshipDollars])",
    "1. Scholarship Metrics",
    @"""$""#,##0.00"
);

CreateMeasure(
    "Total Scholarship Athletes",
    @"SUM(Scholarship_Summary_By_Term_Sport[TotalScholarshipAthletes])",
    "1. Scholarship Metrics",
    "#,##0"
);

CreateMeasure(
    "Avg Scholarship Per Athlete",
    @"DIVIDE(
    [Total Scholarship Dollars],
    [Total Scholarship Athletes],
    0
)",
    "1. Scholarship Metrics",
    @"""$""#,##0.00"
);

CreateMeasure(
    "Max Scholarship Awarded",
    @"MAX(Scholarship_Summary_By_Term_Sport[MaxScholarship])",
    "1. Scholarship Metrics",
    @"""$""#,##0.00"
);

CreateMeasure(
    "Min Scholarship Awarded",
    @"CALCULATE(
    MIN(Scholarship_Summary_By_Term_Sport[MinScholarship]),
    Scholarship_Summary_By_Term_Sport[MinScholarship] > 0
)",
    "1. Scholarship Metrics",
    @"""$""#,##0.00"
);

CreateMeasure(
    "Total Scholarships Detail",
    @"SUM(Athletic_Scholarships_By_Term_2017_2025[AmountAwarded])",
    "1. Scholarship Metrics",
    @"""$""#,##0.00"
);

CreateMeasure(
    "Count of Scholarship Awards",
    @"COUNTROWS(Athletic_Scholarships_By_Term_2017_2025)",
    "1. Scholarship Metrics",
    "#,##0"
);

CreateMeasure(
    "Distinct Scholarship Recipients",
    @"DISTINCTCOUNT(Athletes_With_Scholarships_By_Term_2017_2025[ID_NUM])",
    "1. Scholarship Metrics",
    "#,##0"
);

CreateMeasure(
    "Scholarship Coverage Rate",
    @"VAR AthletesWithScholarship =
    CALCULATE(
        DISTINCTCOUNT(Athletes_With_Scholarships_By_Term_2017_2025[ID_NUM]),
        Athletes_With_Scholarships_By_Term_2017_2025[TotalScholarshipAwarded] > 0
    )
VAR TotalAthletes =
    DISTINCTCOUNT(Athletes_With_Scholarships_By_Term_2017_2025[ID_NUM])
RETURN
    DIVIDE(AthletesWithScholarship, TotalAthletes, 0)",
    "1. Scholarship Metrics",
    "0.0%"
);

// ============================================================================
// SECTION 2: PARTICIPATION METRICS
// ============================================================================

CreateMeasure(
    "Total Athletes",
    @"DISTINCTCOUNT(Athletes_Master_By_Term_2017_2025[ID_NUM])",
    "2. Participation Metrics",
    "#,##0"
);

CreateMeasure(
    "Total Male Athletes",
    @"CALCULATE(
    DISTINCTCOUNT(Athletes_Master_By_Term_2017_2025[ID_NUM]),
    Athletes_Master_By_Term_2017_2025[Sex] = ""M""
)",
    "2. Participation Metrics",
    "#,##0"
);

CreateMeasure(
    "Total Female Athletes",
    @"CALCULATE(
    DISTINCTCOUNT(Athletes_Master_By_Term_2017_2025[ID_NUM]),
    Athletes_Master_By_Term_2017_2025[Sex] = ""F""
)",
    "2. Participation Metrics",
    "#,##0"
);

CreateMeasure(
    "Male Athlete Pct",
    @"DIVIDE(
    [Total Male Athletes],
    [Total Athletes],
    0
)",
    "2. Participation Metrics",
    "0.0%"
);

CreateMeasure(
    "Female Athlete Pct",
    @"DIVIDE(
    [Total Female Athletes],
    [Total Athletes],
    0
)",
    "2. Participation Metrics",
    "0.0%"
);

CreateMeasure(
    "Count of Sports",
    @"DISTINCTCOUNT(Athletes_Master_By_Term_2017_2025[Sport])",
    "2. Participation Metrics",
    "#,##0"
);

CreateMeasure(
    "Athletes by Sport",
    @"DISTINCTCOUNT(Athletes_Master_By_Term_2017_2025[ID_NUM])",
    "2. Participation Metrics",
    "#,##0"
);

CreateMeasure(
    "Unique Athletes Master",
    @"DISTINCTCOUNT(Athletes_Master_By_Term_2017_2025[ID_NUM])",
    "2. Participation Metrics",
    "#,##0"
);

// ============================================================================
// SECTION 3: ACADEMIC PERFORMANCE METRICS
// ============================================================================

CreateMeasure(
    "Total Academic Students",
    @"DISTINCTCOUNT(Athletes_Master_By_Term_2017_2025[ID_NUM])",
    "3. Academic Performance",
    "#,##0"
);

CreateMeasure(
    "Students GPA Above 3.5",
    @"CALCULATE(
    DISTINCTCOUNT(Athletes_Master_By_Term_2017_2025[ID_NUM]),
    Athletes_Master_By_Term_2017_2025[CAREER_GPA] >= 3.5
)",
    "3. Academic Performance",
    "#,##0"
);

CreateMeasure(
    "Students GPA 3.0 to 3.5",
    @"CALCULATE(
    DISTINCTCOUNT(Athletes_Master_By_Term_2017_2025[ID_NUM]),
    Athletes_Master_By_Term_2017_2025[CAREER_GPA] >= 3.0,
    Athletes_Master_By_Term_2017_2025[CAREER_GPA] < 3.5
)",
    "3. Academic Performance",
    "#,##0"
);

CreateMeasure(
    "Students GPA 2.5 to 3.0",
    @"CALCULATE(
    DISTINCTCOUNT(Athletes_Master_By_Term_2017_2025[ID_NUM]),
    Athletes_Master_By_Term_2017_2025[CAREER_GPA] >= 2.5,
    Athletes_Master_By_Term_2017_2025[CAREER_GPA] < 3.0
)",
    "3. Academic Performance",
    "#,##0"
);

CreateMeasure(
    "Students GPA Below 2.5",
    @"CALCULATE(
    DISTINCTCOUNT(Athletes_Master_By_Term_2017_2025[ID_NUM]),
    Athletes_Master_By_Term_2017_2025[CAREER_GPA] < 2.5,
    Athletes_Master_By_Term_2017_2025[CAREER_GPA] > 0
)",
    "3. Academic Performance",
    "#,##0"
);

CreateMeasure(
    "Academic Excellence Rate",
    @"VAR ExcellentStudents = [Students GPA Above 3.5] + [Students GPA 3.0 to 3.5]
VAR TotalStudents = [Total Academic Students]
RETURN
    DIVIDE(ExcellentStudents, TotalStudents, 0)",
    "3. Academic Performance",
    "0.0%"
);

CreateMeasure(
    "Academic Risk Rate",
    @"DIVIDE(
    [Students GPA Below 2.5],
    [Total Academic Students],
    0
)",
    "3. Academic Performance",
    "0.0%"
);

CreateMeasure(
    "Weighted Avg GPA",
    @"AVERAGEX(
    SUMMARIZE(
        Athletes_Master_By_Term_2017_2025,
        Athletes_Master_By_Term_2017_2025[ID_NUM],
        ""LatestGPA"", MAX(Athletes_Master_By_Term_2017_2025[CAREER_GPA])
    ),
    [LatestGPA]
)",
    "3. Academic Performance",
    "0.00"
);

CreateMeasure(
    "Avg Hours Earned",
    @"AVERAGEX(
    SUMMARIZE(
        Athletes_Master_By_Term_2017_2025,
        Athletes_Master_By_Term_2017_2025[ID_NUM],
        ""LatestHours"", MAX(Athletes_Master_By_Term_2017_2025[CAREER_HRS_EARNED])
    ),
    [LatestHours]
)",
    "3. Academic Performance",
    "0.0"
);

CreateMeasure(
    "Scholarship Student GPA",
    @"CALCULATE(
    AVERAGE(Academic_Performance_By_Term_Sport_Scholarship[AvgCareerGPA]),
    Academic_Performance_By_Term_Sport_Scholarship[ScholarshipStatus] = ""On Scholarship""
)",
    "3. Academic Performance",
    "0.00"
);

CreateMeasure(
    "Non-Scholarship Student GPA",
    @"CALCULATE(
    AVERAGE(Academic_Performance_By_Term_Sport_Scholarship[AvgCareerGPA]),
    Academic_Performance_By_Term_Sport_Scholarship[ScholarshipStatus] = ""No Scholarship""
)",
    "3. Academic Performance",
    "0.00"
);

// ============================================================================
// SECTION 4: TREND ANALYSIS & COMPARISONS
// ============================================================================

CreateMeasure(
    "PY Scholarship Dollars",
    @"CALCULATE(
    [Total Scholarship Dollars],
    FILTER(
        ALL(Scholarship_Summary_By_Term_Sport[AwardYear]),
        Scholarship_Summary_By_Term_Sport[AwardYear] = MAX(Scholarship_Summary_By_Term_Sport[AwardYear]) - 1
    )
)",
    "4. Trend Analysis",
    @"""$""#,##0.00"
);

CreateMeasure(
    "YoY Scholarship Change",
    @"[Total Scholarship Dollars] - [PY Scholarship Dollars]",
    "4. Trend Analysis",
    @"""$""#,##0.00;""$""-#,##0.00"
);

CreateMeasure(
    "YoY Scholarship Change Pct",
    @"DIVIDE(
    [YoY Scholarship Change],
    [PY Scholarship Dollars],
    0
)",
    "4. Trend Analysis",
    "+0.0%;-0.0%;0.0%"
);

CreateMeasure(
    "PY Total Athletes",
    @"CALCULATE(
    [Total Athletes],
    FILTER(
        ALL(Participation_Summary_By_Term_Sport[AwardYear]),
        Participation_Summary_By_Term_Sport[AwardYear] = MAX(Participation_Summary_By_Term_Sport[AwardYear]) - 1
    )
)",
    "4. Trend Analysis",
    "#,##0"
);

CreateMeasure(
    "YoY Athlete Change",
    @"[Total Athletes] - [PY Total Athletes]",
    "4. Trend Analysis",
    "+#,##0;-#,##0;0"
);

CreateMeasure(
    "YoY Athlete Change Pct",
    @"DIVIDE(
    [YoY Athlete Change],
    [PY Total Athletes],
    0
)",
    "4. Trend Analysis",
    "+0.0%;-0.0%;0.0%"
);

CreateMeasure(
    "Current Award Year",
    @"MAX(Scholarship_Summary_By_Term_Sport[AwardYear])",
    "4. Trend Analysis",
    "0"
);

CreateMeasure(
    "Selected Term",
    @"SELECTEDVALUE(Scholarship_Summary_By_Term_Sport[Term], ""All Terms"")",
    "4. Trend Analysis",
    ""
);

// ============================================================================
// SECTION 5: KPI & EXECUTIVE SUMMARY MEASURES
// ============================================================================

CreateMeasure(
    "Investment Per Athlete",
    @"VAR TotalScholarship = [Total Scholarship Dollars]
VAR Athletes = [Total Athletes]
RETURN
    DIVIDE(TotalScholarship, Athletes, 0)",
    "5. KPI Measures",
    @"""$""#,##0.00"
);

CreateMeasure(
    "Scholarship Utilization",
    @"VAR ScholarshipAthletes = [Total Scholarship Athletes]
VAR AllAthletes = [Total Athletes]
RETURN
    DIVIDE(ScholarshipAthletes, AllAthletes, 0)",
    "5. KPI Measures",
    "0.0%"
);

CreateMeasure(
    "Gender Balance Index",
    @"ABS(0.5 - [Female Athlete Pct])",
    "5. KPI Measures",
    "0.00"
);

CreateMeasure(
    "Academic Success Score",
    @"VAR ExcellenceWeight = 0.4
VAR GoodStandingWeight = 0.3
VAR SatisfactoryWeight = 0.2
VAR AtRiskWeight = 0.1
RETURN
    ([Students GPA Above 3.5] * 4 * ExcellenceWeight) +
    ([Students GPA 3.0 to 3.5] * 3 * GoodStandingWeight) +
    ([Students GPA 2.5 to 3.0] * 2 * SatisfactoryWeight) +
    ([Students GPA Below 2.5] * 1 * AtRiskWeight)",
    "5. KPI Measures",
    "#,##0.0"
);

CreateMeasure(
    "Scholarship ROI Indicator",
    @"DIVIDE(
    [Weighted Avg GPA] * 1000,
    [Avg Scholarship Per Athlete],
    0
)",
    "5. KPI Measures",
    "0.00"
);

// ============================================================================
// SECTION 6: SPORT-SPECIFIC MEASURES
// ============================================================================

CreateMeasure(
    "Baseball Scholarship Total",
    @"CALCULATE(
    [Total Scholarship Dollars],
    Scholarship_Summary_By_Term_Sport[Sport] = ""Baseball""
)",
    "6. Sport Specific",
    @"""$""#,##0.00"
);

CreateMeasure(
    "Basketball Scholarship Total",
    @"CALCULATE(
    [Total Scholarship Dollars],
    Scholarship_Summary_By_Term_Sport[Sport] = ""Basketball""
)",
    "6. Sport Specific",
    @"""$""#,##0.00"
);

CreateMeasure(
    "Soccer Scholarship Total",
    @"CALCULATE(
    [Total Scholarship Dollars],
    Scholarship_Summary_By_Term_Sport[Sport] = ""Soccer""
)",
    "6. Sport Specific",
    @"""$""#,##0.00"
);

CreateMeasure(
    "Track Field Scholarship Total",
    @"CALCULATE(
    [Total Scholarship Dollars],
    Scholarship_Summary_By_Term_Sport[Sport] = ""Track & Field""
)",
    "6. Sport Specific",
    @"""$""#,##0.00"
);

CreateMeasure(
    "Volleyball Scholarship Total",
    @"CALCULATE(
    [Total Scholarship Dollars],
    Scholarship_Summary_By_Term_Sport[Sport] = ""Volleyball""
)",
    "6. Sport Specific",
    @"""$""#,##0.00"
);

CreateMeasure(
    "Top Funded Sport",
    @"VAR SportTotals =
    ADDCOLUMNS(
        VALUES(Scholarship_Summary_By_Term_Sport[Sport]),
        ""SportTotal"", [Total Scholarship Dollars]
    )
RETURN
    MAXX(SportTotals, [SportTotal])",
    "6. Sport Specific",
    @"""$""#,##0.00"
);

// ============================================================================
// SECTION 7: CONDITIONAL FORMATTING MEASURES
// ============================================================================

CreateMeasure(
    "GPA Status",
    @"VAR AvgGPA = [Weighted Avg GPA]
RETURN
    SWITCH(
        TRUE(),
        AvgGPA >= 3.5, ""Excellent"",
        AvgGPA >= 3.0, ""Good"",
        AvgGPA >= 2.5, ""Satisfactory"",
        ""At Risk""
    )",
    "7. Conditional Formatting",
    ""
);

CreateMeasure(
    "Scholarship Trend",
    @"VAR Change = [YoY Scholarship Change Pct]
RETURN
    SWITCH(
        TRUE(),
        Change > 0.1, ""Strong Growth"",
        Change > 0, ""Growth"",
        Change = 0, ""Stable"",
        Change > -0.1, ""Decline"",
        ""Significant Decline""
    )",
    "7. Conditional Formatting",
    ""
);

CreateMeasure(
    "Athlete Trend",
    @"VAR Change = [YoY Athlete Change Pct]
RETURN
    SWITCH(
        TRUE(),
        Change > 0.1, ""Strong Growth"",
        Change > 0, ""Growth"",
        Change = 0, ""Stable"",
        Change > -0.1, ""Decline"",
        ""Significant Decline""
    )",
    "7. Conditional Formatting",
    ""
);

// ============================================================================
// COMPLETION SUMMARY
// ============================================================================

string summaryMessage = $@"
═══════════════════════════════════════════════════════════════
  EXECUTIVE DASHBOARD MEASURES - CREATION COMPLETE
═══════════════════════════════════════════════════════════════

  ✓ Measures Created:  {measuresCreated}
  ✓ Measures Updated:  {measuresUpdated}
  ✗ Errors:            {errors}

───────────────────────────────────────────────────────────────
  MEASURES BY CATEGORY:
───────────────────────────────────────────────────────────────
  1. Scholarship Metrics ............ 9 measures
  2. Participation Metrics .......... 8 measures
  3. Academic Performance ........... 11 measures
  4. Trend Analysis ................. 8 measures
  5. KPI Measures ................... 5 measures
  6. Sport Specific ................. 6 measures
  7. Conditional Formatting ......... 3 measures
───────────────────────────────────────────────────────────────
  TOTAL:                              50 measures

═══════════════════════════════════════════════════════════════
  NEXT STEPS:
  1. Review measures in the Model Explorer
  2. Save your model (Ctrl+S)
  3. Refresh your Power BI report
═══════════════════════════════════════════════════════════════
";

// Display any errors
if (errors > 0)
{
    summaryMessage += "\n  ERRORS:\n";
    foreach (var err in errorMessages)
    {
        summaryMessage += $"  • {err}\n";
    }
}

// Show completion message (single popup at the end)
Info(summaryMessage);
