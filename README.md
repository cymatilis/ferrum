## Ferrum

> ### Noun (Latin)
> **ferrum** n  (_genitive_  **[ferrī](https://en.wiktionary.org/wiki/ferri#Latin "ferri")**);  _[second declension](https://en.wiktionary.org/wiki/Appendix:Latin_second_declension "Appendix:Latin second declension")_
>
>1.  [iron](https://en.wiktionary.org/wiki/iron "iron")
>2.  any  [tool](https://en.wiktionary.org/wiki/tool "tool")  made of iron

# Payment Gateway API
REST API for authorising card payments against a mocked bank integration. 
1. Authorise a card payment
2. Retrieve a previous transaction.

Data is stored against a local SQL server database using Entity Framework and will seed on run. Basic user authentication on requests. Default user is seeded into DB.

Project is (should be) configured to run in Kestral:
* localhost:5001 - Gateway
* localhost:5003 - FakeBank 
Postman collection (with default user) included for demonstration.

# Mocked Bank API (FakeBank)
To invoke a specific result:

1. Authorised: Use security code 200 and a card number with a valid Luhn check digit.
2. Unknown: Use security code 404.
3. Force an Exception : Use security code 500.
4. Error: Use security code 501.
5. Declined: Any other security code and card number.

Additionally the API will randomly fail (throw an exception) for approx 1 in 4 requests, and response time will vary from ~0.5 to ~4.5 seconds.

*Built in C# .NET targeting .NET Core 3.1*
