// Tabular Editor 3 Script: Extract All Relationships from Power BI Model
// This script exports all relationships to a CSV file and displays them in the output

using System.IO;

// Define output file path
string outputPath = @"C:\Users\ruking\Downloads\PowerBI_Relationships_Export.csv";

// Create CSV header
var sb = new System.Text.StringBuilder();
sb.AppendLine("Relationship Name,From Table,From Column,To Table,To Column,Cardinality,Cross Filter Direction,Is Active,Security Filtering");

// Iterate through all relationships in the model
foreach (var rel in Model.Relationships)
{
    string relName = rel.Name;
    string fromTable = rel.FromTable.Name;
    string fromColumn = rel.FromColumn.Name;
    string toTable = rel.ToTable.Name;
    string toColumn = rel.ToColumn.Name;
    string cardinality = rel.FromCardinality.ToString() + " to " + rel.ToCardinality.ToString();
    string crossFilter = rel.CrossFilteringBehavior.ToString();
    string isActive = rel.IsActive.ToString();
    string securityFiltering = rel.SecurityFilteringBehavior.ToString();

    // Add to CSV
    sb.AppendLine($"\"{relName}\",\"{fromTable}\",\"{fromColumn}\",\"{toTable}\",\"{toColumn}\",\"{cardinality}\",\"{crossFilter}\",\"{isActive}\",\"{securityFiltering}\"");

    // Output to console
    Output($"Relationship: {fromTable}[{fromColumn}] --> {toTable}[{toColumn}]");
    Output($"  Cardinality: {cardinality}");
    Output($"  Cross Filter: {crossFilter}");
    Output($"  Active: {isActive}");
    Output("");
}

// Write to CSV file
File.WriteAllText(outputPath, sb.ToString());

// Summary
Output("===========================================");
Output($"Total Relationships: {Model.Relationships.Count}");
Output($"Export saved to: {outputPath}");
Output("===========================================");
