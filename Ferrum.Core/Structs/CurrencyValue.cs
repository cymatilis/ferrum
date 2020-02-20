namespace Ferrum.Core.Structs
{
    public struct CurrencyValue
    {
        public string CurrencyCode { get; set; }

        public decimal Value { get; set; }

        public CurrencyValue(string currencyCode, decimal value)
        {
            CurrencyCode = currencyCode;
            Value = value;
        }                
    }    
}
