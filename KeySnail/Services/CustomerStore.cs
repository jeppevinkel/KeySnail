using System.Collections.Generic;

namespace KeySnail.Services;

public interface ICustomerStore
{
    List<string> GetAll();
}

public class DbCustomerStore : ICustomerStore
{
    public List<string> GetAll()
    {
        return new List<string>()
        {
            "cust 1",
            "cust 2",
            "cust 3",
        };
    }
}