using Xamarin.Forms;

namespace SeedApp.Controls
{
    public class NoScrollingListView : ListView
    {
        public NoScrollingListView() : base(ListViewCachingStrategy.RecycleElement)
        {
        }
    }
}