# ReportGeneratorWebGui

## 1. Description
Web application for reports (data from stored procedure or view with parameters) generation using ReportGenerator library.
Reports could be generated in following formats using template files:
* Excel
* Csv

To generate report we need just a 2 things:
* Execution config - **.xml** file that describes stored procedure / view and it parameters
* Template file - a **.csv or .xlsx** file

If you would like to add you own reports add them to appropriate subfolder in wwwroot/files

**Key features** are:
1. Web api that allow to call report generation from any client (`POST {baseUrl}/ReportsManager/Generate`)
2. Client GUI that allows to generate reports using built in GUI
3. Report could be executed using any database (Sql Server, MySql or Postgres) on any server 
   that accessible from machine where ReportGeneratorWebGui is deployed

## 2. Build

Backend builds very simple, we don't need any specific actions
Frontend build have one pecularity - manual copy webfonts from `node_modules\font-awesome-5-css\webfonts\` to `wwwroot\webfonts`

To full frontend rebuild (if there were made significant changes):
* npm i
* gulp build

## 3. How to generate reports

Open browser with address: {baseUrl}/ReportsManager, where {baseUrl} is an address on which applcation was started

Ensure that you have database with data and/or stored procedures or views

1. Choose ExecutionConfig and Template file:

![Choose execution config and template files](https://github.com/Wissance/ReportGeneratorWebGui/blob/master/docs/mainpage.png)

2. Set parameters otherwise they will be empty

![Set params values](https://github.com/Wissance/ReportGeneratorWebGui/blob/master/docs/paramsset.png)

3. Click OK. In previous View properly set:

- Data Source and Data source Type.
- Select database type
- Select output generation file type (CSV or Excel)
- Excel files positions: worksheet, start row and column (Excel numbers starts from 1)

4. Click Generate, receive **.xlsx or .csv** file with data
