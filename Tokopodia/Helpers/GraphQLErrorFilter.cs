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
      if (error.Exception is RegistrationFailed rf)
        return error.WithMessage(rf.Message);
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
  public class RegistrationFailed : Exception
  {
    public RegistrationFailed(string message) : base(message) { }
  }
}