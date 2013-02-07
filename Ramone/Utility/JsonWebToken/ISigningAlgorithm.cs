namespace Ramone.Utility.JsonWebToken
{
  public interface ISigningAlgorithm
  {
    string Name { get; }
    string Sign(string data);
  }
}
