[![Build and Test](https://github.com/tramston/pdf-to-word-exporter-api/actions/workflows/build.yml/badge.svg?branch=develop)](https://github.com/tramston/pdf-to-word-exporter-api/actions/workflows/build.yml)

# Word to Pdf Exporter Api Repository

## About
This Project came as a need to export Word documents to PDF programmatically, while via LibreOffice we already had found a solution the problem is there are layout inconsistencies between Word and LibreOffice, so we felt like it's better to go back to finding a solution via Word, our solution was as follows:
* Developed the Word To Pdf Exporter Web API
* Host it in a Windows Server that has access to Microsoft Office Word.
* Make requests to the api and process them here.
* We should only be able to do one export at a time, so we will be using SQLite to manage to keep track of the pending requests and their statuses.

## 🚀 Quick start
* Clone the Repository
* Open solution in your preferred IDE/Text Editor
* Update Environment Variables by updating the [appsettings.Development.json](src/WordToPdfExporter.Api/appsettings.Development.json) file if needed.
* Run the project

## Conventions
All Database table names and columns should use `snake_case` naming convention.<br/>
All methods Should use `PascalCase()` naming convention.<br/>
All classes should use `class PascalCase` naming convention.<br/>
Private variables should have the `_underscorePrivateVariable` naming convention.<br/>
All other variables should use `var camelCase` naming convention.<br/>
