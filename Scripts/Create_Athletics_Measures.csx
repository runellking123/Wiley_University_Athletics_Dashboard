// WILEY UNIVERSITY ATHLETICS DASHBOARD - MEASURES CREATION SCRIPT
// Creates DAX measures organized by categories for athletics analysis
// Version: 1.0 - Created for Wiley University Athletics Dashboard

// Define measure definitions with category organization
var measures = new[] {

    // ============================================
    // CATEGORY: Athlete Counts
    // ============================================
    new {
        Name = "Total Athletes",
        Expression = @"DISTINCTCOUNT('ATTRIBUTE_TRANS'[ID_NUM])",
        Category = "Athlete Counts",
        FormatString = "#,0"
    },
    new {
        Name = "Male Athletes",
        Expression = @"
CALCULATE(
    DISTINCTCOUNT('ATTRIBUTE_TRANS'[ID_NUM]),
    'BIOGRAPH_MASTER'[GENDER] = ""M""
)",
        Category = "Athlete Counts",
        FormatString = "#,0"
    },
    new {
        Name = "Female Athletes",
        Expression = @"
CALCULATE(
    DISTINCTCOUNT('ATTRIBUTE_TRANS'[ID_NUM]),
    'BIOGRAPH_MASTER'[GENDER] = ""F""
)",
        Category = "Athlete Counts",
        FormatString = "#,0"
    },
    new {
        Name = "Athletes by Sport",
        Expression = @"
CALCULATE(
    DISTINCTCOUNT('ATTRIBUTE_TRANS'[ID_NUM]),
    ALLEXCEPT('ATTRIBUTE_TRANS', 'ATTRIBUTE_TRANS'[ATTRIB_CDE])
)",
        Category = "Athlete Counts",
        FormatString = "#,0"
    },
    new {
        Name = "Athletes by Term",
        Expression = @"
CALCULATE(
    DISTINCTCOUNT('ATTRIBUTE_TRANS'[ID_NUM]),
    ALLEXCEPT('PF_POE_DEF', 'PF_POE_DEF'[POE_DESC])
)",
        Category = "Athlete Counts",
        FormatString = "#,0"
    },

    // ============================================
    // CATEGORY: Academic Performance
    // ============================================
    new {
        Name = "Average GPA",
        Expression = @"AVERAGE('student_master'[CAREER_GPA])",
        Category = "Academic Performance",
        FormatString = "0.00"
    },
    new {
        Name = "Average GPA - Athletes",
        Expression = @"
CALCULATE(
    AVERAGE('student_master'[CAREER_GPA]),
    'ATTRIBUTE_TRANS'[ID_NUM]
)",
        Category = "Academic Performance",
        FormatString = "0.00"
    },
    new {
        Name = "Athletes GPA Above 3.5",
        Expression = @"
CALCULATE(
    DISTINCTCOUNT('ATTRIBUTE_TRANS'[ID_NUM]),
    'student_master'[CAREER_GPA] >= 3.5
)",
        Category = "Academic Performance",
        FormatString = "#,0"
    },
    new {
        Name = "Athletes GPA 3.0 to 3.5",
        Expression = @"
CALCULATE(
    DISTINCTCOUNT('ATTRIBUTE_TRANS'[ID_NUM]),
    'student_master'[CAREER_GPA] >= 3.0,
    'student_master'[CAREER_GPA] < 3.5
)",
        Category = "Academic Performance",
        FormatString = "#,0"
    },
    new {
        Name = "Athletes GPA 2.5 to 3.0",
        Expression = @"
CALCULATE(
    DISTINCTCOUNT('ATTRIBUTE_TRANS'[ID_NUM]),
    'student_master'[CAREER_GPA] >= 2.5,
    'student_master'[CAREER_GPA] < 3.0
)",
        Category = "Academic Performance",
        FormatString = "#,0"
    },
    new {
        Name = "Athletes GPA Below 2.5",
        Expression = @"
CALCULATE(
    DISTINCTCOUNT('ATTRIBUTE_TRANS'[ID_NUM]),
    'student_master'[CAREER_GPA] < 2.5
)",
        Category = "Academic Performance",
        FormatString = "#,0"
    },
    new {
        Name = "Average Hours Earned",
        Expression = @"AVERAGE('student_master'[CAREER_HRS_EARNED])",
        Category = "Academic Performance",
        FormatString = "#,0.0"
    },
    new {
        Name = "Average Hours Attempted",
        Expression = @"AVERAGE('student_master'[CAREER_HRS_ATTEMPT])",
        Category = "Academic Performance",
        FormatString = "#,0.0"
    },
    new {
        Name = "Completion Rate %",
        Expression = @"
DIVIDE(
    AVERAGE('student_master'[CAREER_HRS_EARNED]),
    AVERAGE('student_master'[CAREER_HRS_ATTEMPT]),
    0
) * 100",
        Category = "Academic Performance",
        FormatString = "0.0%"
    },

    // ============================================
    // CATEGORY: Scholarship Analysis
    // ============================================
    new {
        Name = "Total Scholarship Amount",
        Expression = @"SUM('stu_award'[TOTAL_AWARDED])",
        Category = "Scholarship Analysis",
        FormatString = "$#,0"
    },
    new {
        Name = "Total Amount Earned",
        Expression = @"SUM('stu_award'[TOTAL_EARNED])",
        Category = "Scholarship Analysis",
        FormatString = "$#,0"
    },
    new {
        Name = "Average Scholarship Amount",
        Expression = @"AVERAGE('stu_award'[TOTAL_AWARDED])",
        Category = "Scholarship Analysis",
        FormatString = "$#,0"
    },
    new {
        Name = "Total Scholarship Athletes",
        Expression = @"DISTINCTCOUNT('stu_award'[ID_NUM])",
        Category = "Scholarship Analysis",
        FormatString = "#,0"
    },
    new {
        Name = "Athletes with Scholarships",
        Expression = @"
CALCULATE(
    DISTINCTCOUNT('stu_award'[ID_NUM]),
    'ATTRIBUTE_TRANS'[ID_NUM]
)",
        Category = "Scholarship Analysis",
        FormatString = "#,0"
    },
    new {
        Name = "Athletes without Scholarships",
        Expression = @"[Total Athletes] - [Athletes with Scholarships]",
        Category = "Scholarship Analysis",
        FormatString = "#,0"
    },
    new {
        Name = "Scholarship Coverage %",
        Expression = @"
DIVIDE(
    [Athletes with Scholarships],
    [Total Athletes],
    0
) * 100",
        Category = "Scholarship Analysis",
        FormatString = "0.0%"
    },
    new {
        Name = "Min Scholarship",
        Expression = @"MIN('stu_award'[TOTAL_AWARDED])",
        Category = "Scholarship Analysis",
        FormatString = "$#,0"
    },
    new {
        Name = "Max Scholarship",
        Expression = @"MAX('stu_award'[TOTAL_AWARDED])",
        Category = "Scholarship Analysis",
        FormatString = "$#,0"
    },
    new {
        Name = "Median Scholarship",
        Expression = @"MEDIAN('stu_award'[TOTAL_AWARDED])",
        Category = "Scholarship Analysis",
        FormatString = "$#,0"
    },

    // ============================================
    // CATEGORY: Scholarship by Sport
    // ============================================
    new {
        Name = "Basketball Scholarships",
        Expression = @"
CALCULATE(
    [Total Scholarship Amount],
    'ATTRIBUTE_DEF'[ATTRIB_CDE] = ""BSKTB""
)",
        Category = "Scholarship by Sport",
        FormatString = "$#,0"
    },
    new {
        Name = "Track Scholarships",
        Expression = @"
CALCULATE(
    [Total Scholarship Amount],
    'ATTRIBUTE_DEF'[ATTRIB_CDE] = ""TRACK""
)",
        Category = "Scholarship by Sport",
        FormatString = "$#,0"
    },
    new {
        Name = "Volleyball Scholarships",
        Expression = @"
CALCULATE(
    [Total Scholarship Amount],
    'ATTRIBUTE_DEF'[ATTRIB_CDE] = ""VBALL""
)",
        Category = "Scholarship by Sport",
        FormatString = "$#,0"
    },
    new {
        Name = "Baseball Scholarships",
        Expression = @"
CALCULATE(
    [Total Scholarship Amount],
    'ATTRIBUTE_DEF'[ATTRIB_CDE] = ""BASEB""
)",
        Category = "Scholarship by Sport",
        FormatString = "$#,0"
    },

    // ============================================
    // CATEGORY: Demographic Analysis
    // ============================================
    new {
        Name = "Average Age",
        Expression = @"
AVERAGE(
    DATEDIFF('BIOGRAPH_MASTER'[BIRTH_DTE], TODAY(), YEAR)
)",
        Category = "Demographic Analysis",
        FormatString = "#,0"
    },
    new {
        Name = "Gender Distribution %",
        Expression = @"
DIVIDE(
    DISTINCTCOUNT('BIOGRAPH_MASTER'[ID_NUM]),
    CALCULATE(DISTINCTCOUNT('BIOGRAPH_MASTER'[ID_NUM]), ALL('BIOGRAPH_MASTER'[GENDER])),
    0
) * 100",
        Category = "Demographic Analysis",
        FormatString = "0.0%"
    },
    new {
        Name = "Unique Ethnic Groups",
        Expression = @"DISTINCTCOUNT('BIOGRAPH_MASTER'[ETHNIC_GROUP])",
        Category = "Demographic Analysis",
        FormatString = "#,0"
    },

    // ============================================
    // CATEGORY: Trend Analysis
    // ============================================
    new {
        Name = "YoY Athlete Growth",
        Expression = @"
VAR CurrentYearAthletes = [Total Athletes]
VAR PreviousYearAthletes =
    CALCULATE(
        [Total Athletes],
        DATEADD('PF_POE_DEF'[POE_START_DTE], -1, YEAR)
    )
RETURN
    CurrentYearAthletes - PreviousYearAthletes",
        Category = "Trend Analysis",
        FormatString = "#,0"
    },
    new {
        Name = "YoY Athlete Growth %",
        Expression = @"
VAR CurrentYearAthletes = [Total Athletes]
VAR PreviousYearAthletes =
    CALCULATE(
        [Total Athletes],
        DATEADD('PF_POE_DEF'[POE_START_DTE], -1, YEAR)
    )
RETURN
    DIVIDE(
        CurrentYearAthletes - PreviousYearAthletes,
        PreviousYearAthletes,
        0
    ) * 100",
        Category = "Trend Analysis",
        FormatString = "0.0%"
    },
    new {
        Name = "YoY Scholarship Change",
        Expression = @"
VAR CurrentYearScholarships = [Total Scholarship Amount]
VAR PreviousYearScholarships =
    CALCULATE(
        [Total Scholarship Amount],
        DATEADD('PF_POE_DEF'[POE_START_DTE], -1, YEAR)
    )
RETURN
    CurrentYearScholarships - PreviousYearScholarships",
        Category = "Trend Analysis",
        FormatString = "$#,0"
    },
    new {
        Name = "YoY GPA Change",
        Expression = @"
VAR CurrentYearGPA = [Average GPA - Athletes]
VAR PreviousYearGPA =
    CALCULATE(
        [Average GPA - Athletes],
        DATEADD('PF_POE_DEF'[POE_START_DTE], -1, YEAR)
    )
RETURN
    CurrentYearGPA - PreviousYearGPA",
        Category = "Trend Analysis",
        FormatString = "0.00"
    },

    // ============================================
    // CATEGORY: Comparative Analysis
    // ============================================
    new {
        Name = "Scholarship Athletes Avg GPA",
        Expression = @"
CALCULATE(
    [Average GPA - Athletes],
    'stu_award'[ID_NUM]
)",
        Category = "Comparative Analysis",
        FormatString = "0.00"
    },
    new {
        Name = "Non-Scholarship Athletes Avg GPA",
        Expression = @"
CALCULATE(
    [Average GPA - Athletes],
    NOT('stu_award'[ID_NUM])
)",
        Category = "Comparative Analysis",
        FormatString = "0.00"
    },
    new {
        Name = "GPA Difference (Scholarship vs Non)",
        Expression = @"[Scholarship Athletes Avg GPA] - [Non-Scholarship Athletes Avg GPA]",
        Category = "Comparative Analysis",
        FormatString = "0.00"
    },
    new {
        Name = "Male to Female Ratio",
        Expression = @"
DIVIDE(
    [Male Athletes],
    [Female Athletes],
    0
)",
        Category = "Comparative Analysis",
        FormatString = "0.00"
    }
};

