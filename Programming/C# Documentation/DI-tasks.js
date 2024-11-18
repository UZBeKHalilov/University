1. Register and Inject a Simple Service

    Create an interface IGreetingService with a method string GetGreeting().
    Implement the service with GreetingService and register it as a Singleton in Program.cs.
    Inject it into a controller and call it in an endpoint.

2. Switch Between Different Implementations

    Create another implementation of IGreetingService, e.g., FormalGreetingService.
    Use a conditional statement in Program.cs to register a specific implementation based on an environment variable.

3. Use DI with Constructor Parameters

    Create a service TimestampService that requires a DateTime in its constructor.
    Inject TimestampService into a controller and display the current timestamp in an endpoint.

4. Inject Scoped Services

    Create a RequestLoggerService that logs the request ID (use Guid).
    Register it as a Scoped service.
    Inject it into a controller and verify that the same request ID is reused within a single HTTP request.

5. Inject Transient Services

    Create a service RandomNumberService that generates a random number.
    Register it as a Transient service.
    Inject it into a controller and verify that a new number is generated for every method call.

6. Inject Multiple Services into a Controller

    Create a ProductService and an OrderService.
    Inject both services into a single controller.
    Use endpoints to call methods from each service.

Intermediate Tasks
7. Create a Service with Dependency on Another Service

    Create a service DiscountService that depends on ProductService.
    Inject ProductService into DiscountService using constructor injection.
    Use both services in a controller.

8. Use an Interface with Multiple Implementations

    Create an interface ILoggingService and two implementations: ConsoleLoggingService and FileLoggingService.
    Register both implementations using Named Dependency Injection (e.g., using IServiceProvider).
    Inject and switch between the two implementations in a controller based on a query parameter.

9. Configure Options with DI

    Create a custom settings class AppSettings with properties like AppName and Version.
    Use IOptions<AppSettings> to inject configuration values from appsettings.json into a controller.

10. Use DI with Middleware

    Create custom middleware RequestLoggerMiddleware that depends on RequestLoggerService.
    Inject the service into the middleware and log the request details.

11. Implement and Inject a Repository

    Create an interface IProductRepository and its implementation ProductRepository to interact with an in-memory list of products.
    Inject the repository into a service and use the service in a controller.

Advanced Tasks
12. Register and Use Generic Services

    Create a generic interface ICacheService<T> and its implementation MemoryCacheService<T>.
    Register the generic service and inject it into a controller to cache product data.

13. Test DI with Unit Testing

    Use Moq to create a mock of IGreetingService.
    Inject the mock into a controller in a unit test and verify the behavior of the controller.

14. Inject Services Based on Environment

    Register a DatabaseService with different implementations for Development and Production environments using IServiceCollection.AddTransient.

15. Use Factory Pattern with DI

    Create a factory ILoggingServiceFactory to dynamically create instances of ConsoleLoggingService or FileLoggingService.
    Inject the factory into a controller and use it to get the desired logging implementation at runtime.