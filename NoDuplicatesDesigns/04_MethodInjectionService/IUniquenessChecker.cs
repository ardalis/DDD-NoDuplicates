namespace NoDuplicatesDesigns._04_MethodInjectionService
{
    public interface IUniquenessChecker
    {
        void ValidateNameIsUnique(string name, Product productBeingUpdated);
    }
}