// Create a measure table if it doesn't exist
var measureTableName = "📊 Athletics Measures";
var measureTable = Model.Tables.FirstOrDefault(t => t.Name == measureTableName);
if (measureTable == null)
{
    measureTable = Model.AddTable(measureTableName);
    measureTable.Description = "Central location for all athletics dashboard measures";
}

// Track creation results
int created = 0;
int skipped = 0;
int errors = 0;

Info($"Starting measure creation in table '{measureTableName}'...\n");

// Create measures
foreach (var m in measures)
{
    try
    {
        // Check if measure already exists
        var existingMeasure = measureTable.Measures.FirstOrDefault(measure => measure.Name == m.Name);

        if (existingMeasure != null)
        {
            Info($"⚠️  SKIPPED: '{m.Name}' already exists");
            skipped++;
        }
        else
        {
            // Create new measure
            var newMeasure = measureTable.AddMeasure(m.Name, m.Expression);
            newMeasure.DisplayFolder = m.Category;
            newMeasure.FormatString = m.FormatString;

            Info($"✓ CREATED: '{m.Name}' in category '{m.Category}'");
            created++;
        }
    }
    catch (Exception ex)
    {
        Error($"✗ ERROR creating '{m.Name}': {ex.Message}");
        errors++;
    }
}

// Output summary
Info("\n========================================");
Info("MEASURE CREATION SUMMARY");
Info("========================================");
Info($"✓ Created: {created} measures");
Info($"⚠️  Skipped: {skipped} measures (already exist)");
Info($"✗ Errors: {errors} measures");
Info($"Total Processed: {measures.Length} measures");
Info("========================================");
Info($"\nAll measures have been added to the '{measureTableName}' table!");
Info("Measures are organized in the following categories:");
Info("  • Athlete Counts");
Info("  • Academic Performance");
Info("  • Scholarship Analysis");
Info("  • Scholarship by Sport");
Info("  • Demographic Analysis");
Info("  • Trend Analysis");
Info("  • Comparative Analysis");
