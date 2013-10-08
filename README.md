BLToolkit.AzureSql
==================

Business Logic Toolkit AzureSql Data Provider for .NET &ndash; [www.bltoolkit.net](http://www.bltoolkit.net/)

#### Install from NuGet gallery

    PM> Install-Package BLToolkit.AzureSql

#### Configuration in code...

    DbManager.AddDataProvider(new AzureSqlDataProvider());
    DbManager.AddConnectionString(ProviderName.AzureSql, "your connection string to Windows Azure SQL Database");

#### ...or via config file

    <?xml version="1.0" encoding="utf-8" ?>
    <configuration>
      <configSections>
        <section name="bltoolkit" type="BLToolkit.Configuration.BLToolkitSection, BLToolkit.4" />
      </configSections>
      
      <bltoolkit>
        <dataProviders>
          <add type="BLToolkit.Data.DataProvider.AzureSql.AzureSqlDataProvider, BLToolkit.Data.DataProvider.AzureSql" />
        </dataProviders>
      </bltoolkit>
      
      <connectionStrings>
        <add name="default" 
             connectionString="your connection string to Windows Azure SQL Database" providerName="AzureSql" />
      </connectionStrings>
    </configuration>
