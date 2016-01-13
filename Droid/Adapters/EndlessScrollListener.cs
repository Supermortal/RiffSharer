using System;
using System.Threading.Tasks;
using Android.Widget;
using Android.Views;
using Android.Support.V7.Widget;

namespace RiffSharer.Droid.Adapters
{
    public abstract class EndlessScrollListener : RecyclerView.OnScrollListener
    {
        // The minimum amount of items to have below your current scroll position
        // before loading more.
        private int _visibleThreshold = 5;
        // The current offset index of data you have loaded
        private int _currentPage = 1;
        // The total number of items in the dataset after the last load
        private int _previousTotalItemCount = 0;
        // True if we are still waiting for the last set of data to load.
        private volatile bool _loading = true;
        // Sets the starting page index
        private int _startingPageIndex = 0;
        private int _totalItemCount = 0;
        private int _visibleItemCount = 0;
        private int _firstVisibleItem = 0;
        private LinearLayoutManager _manager;

        public EndlessScrollListener()
        {
        }

        public EndlessScrollListener(int visibleThreshold)
        {
            _visibleThreshold = visibleThreshold;
        }

        public EndlessScrollListener(int visibleThreshold, int startPage)
        {
            _visibleThreshold = visibleThreshold;
            _startingPageIndex = startPage;
            _currentPage = startPage;
        }

        public EndlessScrollListener(LinearLayoutManager manager)
        {
            _manager = manager;
        }

        public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
        {
            base.OnScrolled(recyclerView, dx, dy);

            _visibleItemCount = recyclerView.ChildCount;
            _totalItemCount = _manager.ItemCount;
            _firstVisibleItem = _manager.FindFirstVisibleItemPosition();

            if (_loading)
            {
                if (_totalItemCount > _previousTotalItemCount)
                {
                    _loading = false;
                    _previousTotalItemCount = _totalItemCount;
                }
            }
            if (!_loading && (_totalItemCount - _visibleItemCount)
                <= (_firstVisibleItem + _visibleThreshold))
            {
                _currentPage++;

                OnLoadMore(_currentPage, _totalItemCount);

                _loading = true;
            }
        }

        // Defines the process for actually loading more data based on page
        public abstract Task<bool> OnLoadMore(int page, int totalItemsCount);

    }
}

