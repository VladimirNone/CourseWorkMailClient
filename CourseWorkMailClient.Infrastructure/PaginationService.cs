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
                if (value < 1)
                    page = 1;
                else if (value > MaxCountOfPage)
                    page = MaxCountOfPage;
                else
                    page = value;

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