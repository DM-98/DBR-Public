using System.Linq.Expressions;
using DBR.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace DBR.Core.Helpers;

public static class ExtensionMethods
{
	public static IQueryable<T> QueryHelper<T>(this IQueryable<T> query, bool isTrackingDisabled = true, Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includeProperties = null, Expression<Func<T, T>>? selector = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null) where T : class
	{
		query = isTrackingDisabled ? query.AsNoTracking() : query.AsTracking();

		if (filter is not null)
		{
			query = query.Where(filter);
		}

		if (includeProperties is not null)
		{
			query = includeProperties(query);
		}

		if (selector is not null)
		{
			query = query.Select(selector);
		}

		if (orderBy is not null)
		{
			query = orderBy(query);
		}

		return query;
	}

	public static string FormatTime(this DateTime? dateTime)
	{
		return dateTime.HasValue ? dateTime.Value.ToLocalTime().ToString("d. MMM yyyy kl. HH:mm") : "Uændret";
	}

	public static string FormatTime(this DateTime dateTime)
	{
		return dateTime.ToLocalTime().ToString("d. MMM yyyy kl. HH:mm");
	}

	public static string EnumToString(this IncidentType incidentType)
	{
		if (incidentType is IncidentType.InfoIntern)
		{
			return "Intern information";
		}

		if (incidentType is IncidentType.InfoCustomer)
		{
			return "Information til kunden";
		}

		if (incidentType is IncidentType.QuestionCustomer)
		{
			return "Spørgsmål til kunden";
		}

		if (incidentType is IncidentType.Invoice)
		{
			return "Faktura";
		}

		return string.Empty;
	}
}