namespace ShipsInSpace.Logic.Generators
{
    internal interface IGenerator<T>
    {
        T Generate();
    }

    internal interface IGenerator : IGenerator<string>
    {
    }
}