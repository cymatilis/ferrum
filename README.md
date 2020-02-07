## Ferrum

> ### Noun
> **ferrum** n  (_genitive_  **[ferrī](https://en.wiktionary.org/wiki/ferri#Latin "ferri")**);  _[second declension](https://en.wiktionary.org/wiki/Appendix:Latin_second_declension "Appendix:Latin second declension")_
>
>1.  [iron](https://en.wiktionary.org/wiki/iron "iron")
>2.  any  [tool](https://en.wiktionary.org/wiki/tool "tool")  made of iron

# Payment Gateway API
REST API for authorising card payments against a mocked bank integration.

# Mocked Bank API (FakeBank)
To invoke a specific result:

1. Authorised: Use security code 200 and a card number with a valid Luhn check digit.
2. Unknown: Use security code 404.
3. Force an Exception : Use security code 500.
4. Error: Use security code 501.
5. Declined: Any other security code and card number.

Additionally the API will randomly fail (throw and exception) for approx 1 in 4 requests, and response time will vary from ~0.5 to ~4.5 seconds.

*Built in C# .NET targeting .NET Core 3.1*
