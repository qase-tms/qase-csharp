# Qase C# Commons

This module is an SDK for developing test reporters for Qase TMS.
It's using `Qase.ApiClient.V1` and `Qase.ApiClient.V2` as an API clients, and all Qase reporters are, in turn, using this package.
You should use it if you're developing your own test reporter for a special-purpose framework.

To report results from tests using a popular framework or test runner,
don't install this module directly and
use the corresponding reporter module instead:

* [xUnit](https://github.com/qase-tms/qase-csharp/tree/main/Qase.XUnit.Reporter#readme)

## Configuration

Qase C# Reporters can be configured in multiple ways:

* using a config file `qase.config.json`
* using environment variables

All configuration options are listed in the table below:

| Description                                                                                                                | Config file                | Environment variable            | CLI option                      | Default value                           | Required | Possible values            |
|----------------------------------------------------------------------------------------------------------------------------|----------------------------|---------------------------------|---------------------------------|-----------------------------------------|----------|----------------------------|
| **Common**                                                                                                                 |                            |                                 |                                 |                                         |          |                            |
| Mode of reporter                                                                                                           | `mode`                     | `QASE_MODE`                     | `QASE_MODE`                     | `off`                                   | No       | `testops`, `report`, `off` |
| Fallback mode of reporter                                                                                                  | `fallback`                 | `QASE_FALLBACK`                 | `QASE_FALLBACK`                 | `off`                                   | No       | `testops`, `report`, `off` |
| Environment                                                                                                                | `environment`              | `QASE_ENVIRONMENT`              | `QASE_ENVIRONMENT`              | undefined                               | No       | Any string                 |
| Root suite                                                                                                                 | `rootSuite`                | `QASE_ROOT_SUITE`               | `QASE_ROOT_SUITE`               | undefined                               | No       | Any string                 |
| Enable debug logs                                                                                                          | `debug`                    | `QASE_DEBUG`                    | `QASE_DEBUG`                    | `False`                                 | No       | `True`, `False`            |
| **Qase Report configuration**                                                                                              |                            |                                 |                                 |                                         |          |                            |
| Driver used for report mode                                                                                                | `report.driver`            | `QASE_REPORT_DRIVER`            | `QASE_REPORT_DRIVER`            | `local`                                 | No       | `local`                    |
| Path to save the report                                                                                                    | `report.connection.path`   | `QASE_REPORT_CONNECTION_PATH`   | `QASE_REPORT_CONNECTION_PATH`   | `./build/qase-report`                   |          |                            |
| Local report format                                                                                                        | `report.connection.format` | `QASE_REPORT_CONNECTION_FORMAT` | `QASE_REPORT_CONNECTION_FORMAT` | `json`                                  |          | `json`, `jsonp`            |
| **Qase TestOps configuration**                                                                                             |                            |                                 |                                 |                                         |          |                            |
| Token for [API access](https://developers.qase.io/#authentication)                                                         | `testops.api.token`        | `QASE_TESTOPS_API_TOKEN`        | `QASE_TESTOPS_API_TOKEN`        | undefined                               | Yes      | Any string                 |
| Qase API host. For enterprise users, specify address: `example.qase.io`                                           | `testops.api.host`         | `QASE_TESTOPS_API_HOST`         | `QASE_TESTOPS_API_HOST`         | `qase.io`                               | No       | Any string                 |
| Qase enterprise environment                                                                                                | `testops.api.enterprise`   | `QASE_TESTOPS_API_ENTERPRISE`   | `QASE_TESTOPS_API_ENTERPRISE`   | `False`                                 | No       | `True`, `False`            |
| Code of your project, which you can take from the URL: `https://app.qase.io/project/DEMOTR` - `DEMOTR` is the project code | `testops.project`          | `QASE_TESTOPS_PROJECT`          | `QASE_TESTOPS_PROJECT`          | undefined                               | Yes      | Any string                 |
| Qase test run ID                                                                                                           | `testops.run.id`           | `QASE_TESTOPS_RUN_ID`           | `QASE_TESTOPS_RUN_ID`           | undefined                               | No       | Any integer                |
| Qase test run title                                                                                                        | `testops.run.title`        | `QASE_TESTOPS_RUN_TITLE`        | `QASE_TESTOPS_RUN_TITLE`        | `Automated run <Current date and time>` | No       | Any string                 |
| Qase test run description                                                                                                  | `testops.run.description`  | `QASE_TESTOPS_RUN_DESCRIPTION`  | `QASE_TESTOPS_RUN_DESCRIPTION`  | `<Framework name> automated run`        | No       | Any string                 |
| Qase test run complete                                                                                                     | `testops.run.complete`     | `QASE_TESTOPS_RUN_COMPLETE`     | `QASE_TESTOPS_RUN_COMPLETE`     | `True`                                  |          | `True`, `False`            |
| Qase test run tags                                                                                                         | `testops.run.tags`         | `QASE_TESTOPS_RUN_TAGS`         | `QASE_TESTOPS_RUN_TAGS`         | `[]`                                    | No       | Array of strings           |
| External link type for test run                                                                                            | `testops.run.externalLink.type` | `QASE_TESTOPS_RUN_EXTERNAL_LINK_TYPE` | `QASE_TESTOPS_RUN_EXTERNAL_LINK_TYPE` | None, don't add external link           | No       | `jiraCloud`, `jiraServer`  |
| External link URL for test run                                                                                             | `testops.run.externalLink.link` | `QASE_TESTOPS_RUN_EXTERNAL_LINK_URL` | `QASE_TESTOPS_RUN_EXTERNAL_LINK_URL` | None, don't add external link           | No       | Any string (e.g., "PROJ-1234") |
| Qase test plan ID                                                                                                          | `testops.plan.id`          | `QASE_TESTOPS_PLAN_ID`          | `QASE_TESTOPS_PLAN_ID`          | undefined                               | No       | Any integer                |
| Size of batch for sending test results                                                                                     | `testops.batch.size`       | `QASE_TESTOPS_BATCH_SIZE`       | `QASE_TESTOPS_BATCH_SIZE`       | `200`                                   | No       | Any integer                |
| Enable defects for failed test cases                                                                                       | `testops.defect`           | `QASE_TESTOPS_DEFECT`           | `QASE_TESTOPS_DEFECT`           | `False`                                 | No       | `True`, `False`            |
| Configuration values for test run                                                                                          | `testops.configurations.values` | `QASE_TESTOPS_CONFIGURATIONS_VALUES` | `QASE_TESTOPS_CONFIGURATIONS_VALUES` | `[]`                                    | No       | Comma-separated key=value pairs |
| Whether to create configuration groups and values if they don't exist                                                      | `testops.configurations.createIfNotExists` | `QASE_TESTOPS_CONFIGURATIONS_CREATE_IF_NOT_EXISTS` | `QASE_TESTOPS_CONFIGURATIONS_CREATE_IF_NOT_EXISTS` | `False`                                 | No       | `True`, `False`            |
| Filter results by status                      | `testops.statusFilter`                | `QASE_TESTOPS_STATUS_FILTER`     | `QASE_TESTOPS_STATUS_FILTER`     | None, don't filter any results          | No       | Comma-separated list of statuses |
| Status mapping for test results               | `statusMapping`                       | `QASE_STATUS_MAPPING`            | `QASE_STATUS_MAPPING`            | None, don't map any statuses            | No       | JSON object or comma-separated key=value pairs |

### Example `qase.config.json` config

```json
{
  "mode": "testops",
  "fallback": "report",
  "debug": false,
  "environment": "local",
  "captureLogs": false,
  "report": {
    "driver": "local",
    "connection": {
      "local": {
        "path": "./build/qase-report",
        "format": "json"
      }
    }
  },
  "testops": {
    "api": {
      "token": "<token>",
      "host": "qase.io"
    },
    "run": {
      "title": "Regress run",
      "description": "Regress run description",
      "complete": true,
      "tags": ["tag1", "tag2"],
      "externalLink": {
        "type": "JiraCloud",
        "link": "PROJ-1234"
      }
    },
    "defect": false,
    "project": "<project_code>",
    "batch": {
      "size": 100
    },
    "configurations": {
      "values": [
        {
          "name": "Browser",
          "value": "Chrome"
        },
        {
          "name": "Environment",
          "value": "Production"
        }
      ],
      "createIfNotExists": true
    },
    "statusFilter": ["passed", "failed"]
  },
  "statusMapping": {
    "invalid": "failed",
    "skipped": "passed"
  }
}
```
