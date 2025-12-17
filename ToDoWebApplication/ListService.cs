namespace ToDoWebApplication
{
    public class ListService
    {
        public ListService()
        {
            _lastListId = 0;
        }

        private List<ListModel> _listOfLists = new List<ListModel>();
        private int _lastListId;

        public List<ListModel> GetAll()
        {
            return _listOfLists;
        }

        public void AddList(string listName)
        {
            int listId = ++_lastListId;
            ListModel list = new ListModel() { Id = listId, Name = listName };
            _listOfLists.Add(list);
        }

        public bool RemoveList(int listId)
        {
            ListModel list = _listOfLists.FirstOrDefault(l => l.Id == listId);

            if (list == null)
                return false;

            _listOfLists.Remove(list);
            return true;
        }

    }
}
