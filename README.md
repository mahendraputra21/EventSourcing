## 📊 Code Coverage Summary
This project implements event-sourcing for domain-driven aggregates with a strong focus on testability and maintainability. Code coverage is analyzed using Coverlet with ReportGenerator, visualized through detailed metrics. Here’s a summary:

## ✅ Overall Coverage
- Line Coverage: 82.9%
- Branch Coverage: 89.2%
- Method Coverage: 82.6%

There are no risk hotspots detected in the codebase, ensuring that critical paths are well tested.

## 🏦 Focus: BankAccount Aggregate
The BankAccount aggregate is a core domain entity and has achieved exceptional test coverage:
- Line Coverage: 96.1%
- Branch Coverage: 88.4%
- Method Coverage: 100%

Each domain behavior is covered with unit and integration tests, ensuring correct behavior under various scenarios.

## 📈 Metrics Snapshot
| Metric                | Value         |
|-----------------------|---------------|
| Line Coverage         | 96.1%         |
| Branch Coverage       | 88.4%         |
| Method Coverage       | 100% (15/15)  |
| Covered Lines         | 74            |
| Covered Branches      | 23            |
| Covered Methods       | 15 / 15       |
| Cyclomatic Complexity | ≤ 4           |
| Crap Score            | 1–2           |

## 🧪 Test Suite
The solution includes:
- Unit tests for domain logic (BankAccount, Events, ValueObjects)
- Integration tests for end-to-end event scenarios
- All tests pass successfully with zero warnings or errors

## 📷 Sample test results:
- ✅ Open_WithValidParameters_CreatesAccountWithInitialDeposit
- ✅ Open_WithInvalidAccountHolder_ThrowsException
- ✅ Withdraw_WhenInsufficientFunds_ThrowsException

```bash
src/
├── Domain/
│   ├── Aggregates/
│   │   └── BankAccount.cs
│   ├── Events/
│   └── ValueObjects/
└── Program.cs

tests/
├── Unit/
│   ├── BankAccountTest.cs
│   ├── ValueObjectTests.cs
└── Integration/
    └── BankAccountIntegrationTests.cs
```

## 🛠️ Tools Used
- NET Core
- xUnit for testing
- Coverlet for code coverage
- ReportGenerator for coverage reporting
- Visual Studio Test Explorer for test orchestration

## 📷 Preview
![test-coverage](https://github.com/mahendraputra21/EventSourcing/blob/master/images/Screenshot_1.png) 

![test-coverage2](https://github.com/mahendraputra21/EventSourcing/blob/master/images/2.png) 

![test-coverage3](https://github.com/mahendraputra21/EventSourcing/blob/master/images/3.png)

![test-coverage4](https://github.com/mahendraputra21/EventSourcing/blob/master/images/4.png) 

