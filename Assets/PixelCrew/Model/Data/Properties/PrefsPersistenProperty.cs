namespace PixelCrew.Model.Data.Properties
{
    public abstract class PrefsPersistenProperty <TPropertyType> : PersistenProperty<TPropertyType>
    {
        protected string Key;
        protected PrefsPersistenProperty(TPropertyType defaultValue, string key) : base(defaultValue)
        {
            Key = key;
        }
    }
}