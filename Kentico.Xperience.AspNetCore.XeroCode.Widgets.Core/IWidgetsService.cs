namespace Kentico.Xperience.AspNetCore.XeroCode.Widgets.Core
{
    public interface IWidgetsService
    {
        void RegisterAll();

        void Add(string identifier);

        void Remove(string identifier);
    }
}