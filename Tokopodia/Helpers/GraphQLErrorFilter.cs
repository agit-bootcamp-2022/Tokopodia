using System;
using HotChocolate;

namespace Tokopodia.Helper
{
  public class GraphQLErrorFilter : IErrorFilter
  {
    public IError OnError(IError error)
    {
      if (error.Exception is UserNotFoundException ex)
        return error.WithMessage($"Invalid username or password.");
      if (error.Exception is DuplicateUsername e)
        return error.WithMessage($"Username already used.");
      if (error.Exception is DataNotFound dnf)
        return error.WithMessage($"Data tidak ditemukan.");
      if (error.Exception is UserLockedException ule)
        return error.WithMessage($"User status locked.");
      if (error.Exception is DataFailed df)
        return error.WithMessage(df.Message);

      if (error.Exception is PositionNotInserted pos)
        return error.WithMessage($"User belum memasukan posisi.");
      if (error.Exception is ValueNegative neg)
        return error.WithMessage($"Value Tidak Boleh Negatif.");
      if (error.Exception is ProductNotFound pnf)
        return error.WithMessage($"Product Tidak Ditemukan");

      if (error.Exception is CartNotFound crt)
        return error.WithMessage($"Cart data could not be found");
      if (error.Exception is ShippingNotFound crc)
        return error.WithMessage($"Shipping Type not found");
      if (error.Exception is NotAccess cre)
        return error.WithMessage($"Data in the cart does not belong to the buyer");

      return error;
    }

  }

  public class UserNotFoundException : Exception
  {
    public int BookId { get; internal set; }
  }

  public class DuplicateUsername : Exception { }

  public class DataNotFound : Exception { }
  public class UserLockedException : Exception { }  
  
  public class DataFailed : Exception
  {
    public DataFailed(string message) : base(message) { }
  }

  //Product
  public class PositionNotInserted : Exception { }
  public class ValueNegative : Exception { }
  public class ProductNotFound : Exception { }

  //Cart
  public class CartNotFound : Exception { }
  public class ShippingNotFound : Exception { }
  public class NotAccess : Exception { }
}