using Microsoft.Phone.Maps.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheMainEvent_Capstone
{
	static class GeocodeQueryHandler
	{
		public static Task<IList<MapLocation>> ExecuteAsync(this GeocodeQuery query)
		{
			var taskSource = new TaskCompletionSource<IList<MapLocation>>();

			EventHandler<QueryCompletedEventArgs<IList<MapLocation>>> handler = null;

			handler = (s, e) =>
			{
				query.QueryCompleted -= handler;

				if (e.Cancelled)
					taskSource.SetCanceled();
				else if (e.Error != null)
					taskSource.SetException(e.Error);
				else
					taskSource.SetResult(e.Result);
			};

			query.QueryCompleted += handler;

			query.QueryAsync();

			return taskSource.Task;
		}
		
	}
}
