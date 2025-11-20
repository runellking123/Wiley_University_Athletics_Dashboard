# Wiley University Athletics Dashboard

Comprehensive sports analytics data for Wiley University athletic programs covering academic years 2017-2025.

## Overview

This repository contains detailed analytics on student-athlete participation, scholarship distribution, and academic performance across four major sports programs:
- Basketball
- Track & Field
- Volleyball
- Baseball

## Data Files

### 1. Athletes_Master_By_Term_2017_2025.csv
Complete roster of all student-athletes organized by academic term.

**Fields:**
- Student ID and personal information (name, email)
- Term and award year
- Sport code and description
- Demographics (gender, birth date, ethnicity)
- Academic information (class code, division, GPA)
- Enrollment data (entrance year/term)
- Academic progress (career hours earned/attempted)

### 2. Athletic_Scholarships_By_Term_2017_2025.csv
All athletic scholarship awards with dollar amounts by term.

**Fields:**
- Student ID
- Term and award year
- Fund code and description
- Amount awarded and earned
- Release status
- Sport identified from fund description

### 3. Athletes_With_Scholarships_By_Term_2017_2025.csv
Combined view linking athletes to their scholarship awards.

**Fields:**
- All athlete master data
- Total scholarship amount awarded
- Number of scholarships received
- Sport identified from both attributes and fund descriptions

### 4. Participation_Summary_By_Term_Sport.csv
Aggregated participation statistics by term and sport.

**Fields:**
- Term and award year
- Sport
- Total athletes
- Male/female athlete counts
- Average career GPA
- Average hours earned

### 5. Scholarship_Summary_By_Term_Sport.csv
Aggregated scholarship statistics by term and sport.

**Fields:**
- Term and award year
- Sport
- Total scholarship athletes
- Total scholarship dollars awarded
- Average, minimum, and maximum scholarship amounts

### 6. Academic_Performance_By_Term_Sport_Scholarship.csv
Academic performance analysis comparing scholarship vs non-scholarship athletes.

**Fields:**
- Term, year, and sport
- Scholarship status (On Scholarship / No Scholarship)
- Total students
- Average career GPA and hours earned/attempted
- GPA distribution breakdown (3.5+, 3.0-3.5, 2.5-3.0, <2.5)

## Data Sources

### Database Tables Used:
- **ATTRIBUTE_TRANS** - Sport participation tracking
- **ATTRIBUTE_DEF** - Sport attribute definitions
- **PF_STDNT_AWARD** - Financial aid awards
- **PF_POE_DEF** - Period of enrollment (terms)
- **PF_FUND_CDE_MSTR** - Scholarship fund definitions
- **NAME_MASTER** - Student names and contact information
- **BIOGRAPH_MASTER** - Student demographics
- **STUDENT_MASTER** - Enrollment and classification
- **STUDENT_DIV_MAST** - Academic division and GPA data

## Key Findings

### Recent Participation (Fall 2025)
- Basketball: 77 athletes
- Track & Field: 91 athletes
- Baseball: 39 athletes
- Volleyball: 8 athletes

### Scholarship Investment (2017-2025)
Athletic scholarships totaling millions of dollars distributed across all sports programs with detailed tracking by term.

## Data Organization

All data is organized by **academic term** (Fall, Spring, Summer) rather than by calendar year to provide accurate roster counts for each season. This ensures team sizes reflect actual participation during specific enrollment periods.

## Usage Notes

- All student data should be handled according to FERPA regulations
- Career GPA represents cumulative GPA across all academic work
- Scholarship amounts represent total awarded per term
- Some students may participate in multiple sports (multi-sport athletes)
- Term dates vary slightly by program (MAST, ADCP, SCP programs excluded from main analysis)

## Generated

Analysis completed: November 20, 2025
Data range: Academic Years 2017-2025
Database: TMSEPRD (Jenzabar EX)
