// ============================================================================
// TABULAR EDITOR 3 C# SCRIPT - CREATE RELATIONSHIPS & DIMENSION TABLES
// ============================================================================
// Instructions:
//   1. Open your Power BI model in Tabular Editor 3
//   2. Go to File > Open > From DB or open your .bim file
//   3. Press Ctrl+Shift+P or go to Tools > Execute Script
//   4. Paste this script and run (F5)
//   5. Save changes to your model
// ============================================================================

using System.Collections.Generic;

// Track results
int dimensionsCreated = 0;
int relationshipsCreated = 0;
int relationshipsSkipped = 0;
int errors = 0;
var errorMessages = new List<string>();
var createdRelationships = new List<string>();

// ============================================================================
// SECTION 1: CREATE DIMENSION TABLES
// ============================================================================

// Helper function to create a calculated table (dimension)
Action<string, string> CreateDimensionTable = (tableName, daxExpression) =>
{
    try
    {
        // Check if table already exists
        if (Model.Tables.Any(t => t.Name == tableName))
        {
            return; // Skip if exists
        }

        var table = Model.AddCalculatedTable(tableName, daxExpression);
        table.Description = "Dimension table for filtering and slicing";
        dimensionsCreated++;
    }
    catch (Exception ex)
    {
        errorMessages.Add($"Error creating dimension '{tableName}': {ex.Message}");
        errors++;
    }
};

// Create Year Dimension
CreateDimensionTable(
    "Dim_Year",
    @"DISTINCT(
    UNION(
        SELECTCOLUMNS(Scholarship_Summary_By_Term_Sport, ""AwardYear"", [AwardYear]),
        SELECTCOLUMNS(Participation_Summary_By_Term_Sport, ""AwardYear"", [AwardYear]),
        SELECTCOLUMNS(Academic_Performance_By_Term_Sport_Scholarship, ""AwardYear"", [AwardYear])
    )
)"
);

// Create Sport Dimension
CreateDimensionTable(
    "Dim_Sport",
    @"DISTINCT(
    UNION(
        SELECTCOLUMNS(Scholarship_Summary_By_Term_Sport, ""Sport"", [Sport]),
        SELECTCOLUMNS(Participation_Summary_By_Term_Sport, ""Sport"", [Sport]),
        SELECTCOLUMNS(Academic_Performance_By_Term_Sport_Scholarship, ""Sport"", [Sport])
    )
)"
);

// Create Term Dimension
CreateDimensionTable(
    "Dim_Term",
    @"DISTINCT(
    UNION(
        SELECTCOLUMNS(Scholarship_Summary_By_Term_Sport, ""Term"", [Term]),
        SELECTCOLUMNS(Participation_Summary_By_Term_Sport, ""Term"", [Term]),
        SELECTCOLUMNS(Academic_Performance_By_Term_Sport_Scholarship, ""Term"", [Term])
    )
)"
);

// ============================================================================
// SECTION 2: CREATE RELATIONSHIPS
// ============================================================================

// Helper function to create a relationship
Action<string, string, string, string, bool> CreateRelationship = (fromTable, fromColumn, toTable, toColumn, isActive) =>
{
    try
    {
        // Check if tables exist
        var fromTbl = Model.Tables.FirstOrDefault(t => t.Name == fromTable);
        var toTbl = Model.Tables.FirstOrDefault(t => t.Name == toTable);

        if (fromTbl == null)
        {
            errorMessages.Add($"Table '{fromTable}' not found - skipping relationship");
            relationshipsSkipped++;
            return;
        }

        if (toTbl == null)
        {
            errorMessages.Add($"Table '{toTable}' not found - skipping relationship");
            relationshipsSkipped++;
            return;
        }

        // Check if columns exist
        var fromCol = fromTbl.Columns.FirstOrDefault(c => c.Name == fromColumn);
        var toCol = toTbl.Columns.FirstOrDefault(c => c.Name == toColumn);

        if (fromCol == null)
        {
            errorMessages.Add($"Column '{fromColumn}' not found in '{fromTable}' - skipping relationship");
            relationshipsSkipped++;
            return;
        }

        if (toCol == null)
        {
            errorMessages.Add($"Column '{toColumn}' not found in '{toTable}' - skipping relationship");
            relationshipsSkipped++;
            return;
        }

        // Check if relationship already exists
        var existingRel = Model.Relationships.FirstOrDefault(r =>
            (r.FromColumn.Name == fromColumn && r.FromTable.Name == fromTable &&
             r.ToColumn.Name == toColumn && r.ToTable.Name == toTable) ||
            (r.FromColumn.Name == toColumn && r.FromTable.Name == toTable &&
             r.ToColumn.Name == fromColumn && r.ToTable.Name == fromTable)
        );

        if (existingRel != null)
        {
            relationshipsSkipped++;
            return;
        }

        // Create the relationship
        var rel = Model.AddRelationship();
        rel.FromColumn = fromCol;
        rel.ToColumn = toCol;
        rel.IsActive = isActive;
        rel.CrossFilteringBehavior = CrossFilteringBehavior.OneDirection;

        relationshipsCreated++;
        createdRelationships.Add($"{fromTable}[{fromColumn}] -> {toTable}[{toColumn}]");
    }
    catch (Exception ex)
    {
        errorMessages.Add($"Error creating relationship {fromTable}[{fromColumn}] -> {toTable}[{toColumn}]: {ex.Message}");
        errors++;
    }
};

