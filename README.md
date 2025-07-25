# BDDTestSuite
BDDTestSuite is an automated end-to-end test suite for the SauceDemo web application, built using .NET 8, Selenium WebDriver, Reqnroll (SpecFlow), and xUnit. It follows the Behavior-Driven Development (BDD) approach, with feature files describing user scenarios and step definitions implementing the automation logic.

## Table of Contents

- Project Structure
- Configuration Management
- Logging
- Utility Classes
- Reporting
- Scenarios & Features

## Project Structure

```
BDDTestSuite/ 
├── Features/ # Gherkin feature files describing scenarios 
├── Models/ # Data models (e.g., Product) 
├── PageObjects/ # Page Object Model classes for UI abstraction 
├── StepDefinitions/ # Step definitions mapping Gherkin steps to code 
├── Utils/ # Utility/helper classes (browser, waits, etc.) 
├── config.json # Test configuration (browser, credentials, etc.) 
├── Hooks.cs # Test lifecycle hooks (setup/teardown)
```

## Configuration Management

Configuration is managed via config.json which contains:

- The base URL of the application
- Browser selection (`chrome`, `firefox`, `edge`)
- Test user credentials (standard, locked-out, etc.)

Example:
```
{
    "url": "<https://www.saucedemo.com/>",
    "browser": "firefox",
    "username": {
        "standard-user": "standard_user",
        "locked-out-user": "locked_out_user"
    },
    "password": {
        "valid": "secret_sauce",
        "invalid": "invalid_pass"
    }
}
```
Configuration is loaded at test startup in ```Hooks``` using ```Microsoft.Extensions.Configuration```.
```
IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddJsonFile("config.json")
    .AddEnvironmentVariables()
    .Build();

string browser = configuration["browser"];
```

## **Logging**

Logging is implemented using Serilog. Logs are written both to the console and to a file (`log.txt`). Each scenario's log entries are tagged with the scenario name for traceability.

- Log output includes timestamps, log level, scenario name, and message.
- Logging is initialized in `Hooks`.
- Each step and page object method logs key actions and errors.

Example log entry:

```[18:27:10 INF] [Scenario: Sort products in ascending order by price] Navigating to the login page: https://www.saucedemo.com/```

## **Utility Classes**

Utility classes in `Utils/` provide reusable helpers:

- `BrowserUtil`: Browser initialization and navigation.
- `WaitUtils`: Explicit waits for elements (visible, clickable, URL changes, etc.).
- `ElementUtils`: JavaScript-based element interactions (e.g., JS click).
- `ReportUtils`: Step node creation and failure logging.
- `ScreenshotUtils`: A utility class that captures full‑page browser screenshots and returns them as Base64‑encoded strings.

These utilities help keep step definitions and page objects clean and robust.

## **Reporting** 

This automation test suite integrates **Extent Reports** and **Serilog** to give you rich, step‑by‑step insights into every test run:

1. **Extent Reports**  
   - Generates an HTML report for each test run.  
   - Automatically attaches a screenshot whenever a test fails, making it easy to see exactly what went wrong.

2. **Serilog Sink for In‑Test Logging**  
   - A custom **Serilog sink** captures all log events emitted during a test.  
   - Log events are queued per test execution and then dequeued when attaching to Extent Report nodes.  
   - Each BDD step node in the report includes its own detailed log output, so you can trace the exact sequence of actions and messages.

### Feature View
![Feature View](BDDTestSuite/Docs/Screenshots/report_feature_view.png)
### Failed Scenario View
![Failed Scenario View](BDDTestSuite/Docs/Screenshots/failed_scenario_view.png)
### Failed Scenario Screenshot View
![Failed Scenario Screenshot View](BDDTestSuite/Docs/Screenshots/failed_scenario_screenshot_view.png)
### Exception Page View
![Exception Page View](BDDTestSuite/Docs/Screenshots/exceptios_page_view.png)

![Sample Report](BDDTestSuite/Docs/SampleReport)

## **Scenarios & Features**

Scenarios are defined in Gherkin feature files under `Features/`:

- **Login** (Login.feature): Valid/invalid login, locked-out user, empty credentials.
- **Inventory** (Inventory.feature): Viewing product details, sorting products by name/price.
- **Cart** (Cart.feature): Adding/removing products, verifying cart contents.
- **Checkout** (Checkout.feature): Form validation, overview page, completing purchases.

Each scenario is mapped to step definitions in `StepDefinitions/`, which use page objects and utilities to interact with the application.




