# Eating Habit Analyzer

## Business Opportunity

Current tools for tracking food consumption focus on hitting your daily targets and disregard how you feel. Analyzing what your habits are at a high level, and finding if there are patterns that can be used to your advantage will lead to a more sustainable diet in terms of your health, and can prevent over consumption and/or purchasing of food leading to excess waste.

## UN Sustainable Development Goals (SDG)

3: Good Health and Well Being <br/>
12: Responsible Consumption and Production

# Documentation
1. [Activity Based Schedule](./Documentation/PdfVersions/Activity-Based%20Schedule.pdf)
2. [Business Case](./Documentation/PdfVersions/Business%20Case.pdf)
3. [Initial ERD](./Documentation/PdfVersions/EhaErd.drawio.png)
4. [Low Fidelity Prototypes](./Documentation/PdfVersions/LoFis.pdf)
5. [Requirements](./Documentation/PdfVersions/Project%20Requirements.pdf)
6. [Scope Statement](./Documentation/PdfVersions/Project%20Scope%20Statement.pdf)
7. Status Reports
   1. [November 2, 2023](./Documentation/PdfVersions/Project%20Status%20Report%20One.pdf)
   2. [November 16, 2023](./Documentation/PdfVersions/Project%20Status%20Report%20Two.pdf)
   3. [December 5, 2023](./Documentation/PdfVersions/FinalCheckin.pdf)
8. [Stakeholder Analysis](./Documentation/PdfVersions/Stakeholder%20Analysis.pdf)
9. [Technology Inventory](./Documentation/PdfVersions/Technology%20configuration%20inventory.pdf)
10. [Drafting an Emerging Picture](./Documentation/PdfVersions/Drafting%20an%20emerging%20picture.pdf)
11. [Overall System Look](./Documentation/PdfVersions/OverallSystem.pdf)

## Repository Setup
Under the overarching solution there is the API and the App, the API contains all the code running in the Azure cloud. The APP contains all the MAUI code using Event Driven design. <br/>

### Things that could improve code structure over time

1. Split the services into their own solution (sharing models etc)
2. Create a service that models the API for the front end instead of having it directly in the event handlers

# Vlog Link

1. [First Vlog](https://youtu.be/qXECqdHdWP8)

# Install Guide
To run in debug, run using Visual Studio 2022 in either Windows or Android targets and selected the EatingHabitAnalyzerApp as the startup project <br/>
**PLEASE NOTE THAT IOS IS NOT SUPPORTED AT THIS TIME** <br/>

## Publishing a Windows version

Right click on the EatingHabitAnalyzer and select publish, the default publish settings will work. 

## API

The API portion is hosted online as of the writing of this document, and an independent backend will not be required. If an independent back end is required, you will need to create the appsettings.json file, including connections strings as well as run the sql script within the EatingHabitAnalyzer project to create the backend. The original deployment was done via the Azure cloud on a linux app service, with an SQL database on a separate server.

# Technical Notes

Foods are obtained from the [OpenFoodFacts API](https://world.openfoodfacts.org/) when required.

Deployment of the Database and WebAPI is done VIA the azure cloud using the Student tier and a "serverless" architecture.

The front end for this application is Built on MAUI with a focus on Android devices.

A known issue currently is that some foods do not contain the details needed or the details are wrong. E.g. Presidents choice hummus has 1120 calories in a 30g serving, with 90g of salt. This could be partially solved by allowing users to submit changes to foods in the DB but is beyond the current scope of the project.
