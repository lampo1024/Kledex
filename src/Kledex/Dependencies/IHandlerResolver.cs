﻿using System;

namespace Kledex.Dependencies
{
    public interface IHandlerResolver
    {
        THandler ResolveHandler<THandler>();
        object ResolveHandler(Type handlerType);
        object ResolveCommandHandler(object commnad, Type type);
        object ResolveQueryHandler(object query, Type type);
    }
}
