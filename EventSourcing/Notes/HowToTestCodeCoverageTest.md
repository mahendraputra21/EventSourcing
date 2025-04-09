Since you have a Cobertura XML coverage report (coverage.cobertura.xml), here are several ways to view it with a better UI in your browser:
## Using ReportGenerator (Recommended)
1. Install ReportGenerator:

```bash
dotnet tool install -g dotnet-reportgenerator-globaltool
```
2. Generate HTML Report:
- make sure your active directory in C:\Projects\source-code\rnd\EventSourcing\EventSourcingTest\TestResults\b442c333-5622-4067-a91c-76295da2f5c9
```bash
reportgenerator -reports:coverage.cobertura.xml -targetdir:coveragereport -reporttypes:Html -license:YOUR_KEY 
```

3. Open in Browser:
```bash
start coveragereport\index.html
```