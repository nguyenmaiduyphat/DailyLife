/// <summary>
/// Factory Method Pattern
/// </summary>

public interface IShop
{
    void Exit();
    void Reload();
    void ResetFilter();
    void ResetType();
    void FilterType(string value);
    void FilterCategory(int index);
    void FilterPrice(string value);
    void SortName(int index);
    void SearchName(string value);
}
