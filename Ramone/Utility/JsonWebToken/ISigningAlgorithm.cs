namespace Ramone.Utility.JsonWebToken
{
  public interface ISigningAlgorithm
  {
    string Sign(string data);
  }
}