// ============================================================================
// RELATIONSHIPS: Dim_Year to Fact Tables
// ============================================================================

CreateRelationship("Scholarship_Summary_By_Term_Sport", "AwardYear", "Dim_Year", "AwardYear", true);
CreateRelationship("Participation_Summary_By_Term_Sport", "AwardYear", "Dim_Year", "AwardYear", true);
CreateRelationship("Academic_Performance_By_Term_Sport_Scholarship", "AwardYear", "Dim_Year", "AwardYear", true);
CreateRelationship("Athletes_Master_By_Term_2017_2025", "AwardYear", "Dim_Year", "AwardYear", true);
CreateRelationship("Athletes_With_Scholarships_By_Term_2017_2025", "AwardYear", "Dim_Year", "AwardYear", true);
CreateRelationship("Athletic_Scholarships_By_Term_2017_2025", "AwardYear", "Dim_Year", "AwardYear", true);

// ============================================================================
// RELATIONSHIPS: Dim_Sport to Fact Tables
// ============================================================================

CreateRelationship("Scholarship_Summary_By_Term_Sport", "Sport", "Dim_Sport", "Sport", true);
CreateRelationship("Participation_Summary_By_Term_Sport", "Sport", "Dim_Sport", "Sport", true);
CreateRelationship("Academic_Performance_By_Term_Sport_Scholarship", "Sport", "Dim_Sport", "Sport", true);
CreateRelationship("Athletes_Master_By_Term_2017_2025", "Sport", "Dim_Sport", "Sport", true);
CreateRelationship("Athletes_With_Scholarships_By_Term_2017_2025", "SportFromAttribute", "Dim_Sport", "Sport", true);
CreateRelationship("Athletic_Scholarships_By_Term_2017_2025", "SportFromFund", "Dim_Sport", "Sport", true);

// ============================================================================
// RELATIONSHIPS: Dim_Term to Fact Tables
// ============================================================================

CreateRelationship("Scholarship_Summary_By_Term_Sport", "Term", "Dim_Term", "Term", true);
CreateRelationship("Participation_Summary_By_Term_Sport", "Term", "Dim_Term", "Term", true);
CreateRelationship("Academic_Performance_By_Term_Sport_Scholarship", "Term", "Dim_Term", "Term", true);
CreateRelationship("Athletes_Master_By_Term_2017_2025", "Term", "Dim_Term", "Term", true);
CreateRelationship("Athletes_With_Scholarships_By_Term_2017_2025", "Term", "Dim_Term", "Term", true);
CreateRelationship("Athletic_Scholarships_By_Term_2017_2025", "Term", "Dim_Term", "Term", true);

// ============================================================================
// COMPLETION SUMMARY
// ============================================================================

string relationshipList = "";
foreach (var rel in createdRelationships)
{
    relationshipList += $"    • {rel}\n";
}

string summaryMessage = $@"
═══════════════════════════════════════════════════════════════
  RELATIONSHIPS & DIMENSIONS - CREATION COMPLETE
═══════════════════════════════════════════════════════════════

  DIMENSIONS CREATED:
───────────────────────────────────────────────────────────────
    • Dim_Year     (AwardYear values: 2017-2025)
    • Dim_Sport    (Baseball, Basketball, Soccer, etc.)
    • Dim_Term     (Fall, Spring, Summer terms)

  Dimension Tables Created: {dimensionsCreated}

═══════════════════════════════════════════════════════════════

  RELATIONSHIPS SUMMARY:
───────────────────────────────────────────────────────────────
    ✓ Created:   {relationshipsCreated}
    ○ Skipped:   {relationshipsSkipped} (already exist or table not found)
    ✗ Errors:    {errors}

───────────────────────────────────────────────────────────────
  RELATIONSHIPS CREATED:
───────────────────────────────────────────────────────────────
{relationshipList}
═══════════════════════════════════════════════════════════════

  DATA MODEL STRUCTURE:
───────────────────────────────────────────────────────────────

                    ┌─────────────┐
                    │  Dim_Year   │
                    │ (AwardYear) │
                    └──────┬──────┘
                           │
       ┌───────────────────┼───────────────────┐
       │                   │                   │
       ▼                   ▼                   ▼
  ┌─────────┐        ┌─────────┐        ┌─────────┐
  │Scholarsh│        │Particip.│        │Academic │
  │_Summary │        │_Summary │        │_Perform │
  └────┬────┘        └────┬────┘        └────┬────┘
       │                  │                   │
       └──────────────────┼───────────────────┘
                          │
                    ┌─────┴─────┐
                    │ Dim_Sport │
                    │ Dim_Term  │
                    └───────────┘

═══════════════════════════════════════════════════════════════
  NEXT STEPS:
───────────────────────────────────────────────────────────────
  1. Save your model (Ctrl+S)
  2. Add slicers using Dim_Year, Dim_Sport, Dim_Term
  3. These slicers will now filter ALL related tables

═══════════════════════════════════════════════════════════════
";

// Display any errors
if (errors > 0 || relationshipsSkipped > 0)
{
    summaryMessage += "\n  NOTES/WARNINGS:\n";
    foreach (var msg in errorMessages)
    {
        summaryMessage += $"  • {msg}\n";
    }
}

// Show completion message (single popup at the end)
Info(summaryMessage);
