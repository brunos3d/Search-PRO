#if UNITY_EDITOR
namespace SearchPRO {
	public class InterfaceItem : SearchItem {

		public bool close_on_lost_focus;

		public ISearchInterface search_interface;

		public InterfaceItem(ISearchInterface search_interface, bool close_on_lost_focus) {
			this.search_interface = search_interface;
			this.close_on_lost_focus = close_on_lost_focus;
		}
	}
}
#endif

