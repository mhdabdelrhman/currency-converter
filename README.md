
# Currency Converter

Currency Converter is an ASP.NET application that allows users to convert between different currencies.

## Running the Application

You can run the application in three different ways:

### 1. Running with Visual Studio

1. Open the project in Visual Studio.
2. Set the startup project to `CurrencyConverter.Presentation`.
3. Run the application using the `Start Debugging` or `Start Without Debugging` option.
4. The application will be accessible at `http://localhost:5101`.

### 2. Running with Docker

1. Navigate to the root directory of the project where the `Dockerfile` is located.
2. Build and run the Docker container using the following command:

   ```bash
   docker build -t currencyconverter .
   docker run -d --name currencyconverter_container -p 8080:8080 currencyconverter
   ```

3. The application will be accessible at `http://localhost:8080`.

### 3. Running with Make Commands

Make sure you have `make` installed on your system.

- To start the application in Docker, run:

  ```bash
  make start
  ```

  The application will be accessible at `http://localhost:8080`.

- To stop the application, run:

  ```bash
  make stop
  ```

## Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (if running with Visual Studio)
- [Docker](https://www.docker.com/) (if running with Docker)
- [`make`](https://www.gnu.org/software/make/manual/make.html) (if using Make commands)
