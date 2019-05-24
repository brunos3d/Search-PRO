#if UNITY_EDITOR
namespace SearchPRO {
	public class InterfaceItem : SearchItem {

		public SearchInterface search_interface;

		public InterfaceItem(SearchInterface search_interface) {
			this.search_interface = search_interface;
		}
	}
}
#endif

