﻿namespace API.DataAccessLayer
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}
