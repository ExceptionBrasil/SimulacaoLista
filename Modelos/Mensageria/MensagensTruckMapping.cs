using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Mensageria
{
    class MensagensTruckMapping:ClassMap<MensagensTruck>
    {
        public MensagensTruckMapping()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Mensagem);
            Map(x => x.DataCriacao);
        }
    }
}
