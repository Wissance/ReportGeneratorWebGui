# ReportGeneratorWebGui
Web application for reports (data from stored procedure or view with parameters) generation using ReportGenerator library.
Reports could be generated in following formats using template files:
* Excel
* Csv

To generate report we need just 2 things:
* Execution config - .xml file that describes stored procedure / view and it parameters
* Template file - a csv or xlsx file

If you would like to add you own reports add them to appropriate subfolder in wwwroot/files

**Key features** are:
1. Web api that allow to call report generation from any client
2. Client GUI that allows to generate reports using built in GUI
3. Report could be executed using any database (Sql Server, MySql or Postgres) on any server that accessible from machine where ReportGeneratorWebGui is deployed

Screnshoots of how it looks:

1. Choose using parameters and Excel template files:

![Choose params ant template file](https://github.com/EvilLord666/ReportGeneratorWebGui/blob/master/img/list-of-files.png)

2. Run parameters set or not

![Set params values](https://github.com/EvilLord666/ReportGeneratorWebGui/blob/master/img/set-params.png)

3. Click OK. In previous View properly set:

- Data Source and Data source Type.
- Excel files positions: worksheet, start row and column (Excel numbers starts from 1)

4. Click Generate, receive .xlsx file with data
