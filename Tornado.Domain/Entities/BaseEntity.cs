using System;
using Tornado.Domain.Interfaces;

namespace Tornado.Domain.Entities
{
    public class BaseEntity : IEntity
    { 
        private Guid? _id;

        public Guid Id
        {
            get
            {
                if (_id == null)
                {
                    _id = Guid.NewGuid();
                }

                return _id.Value;
            }
            set
            {
                _id = value;
            }
        }
    }
}
