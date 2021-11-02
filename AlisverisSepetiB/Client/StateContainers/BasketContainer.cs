using AlisverisSepetiB.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepetiB.Client.StateContainers
{
    public class BasketContainer
    {
        private List<UrunDTO> _basket = new List<UrunDTO>();
        public List<UrunDTO> basket { get => _basket; }
        public void AddNewItem(UrunDTO item)
        {
            _basket.Add(item);
            NotifyStateChanged();
        }

        public Action OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
