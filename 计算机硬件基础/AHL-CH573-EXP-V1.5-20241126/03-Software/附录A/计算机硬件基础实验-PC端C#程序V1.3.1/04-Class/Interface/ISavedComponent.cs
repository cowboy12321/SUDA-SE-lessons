using GEC_LAB._04_Class.Models;

namespace GEC_LAB._04_Class.Interface
{
    public interface ISavedComponent
    {
        public SavedComponent save();
        public void restore(object? data);
    }
}
