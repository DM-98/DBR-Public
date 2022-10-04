using System.Linq.Expressions;
using DBR.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace DBR.Infrastructure.Helpers;

public static class ExtensionMethods
{
	public static IQueryable<T> QueryHelper<T>(this DbSet<T> table, Expression<Func<T, bool>>? filter = null, Expression<Func<T, T>>? selector = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null) where T : class
	{
		IQueryable<T> query = table;

		if (filter is not null)
		{
			query = query.Where(filter);
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