using System.Collections.Generic;

namespace Bronson.Utils
{
    public class PagingObject
    {

        public enum PageSizes : int
        {
            NotSet = 0,
            All = -1,
            Ten = 10,
            Twenty = 20,
            Fifty = 50,
            Hundred = 100,
            TwoFifty = 250
        }

        private int currentPageNo = 1;
        public int CurrentPageNo
        {
            get { return this.currentPageNo; }
            set { currentPageNo = value; }
        }

        public int PageCount
        {
            get
            {
                if (PageSize == PageSizes.All)
                    return 1;
               
                int pagecount = TotalResults/(int) PageSize;
                if ((TotalResults%(int) PageSize) != 0)
                    pagecount++;
                return pagecount;
                
            }
        }

        public int CurrentPageIndex
        {
            get { return CurrentPageNo - 1; }
        }

        /// <summary>
        /// Starting at index 0
        /// </summary>
        public int QueryFirstResultIndex
        {
            get
            {
                if (PageSize == PageSizes.All)
                    return 1;
                
                int pageSize = (int) PageSize;
                return (CurrentPageIndex*pageSize);
                
            }
        }
        public int QueryMaxResults
        {
            get
            {
                if (PageSize != PageSizes.All)
                    return (int)PageSize;
                
                return TotalResults;
            }
        }

        private int totalResults = 0;
        public int TotalResults
        {
            get { return this.totalResults; }
            set { totalResults = value; }
        }

        private PageSizes pageSize = PageSizes.Twenty;
        public PageSizes PageSize
        {
            get { return this.pageSize; }
            set { pageSize = value; }
        }

        public int FirstResult
        {
            get { return (QueryFirstResultIndex + 1); }
        }
        public int LastResult
        {
            get
            {
                int last = FirstResult + (int)PageSize - 1;
                if (last > TotalResults)
                    last = TotalResults;
                return last;
            }
        }
        public string ResultsText
        {
            get
            {
                if (TotalResults == 0)
                    return "No Results Returned";
                if (PageSize == PageSizes.All)
                    return string.Format("All {0} Results Returned", TotalResults);
                
                return string.Format("Results {0} - {1} of {2}", FirstResult, LastResult, TotalResults);
            }
        }

        public bool EnableFirst
        {
            get
            {
                if (PageSize == PageSizes.All)
                    return false;
                return (bool)(CurrentPageIndex != 0);
            }
        }
        public bool EnablePrevious
        {
            get
            {
                if (PageSize == PageSizes.All)
                    return false;
                return (bool)(CurrentPageIndex != 0);
            }
        }
        public bool EnableLast
        {
            get
            {
                if (PageSize == PageSizes.All)
                    return false;
                if (PageCount == 0)
                    return false;
                return (bool)(PageCount != CurrentPageNo);
            }
        }
        public bool EnableNext
        {
            get
            {
                if (PageSize == PageSizes.All)
                    return false;
                if (PageCount == 0)
                    return false;
                return (bool)(PageCount != CurrentPageNo);
            }
        }

        /// <summary>
        /// Creates list of 7 paging items for current paging object 
        /// e.g. if current is 4 and page count is 10 => 1 2 3 (4) 5 6 7
        /// If current is 3 and page count is 3 => 1 2 (3)
        /// If current is 8 and page count is 9 => 3 4 5 6 7 (8) 9
        /// </summary>
        public List<PagingItem> PageItems
        {
            get
            {
                List<PagingItem> lst = new List<PagingItem>();
                if (PageSize != PageSizes.All)
                {
                    int itemsEitherSideOfCurrent = 3; //e.g. this makes 7 items 1 2 3 (4) 5 6 7



                    //Set start page item
                    int working = this.CurrentPageNo - itemsEitherSideOfCurrent;
                    if (working < 1) working = 1;

                    //Set last page item
                    int actualMax = working + (itemsEitherSideOfCurrent * 2);
                    if (actualMax > this.PageCount)
                        actualMax = this.PageCount;

                    //Ensure there are 5 items if possible set decrease the start item if needed. e.g. 4 5 6 7 (8)
                    if ((working > 1) && ((actualMax - (itemsEitherSideOfCurrent * 2)) < working))
                        working = actualMax - (itemsEitherSideOfCurrent * 2);
                    //add extra items at the start as we are on one of the last pages
                    if (working < 1)
                        working = 1; //check working is not now below 1

                    do
                    {
                        if (working > 0)
                        {
                            PagingItem itm = new PagingItem() { PageNo = working };
                            if (working == this.CurrentPageNo)
                            {
                                itm.Current = true;
                            }
                            lst.Add(itm);
                        }
                        working++;
                    } while (working <= actualMax);
                }
                return lst;
            }
        }


    }

    public class PagingItem
    {
        private int pageNo = 1;
        public int PageNo
        {
            get { return this.pageNo; }
            set { pageNo = value; }
        }
        private bool current = false;
        public bool Current
        {
            get { return this.current; }
            set { current = value; }
        }

    }
}
