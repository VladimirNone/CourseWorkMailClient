using System;

namespace CourseWorkMailClient.Infrastructure
{
    public class PaginationService
    {
        private int page = 1;
        public int Page
        {
            get
            {
                return page;
            }
            set
            {
                page = value < 1 ? 1 : value;
            }
        }
        public int MaxCountOfPage { get; set; }
        public int ItemsOnPage { get; set; } = 5;

        public Action<int, int> ChangingPage { get; set; }

        public void ChangePage(int value)
        {
            Page += value;

            ChangingPage(Page, ItemsOnPage);
        }
    }
}