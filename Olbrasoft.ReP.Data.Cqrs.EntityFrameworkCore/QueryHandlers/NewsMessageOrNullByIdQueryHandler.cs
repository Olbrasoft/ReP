﻿using Altairis.ReP.Data.Dtos.NewsMessageDtos;
using Altairis.ReP.Data.Entities;
using Altairis.ReP.Data.Queries;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers;
public class NewsMessageOrNullByIdQueryHandler : BaseQueryHandlerWithProjector<NewsMessage, NewsMessageOrNullByIdQuery, NewsMessageDto?>
{
    public NewsMessageOrNullByIdQueryHandler(IProjector projector, RepDbContext context) : base(projector, context)
    {
    }

    protected override async Task<NewsMessageDto?> GetResultToHandleAsync(NewsMessageOrNullByIdQuery query, CancellationToken token) 
        => await ProjectTo<NewsMessageDto>(Where(nm => nm.Id == query.NewsMessageId)).SingleOrDefaultAsync(token);
}
