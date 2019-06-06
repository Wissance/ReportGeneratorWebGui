# ReportGeneratorWebGui
ASP NET Core MVC Web GUI (Net core) for easy reports generation  using ReportGenerator library. ReportsGenerator is a fast way to 
select data using database tools (stored procedures and views). It interface now localized only for Ru language, but i am planning to add  
support for En too. By default i copied all params file (.xml) and one template to wwwroot (just for demo purposes).

Screnshoots of how it looks:

1. Choose using parameters and Excel template files:

![Choose params ant template file](https://github.com/EvilLord666/ReportGeneratorWebGui/blob/master/img/list-of-files.png)

2. Run parameters set or not


![Set params values](https://github.com/EvilLord666/ReportGeneratorWebGui/blob/master/img/set-params.png)

3. Click OK. In previous View properly set:

- Data Source and Data source Type.
- Excel files positions: worksheet, start row and column (Excel numbers starts from 1)

4. Click Generate, receive .xlsx file with data
